using System.Text.Json.Serialization;

public class QuoteResponse
{
    [JsonPropertyName("id")]
    public string QuoteId { get; set; } = string.Empty;

    [JsonPropertyName("premium")]
    public decimal Premium { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
}
