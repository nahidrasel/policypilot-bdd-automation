[Binding]
public class QuoteSteps
{
    private readonly QuoteService _quoteService;
    private readonly QuoteScenarioContext _scenarioContext;

    public QuoteSteps(
        QuoteService quoteService,
        QuoteScenarioContext scenarioContext)
    {
        _quoteService = quoteService;
        _scenarioContext = scenarioContext;
    }

    [Given("I have a valid quote request")]
    public void GivenIHaveAValidQuoteRequest()
    {
        _scenarioContext.Request = QuoteTestData.ValidQuote();
    }

    [Given("I have a quote request with an invalid registration")]
    public void GivenIHaveAQuoteRequestWithAnInvalidRegistration()
    {
        _scenarioContext.Request = QuoteTestData.InvalidRegistrationQuote();
    }

    [When("I create the quote")]
    public async Task WhenICreateTheQuote()
    {
        _scenarioContext.Response =
            await _quoteService.CreateQuoteAsync(_scenarioContext.Request);

        _scenarioContext.Quote =
            await JsonHelper.DeserializeAsync<QuoteResponse>(_scenarioContext.Response)
            ?? throw new InvalidOperationException("Quote response was empty.");
    }

    [Then("the API should return 201")]
    public void ThenTheApiShouldReturn201()
    {
        Assert.Equal(201, _scenarioContext.Response.Status);
    }

    [Then("the quote should have an identifier")]
    public void ThenTheQuoteShouldHaveAnIdentifier()
    {
        Assert.False(string.IsNullOrWhiteSpace(_scenarioContext.Quote.QuoteId));
    }

    [Then("the quote premium should be positive")]
    public void ThenTheQuotePremiumShouldBePositive()
    {
        Assert.True(_scenarioContext.Quote.Premium > 0);
    }

    [Then("the quote status should be populated")]
    public void ThenTheQuoteStatusShouldBePopulated()
    {
        Assert.False(string.IsNullOrWhiteSpace(_scenarioContext.Quote.Status));
    }

    [Then("the response should include the invalid registration")]
    public async Task ThenTheResponseShouldIncludeTheInvalidRegistration()
    {
        var responseBody = await _scenarioContext.Response.TextAsync();
        Assert.Contains(
            _scenarioContext.Request.RegistrationNumber,
            responseBody,
            StringComparison.OrdinalIgnoreCase);
    }
}