namespace FACTS.GenericBooking.Api.Models.Auth
{
    public class CreateUserRequest
    {
        /// <summary>
        /// email address
        /// </summary>
        /// <example>youremail@email.com</example>
        public string EmailAddress { get; set; }
        /// <summary>
        /// firstName
        /// </summary>
        /// <example>john</example>
        public string FirstName { get; set; }
        /// <summary>
        /// lastName
        /// </summary>
        /// <example>smith</example>
        public string LastName { get; set; }
        /// <summary>
        /// mobile number
        /// </summary>
        /// <example>0418123123</example>
        public string MobileNumber { get; set; }
        /// <summary>
        /// password
        /// </summary>
        /// <example>YourPassword1!</example>
        public string Password { get; set; }
        /// <summary>
        /// username
        /// </summary>
        /// <example>Username</example>
        public string Username { get; set; }
        //public string CompanyCode { get; set; }
        //public bool? ExternalAuthentication { get; set; }
        //public bool? IsCountryAdmin { get; set; }
        //public bool IsEnabled { get; set; }
        //public string Locale { get; set; }
        //public bool? PasswordExpires { get; set; }
        //public bool? SingleSignon { get; set; }
        //public string UserCode { get; set; }
        //public string UserCodeCreateBy { get; set; }
        //public string UserTypeCode { get; set; }
    }
}