namespace FACTS.GenericBooking.Common.Models.Api
{
    public class HttpRequestDeviceDetails
    {
        public string DeviceType { get; set; }
        public string Browser { get; set; }
        public string PlatformOs { get; set; }
        public string UserAgent { get; set; }
        public string IpAddress { get; set; }
    }
}
