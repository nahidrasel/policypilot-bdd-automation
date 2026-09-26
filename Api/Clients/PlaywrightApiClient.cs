
public class PlaywrightApiClient
{
    private IAPIRequestContext _apiContext;
    private IPlaywright _playwright;
    public IAPIRequestContext Context => _apiContext;

    public async Task InitializeAsync()
    {
        var settings = ConfigLoader.Load();
        _playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        _apiContext = await _playwright.APIRequest.NewContextAsync(
            new APIRequestNewContextOptions
            {
                BaseURL  = settings.ApiBaseUrl
            });
    }
    public async Task DisposeAsync()
    {
        if (_apiContext != null)
        {
        await _apiContext.DisposeAsync();
        }
        _playwright.Dispose();
    }
}