namespace snipeit.Tests.Config
{
    // Test settings: read from environment variables, with demo defaults
    public static class TestSettings
    {
        public static string BaseUrl => Environment.GetEnvironmentVariable("SNIPEIT_URL") ?? "https://demo.snipeitapp.com/";
        public static string Username => Environment.GetEnvironmentVariable("SNIPEIT_USER") ?? "admin";
        public static string Password => Environment.GetEnvironmentVariable("SNIPEIT_PASSWORD") ?? "password";
    }
}