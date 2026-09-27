using Reqnroll.BoDi;

[Binding]
public sealed class ApiHooks
{
    [BeforeScenario]
    public async Task InitializeApiClient(IObjectContainer container)
    {
        var apiClient = new PlaywrightApiClient();
        await apiClient.InitializeAsync();

        container.RegisterInstanceAs(apiClient);
        container.RegisterInstanceAs(new QuoteService(apiClient.Context));
        container.RegisterInstanceAs(new QuoteScenarioContext());
    }

    [AfterScenario]
    public async Task DisposeApiClient(PlaywrightApiClient apiClient)
    {
        await apiClient.DisposeAsync();
    }
}