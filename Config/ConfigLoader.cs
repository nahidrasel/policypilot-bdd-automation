public static class ConfigLoader
{
    public static TestSettings Load()
    {
        var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

        return config.Get<TestSettings>();
    }
}