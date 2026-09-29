[Collection("API Collection")]
public class QuoteApiTests
{
    private readonly QuoteService _quoteService;

    public QuoteApiTests(ApiFixture fixture)
    {
        _quoteService = fixture.QuoteService;
    }

    [Fact]
    public async Task ValidQuoteRequestShouldReturnCreated()
    {
        var request = QuoteTestData.ValidQuote();
        var response = await _quoteService.CreateQuoteAsync(request);
        Assert.Equal(201, response.Status);

        var quote = await JsonHelper.DeserializeAsync<QuoteResponse>(response);

        Assert.NotNull(quote);
        Assert.False(string.IsNullOrEmpty(quote!.QuoteId));
        Assert.True(quote.Premium > 0);
        Assert.False(string.IsNullOrWhiteSpace(quote.Status));
    }

    [Fact]
    public async Task MockApiAcceptsInvalidRegistrationWithoutValidation()
    {
        var request = QuoteTestData.InvalidRegistrationQuote();
        var response =await _quoteService.CreateQuoteAsync(request);

        Assert.Equal(201, response.Status);
        var responseBody = await response.TextAsync();
        Assert.Contains("INVALID", responseBody, StringComparison.OrdinalIgnoreCase);
    }
}