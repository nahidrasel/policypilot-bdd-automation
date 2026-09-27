
public class PlaywrightApiClient
{
    private IAPIRequestContext? _apiContext;
    private IPlaywright? _playwright;
    public IAPIRequestContext Context =>
        _apiContext ?? throw new InvalidOperationException("API client is not initialized.");

    public async Task InitializeAsync()
    {
        var settings = ConfigLoader.Load();
        var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        _playwright = playwright;
        _apiContext = await playwright.APIRequest.NewContextAsync(
            new APIRequestNewContextOptions
            {
                BaseURL  = settings.ApiBaseUrl
            });
    }
    public async Task DisposeAsync()
    {
        if (_apiContext is not null)
        {
            await _apiContext.DisposeAsync();
        }

        _playwright?.Dispose();
    }
}