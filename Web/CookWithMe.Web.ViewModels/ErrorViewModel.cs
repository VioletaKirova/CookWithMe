namespace CookWithMe.Web.ViewModels
{
    public class ErrorViewModel
    {
        public string StatusCode { get; set; }

        public string RequestId { get; set; }

        public string RequestPath { get; set; }

        public bool ShowStatusCode => !string.IsNullOrEmpty(this.StatusCode);

        public bool ShowRequestId => !string.IsNullOrEmpty(this.RequestId);

        public bool ShowRequestPath => !string.IsNullOrEmpty(this.RequestPath);
    }
}
