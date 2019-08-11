namespace CookWithMe.Web.ViewModels
{
    public class ErrorViewModel
    {
        public string StatusCode { get; set; }

        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(this.RequestId);
    }
}
