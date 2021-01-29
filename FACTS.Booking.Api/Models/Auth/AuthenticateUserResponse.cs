namespace FACTS.GenericBooking.Api.Models.Auth
{
    public class AuthenticateUserResponse
    {
        /// <example>username</example>
        public string UserName { get; set; }
        /// <example>eyJhbGc.Ppmh4c4ELUpzDJyde8.SXbFSjEqre.l1p1UsR.....</example>
        public string AccessToken { get; set; }
    }
}