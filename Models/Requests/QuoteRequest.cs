using System.Text.Json.Serialization;

public class QuoteRequest
{
    [JsonPropertyName("customerId")]
    public required string CustomerId { get; set; }

    [JsonPropertyName("registrationNumber")]
    public required string RegistrationNumber { get; set; }

    [JsonPropertyName("coverType")]
    public required string CoverType { get; set; }
}
