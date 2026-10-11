using System.ComponentModel;

public class QuoteService
{
    private readonly IAPIRequestContext _api;
    public QuoteService(IAPIRequestContext api)
    {
        _api = api;
    }

    public async Task<IAPIResponse> CreateQuoteAsync(QuoteRequest request)
    {
        return await _api.PostAsync(
        "/api/v1/quotes",
        new APIRequestContextOptions
        {
            DataObject = request
        });
    }

    public async Task DeleteQuoteAsync(string quoteId)
    {
        var response = await _api.DeleteAsync(
            $"/api/v1/quotes/{Uri.EscapeDataString(quoteId)}");

        if (response.Status is < 200 or >= 300)
        {
            var responseBody = await response.TextAsync();
            throw new HttpRequestException(
                $"Quote API returned HTTP {response.Status} while deleting quote '{quoteId}': {responseBody}");
        }
    }
}