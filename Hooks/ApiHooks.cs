using Reqnroll.BoDi;

[Binding]
public sealed class ApiHooks
{
    private TestApiServer? _testApiServer;

    [BeforeScenario]
    public async Task InitializeApiClient(IObjectContainer container)
    {
        var apiClient = new PlaywrightApiClient();
        var apiBaseUrl = Environment.GetEnvironmentVariable("API_BASE_URL");
        if (string.IsNullOrWhiteSpace(apiBaseUrl))
        {
            _testApiServer = await TestApiServer.StartAsync();
            apiBaseUrl = _testApiServer.BaseUrl;
        }

        await apiClient.InitializeAsync(apiBaseUrl);

        container.RegisterInstanceAs(apiClient);
        container.RegisterInstanceAs(new QuoteService(apiClient.Context));
        container.RegisterInstanceAs(new QuoteScenarioContext());
    }

    [AfterScenario]
    public async Task DisposeApiClient(
        PlaywrightApiClient apiClient,
        QuoteService quoteService,
        QuoteScenarioContext scenarioContext)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(scenarioContext.Quote?.QuoteId))
            {
                await quoteService.DeleteQuoteAsync(scenarioContext.Quote.QuoteId);
            }
        }
        finally
        {
            try
            {
                await apiClient.DisposeAsync();
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
}