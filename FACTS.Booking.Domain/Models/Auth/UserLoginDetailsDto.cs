namespace FACTS.GenericBooking.Domain.Models.Auth
{
    public class UserLoginDetailsDto
    {
        public string OperatorId { get; set; }
        public string GroupId { get; set; }
        public int IsEnabled { get; set; }
        public string OperatorName { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public int BadLoginCount { get; set; }
        public int IsLocked { get; set; }
    }
}