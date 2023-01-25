namespace BlogWebApp.BLL.ViewModels
{
    /// <summary>
    /// Passing Data to the Error View
    /// </summary>
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}