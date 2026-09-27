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
}