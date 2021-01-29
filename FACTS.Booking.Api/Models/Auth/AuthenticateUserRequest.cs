namespace FACTS.GenericBooking.Api.Models.Auth
{
    public class AuthenticateUserRequest
    {
        /// <summary>
        /// username
        /// </summary>
        /// <example>Username</example>
        public string Username { get; set; }
        /// <summary>
        /// password
        /// </summary>
        /// <example>YourPassword1!</example>
        public string Password { get; set; }
    }
}
