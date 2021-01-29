namespace FACTS.GenericBooking.Api.Models.Auth
{
    public class UpdateUserRequest
    {
        public string CompanyCode { get; set; }
        public string EmailAddress { get; set; }
        //public bool? ExternalAuthentication { get; set; }
        //public bool? IsCountryAdmin { get; set; }
        //public bool IsEnabled { get; set; }
        //public string Locale { get; set; }
        //public bool? SingleSignon { get; set; }
        //public string UserCode { get; set; }
        public string UserTypeCode { get; set; }
        public string Username { get; set; }
    }
}