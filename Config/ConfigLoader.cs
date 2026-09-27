public static class ConfigLoader
{
    public static TestSettings Load()
    {
        var config = new ConfigurationBuilder()
        .AddJsonFile("Config/appsetting.json")
        .Build();

        var settings = config.Get<TestSettings>()
            ?? throw new InvalidOperationException("Could not load test settings.");

        if (string.IsNullOrWhiteSpace(settings.ApiBaseUrl))
        {
            throw new InvalidOperationException("ApiBaseUrl is required in Config/appsetting.json.");
        }

        if (string.IsNullOrWhiteSpace(settings.UiBaseUrl))
        {
            throw new InvalidOperationException("UiBaseUrl is required in Config/appsetting.json.");
        }

        return settings;
    }
}