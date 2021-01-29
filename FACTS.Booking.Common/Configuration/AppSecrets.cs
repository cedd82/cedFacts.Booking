namespace FACTS.GenericBooking.Common.Configuration
{
    public class AppSecrets
    {
        public string JwtSymmetricKey { get; set; }
        public bool SwaggerBasicAuthIsEnabled { get; set; }
        public string SwaggerBasicAuthPassword { get; set; }
        public string SwaggerBasicAuthUserName { get; set; }
        public string PostgresConnection { get; set; }
        public string IngresDatabaseConnection { get; set; }
    }
}