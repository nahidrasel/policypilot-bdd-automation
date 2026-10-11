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
        var responseBody = await response.TextAsync();
        Assert.True(
            response.Status == 201,
            $"Expected HTTP 201 but received {response.Status}: {responseBody}");

        var quote = await JsonHelper.DeserializeAsync<QuoteResponse>(response);

        Assert.NotNull(quote);
        try
        {
            Assert.False(string.IsNullOrEmpty(quote!.QuoteId));
            Assert.True(quote.Premium > 0);
            Assert.False(string.IsNullOrWhiteSpace(quote.Status));
        }
        finally
        {
            if (!string.IsNullOrWhiteSpace(quote?.QuoteId))
            {
                await _quoteService.DeleteQuoteAsync(quote.QuoteId);
            }
        }
    }

    [Fact]
    public async Task MockApiAcceptsInvalidRegistrationWithoutValidation()
    {
        var request = QuoteTestData.InvalidRegistrationQuote();
        var response = await _quoteService.CreateQuoteAsync(request);

        var responseBody = await response.TextAsync();
        Assert.True(
            response.Status == 201,
            $"Expected HTTP 201 but received {response.Status}: {responseBody}");

        var quote = await JsonHelper.DeserializeAsync<QuoteResponse>(response);
        Assert.NotNull(quote);
        try
        {
            Assert.Contains("INVALID", responseBody, StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            if (!string.IsNullOrWhiteSpace(quote?.QuoteId))
            {
                await _quoteService.DeleteQuoteAsync(quote.QuoteId);
            }
        }
    }
}