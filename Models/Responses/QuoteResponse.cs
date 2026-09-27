using System.Text.Json.Serialization;

public class QuoteResponse
{
    [JsonPropertyName("id")]
    public string QuoteId { get; set; } = string.Empty;
    public decimal Premium { get; set; }
    public string Status { get; set; } = string.Empty;
}