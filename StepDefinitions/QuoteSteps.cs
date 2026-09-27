[Binding]
public class QuoteSteps
{
    private readonly QuoteService _quoteService;

    private QuoteRequest _request = null!;
    private IAPIResponse _response = null!;
    private QuoteResponse _quoteResponse = null!;

    public QuoteSteps(QuoteService quoteService)
    {
        _quoteService = quoteService;
    }

    [Given("I have a valid quote request")]
    public void GivenIHaveAValidQuoteRequest()
    {
        _request = QuoteTestData.ValidQuote();
    }

    [Given("I have a quote request with an invalid registration")]
    public void GivenIHaveAQuoteRequestWithAnInvalidRegistration()
    {
        _request = QuoteTestData.InvalidRegistrationQuote();
    }

    [When("I create the quote")]
    public async Task WhenICreateTheQuote()
    {
        _response =
            await _quoteService.CreateQuoteAsync(_request);

        _quoteResponse =
            await JsonHelper.DeserializeAsync<QuoteResponse>(_response)
            ?? throw new InvalidOperationException("Quote response was empty.");
    }

    [Then("the API should return 201")]
    public void ThenTheApiShouldReturn201()
    {
        Assert.Equal(201, _response.Status);
    }

    [Then("the quote should have an identifier")]
    public void ThenTheQuoteShouldHaveAnIdentifier()
    {
        Assert.False(string.IsNullOrWhiteSpace(_quoteResponse.QuoteId));
    }

    [Then("the quote premium should be positive")]
    public void ThenTheQuotePremiumShouldBePositive()
    {
        Assert.True(_quoteResponse.Premium > 0);
    }

    [Then("the quote status should be populated")]
    public void ThenTheQuoteStatusShouldBePopulated()
    {
        Assert.False(string.IsNullOrWhiteSpace(_quoteResponse.Status));
    }

    [Then("the response should include the invalid registration")]
    public async Task ThenTheResponseShouldIncludeTheInvalidRegistration()
    {
        var responseBody = await _response.TextAsync();
        Assert.Contains(
            _request.RegistrationNumber,
            responseBody,
            StringComparison.OrdinalIgnoreCase);
    }
}