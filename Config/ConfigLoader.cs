public static class ConfigLoader
{
    public static TestSettings Load()
    {
        var config = new ConfigurationBuilder()
        .AddJsonFile("Config/appsetting.json")
        .Build();

        return config.Get<TestSettings>();
    }
}