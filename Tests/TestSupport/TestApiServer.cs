using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

public sealed class TestApiServer : IAsyncDisposable
{
    private const string QuotesPath = "/api/v1/quotes";
    private static readonly object PortLock = new();
    private readonly HttpListener _listener = new();
    private readonly CancellationTokenSource _shutdown = new();
    private readonly ConcurrentDictionary<string, byte> _quotes = new();
    private readonly Task _serverTask;

    private TestApiServer(int port)
    {
        BaseUrl = $"http://127.0.0.1:{port}";
        _listener.Prefixes.Add($"{BaseUrl}/");
        _listener.Start();
        _serverTask = ListenAsync();
    }

    public string BaseUrl { get; }

    public static Task<TestApiServer> StartAsync()
    {
        lock (PortLock)
        {
            using var portReservation = new TcpListener(IPAddress.Loopback, 0);
            portReservation.Start();
            var port = ((IPEndPoint)portReservation.LocalEndpoint).Port;
            portReservation.Stop();
            return Task.FromResult(new TestApiServer(port));
        }
    }

    private async Task ListenAsync()
    {
        while (!_shutdown.IsCancellationRequested)
        {
            HttpListenerContext context;
            try
            {
                context = await _listener.GetContextAsync().WaitAsync(_shutdown.Token);
            }
            catch (OperationCanceledException) when (_shutdown.IsCancellationRequested)
            {
                break;
            }
            catch (HttpListenerException) when (_shutdown.IsCancellationRequested)
            {
                break;
            }

            await HandleRequestAsync(context);
        }
    }

    private async Task HandleRequestAsync(HttpListenerContext context)
    {
        var path = context.Request.Url?.AbsolutePath.TrimEnd('/');

        if (context.Request.HttpMethod == "POST" && path == QuotesPath)
        {
            await CreateQuoteAsync(context);
            return;
        }

        if (context.Request.HttpMethod == "DELETE" &&
            path is not null &&
            path.StartsWith($"{QuotesPath}/", StringComparison.Ordinal))
        {
            var quoteId = Uri.UnescapeDataString(path[(QuotesPath.Length + 1)..]);
            if (_quotes.TryRemove(quoteId, out _))
            {
                context.Response.StatusCode = (int)HttpStatusCode.NoContent;
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }

            context.Response.Close();
            return;
        }

        await WriteJsonAsync(context, HttpStatusCode.NotFound, new { error = "Route not found." });
    }

    private async Task CreateQuoteAsync(HttpListenerContext context)
    {
        JsonDocument request;
        try
        {
            request = await JsonDocument.ParseAsync(context.Request.InputStream);
        }
        catch (JsonException)
        {
            await WriteJsonAsync(context, HttpStatusCode.BadRequest, new { error = "Invalid JSON request." });
            return;
        }

        using (request)
        {
            if (!request.RootElement.TryGetProperty("registrationNumber", out var registrationNumberElement) ||
                registrationNumberElement.ValueKind != JsonValueKind.String)
            {
                await WriteJsonAsync(
                    context,
                    HttpStatusCode.BadRequest,
                    new { error = "registrationNumber is required." });
                return;
            }

            var quoteId = Guid.NewGuid().ToString("N");
            _quotes.TryAdd(quoteId, 0);
            await WriteJsonAsync(
                context,
                HttpStatusCode.Created,
                new
                {
                    id = quoteId,
                    premium = 123.45m,
                    status = "Created",
                    registrationNumber = registrationNumberElement.GetString()
                });
        }
    }

    private static async Task WriteJsonAsync(
        HttpListenerContext context,
        HttpStatusCode statusCode,
        object payload)
    {
        var responseBody = JsonSerializer.Serialize(payload);
        var bytes = Encoding.UTF8.GetBytes(responseBody);
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";
        context.Response.ContentLength64 = bytes.Length;
        await context.Response.OutputStream.WriteAsync(bytes);
        context.Response.Close();
    }

    public async ValueTask DisposeAsync()
    {
        _shutdown.Cancel();
        _listener.Close();
        await _serverTask;
        _shutdown.Dispose();
    }
}
