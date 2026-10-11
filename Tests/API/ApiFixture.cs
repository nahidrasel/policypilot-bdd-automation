public sealed class ApiFixture : IAsyncLifetime
{
    private readonly PlaywrightApiClient _apiClient = new();
    private TestApiServer? _testApiServer;

    public QuoteService QuoteService { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        var apiBaseUrl = Environment.GetEnvironmentVariable("API_BASE_URL");
        if (string.IsNullOrWhiteSpace(apiBaseUrl))
        {
            _testApiServer = await TestApiServer.StartAsync();
            apiBaseUrl = _testApiServer.BaseUrl;
        }

        await _apiClient.InitializeAsync(apiBaseUrl);
        QuoteService = new QuoteService(_apiClient.Context);
    }

    public async Task DisposeAsync()
    {
        try
        {
            await _apiClient.DisposeAsync();
        }
        finally
        {
            if (_testApiServer is not null)
            {
                await _testApiServer.DisposeAsync();
            }
        }
    }
}

[CollectionDefinition("API Collection")]
public sealed class ApiCollection : ICollectionFixture<ApiFixture>
{
}