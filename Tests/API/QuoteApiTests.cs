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
        var request = new QuoteRequest
        {
            CustomerId = "C1001",
            RegistrationNumber = "ABC123",
            CoverType = "Comprehensive"
        };
        var response = await _quoteService.CreateQuoteAsync(request);
        Assert.Equal(201, response.Status);
    }
}