public class PlaywrightApiClient
{
    private readonly IAPIRequestContext _apiContext;

    public PlaywrightApiClient(IAPIRequestContext apiContext)
    {
        _apiContext = apiContext;
    }
    public IAPIRequestContext Context => _apiContext;
}