namespace FACTS.GenericBooking.Domain.Models.Auth
{
    public class UpdateUserDto
    {
        public string CompanyCode { get; set; }
        public string EmailAddress { get; set; }
        public bool ExternalAuthentication { get; set; }
        public bool IsCountryAdmin { get; set; }
        public bool IsEnabled { get; set; }
        public string Locale { get; set; }
        public bool SingleSignon { get; set; }
        public string UserCodeUpdateBy { get; set; }
        public int UserIdUpdateBy { get; set; }
        public string UserCode { get; set; }
        public string UserTypeCode { get; set; }
        public object Username { get; set; }
    }
}