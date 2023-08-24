namespace BlogAPI
{
    public static class Configuration
    {
        // token - JSON WEB TOKEN
        public static string JwtKey = "I0p3beps/kijsPm3wiMQnw==";

        public static string ApiKeyName = "api_key";

        public static string ApiKey = "curso_api_kij3nwPmpw/MQ==iss";
        public static SmtpConfiguration Smtp = new();

        public class SmtpConfiguration
        {
            public string Host { get; set; }
            public int Port { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
        }
    }
}