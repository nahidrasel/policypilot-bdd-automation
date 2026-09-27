public sealed class QuoteScenarioContext
{
    public QuoteRequest Request { get; set; } = null!;
    public IAPIResponse Response { get; set; } = null!;
    public QuoteResponse Quote { get; set; } = null!;
}