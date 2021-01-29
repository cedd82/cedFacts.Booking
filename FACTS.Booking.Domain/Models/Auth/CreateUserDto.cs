namespace FACTS.GenericBooking.Domain.Models.Auth
{
    public class CreateUserDto
    {
        public string CompanyCode { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string UserCodeCreateBy { get; set; }
        public int UserIdCreatedBy { get; set; }
        public string UserTypeCode { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
    }
}