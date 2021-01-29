namespace FACTS.GenericBooking.Api.Models.Auth
{
    public class UpdatePasswordRequest
    {
        /// <summary>
        /// OldPassword
        /// </summary>
        /// <example>YourOldPassword1!</example>
        public string OldPassword { get; set; }
        /// <summary>
        /// NewPassword
        /// </summary>
        /// <example>NewPassword1!</example>
        public string NewPassword { get; set; }
    }
}