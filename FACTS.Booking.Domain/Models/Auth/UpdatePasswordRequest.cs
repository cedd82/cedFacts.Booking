namespace FACTS.GenericBooking.Domain.Models.Auth
{
    public class UpdatePasswordDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string Username { get; set; }
        //public int UserId { get; set; }
    }
}