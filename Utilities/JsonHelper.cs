
using System.Text.Json;

public class JsonHelper
{
    public static async Task<T?> DeserializeAsync<T>(IAPIResponse response)
    {
        var json = await response.TextAsync();
        return JsonSerializer.Deserialize<T>(
            json,new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        );
    }
}