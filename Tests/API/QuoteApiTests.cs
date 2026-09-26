public class QuoteApiTests
{
    private readonly QuoteService _quoteService;
    public QuoteApiTests(QuoteService quoteService)
    {
        _quoteService = quoteService;
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
        Assert.Equal("Created", quote.Status);
    }

    [Fact]
    public async Task InvalidRegistration_ShouldReturnBadRequest()
    {
        var request = QuoteTestData.InvalidRegistrationQuote();

        var response = await _quoteService.CreateQuoteAsync(request);

        Assert.Equal(400, response.Status);

        var error = await JsonHelper.DeserializeAsync<ErrorResponse>(response);

        Assert.NotNull(error);
        Assert.Contains(
            "registration",
            error!.Message,
            StringComparison.OrdinalIgnoreCase);
    }
}