[Binding]
public class QuoteSteps
{
    private readonly QuoteService _quoteService;

    private QuoteRequest _request;
    private IAPIResponse _response;
    private QuoteResponse _quoteResponse;

    public QuoteSteps(QuoteService quoteService)
    {
        _quoteService = quoteService;
    }

    [Given("I have a valid quote request")]
    public void GivenIHaveAValidQuoteRequest()
    {
        _request = QuoteTestData.ValidQuote();
    }

    [When("I create the quote")]
    public async Task WhenICreateTheQuote()
    {
        _response =
            await _quoteService.CreateQuoteAsync(_request);

        _quoteResponse =
            await JsonHelper.DeserializeAsync<QuoteResponse>(
                _response);
    }

    [Then("the API should return 201")]
    public void ThenTheApiShouldReturn201()
    {
        Assert.Equal(201, _response.Status);
    }

    [Then("the quote status should be {string}")]
    public void ThenTheQuoteStatusShouldBe(string expectedStatus)
    {
        Assert.Equal(
            expectedStatus,
            _quoteResponse.Status);
    }
}