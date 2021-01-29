namespace FACTS.GenericBooking.Common.Configuration
{
    public class CommonAppSettings
    {
        public string ApiVersion { get; set; }
        public string ApplicationName { get; set; }
        public bool CapturePostRequestBodyOnError { get; set; }
        public string CompanyCode { get; set; }
        public string ContentRootPath { get; set; }
        public string EnvironmentName { get; set; }
        public decimal GST { get; set; }
    }
}