public sealed class QuoteApiFixture : IAsyncLifetime
{
    private readonly PlaywrightApiClient _apiClient = new();

    public QuoteService QuoteService { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await _apiClient.InitializeAsync();
        QuoteService = new QuoteService(_apiClient.Context);
    }

    public async Task DisposeAsync()
    {
        await _apiClient.DisposeAsync();
    }
}