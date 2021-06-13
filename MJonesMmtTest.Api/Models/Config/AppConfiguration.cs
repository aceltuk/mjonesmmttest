namespace MJonesMmtTest.Api.Models.Config
{
    /// <summary>
    /// Simple container to hold secrets
    /// </summary>
    public class AppConfiguration
    {
        public string ApiKey { get; set; }

        public string ApiEndpoint { get; set; }
    }
}
