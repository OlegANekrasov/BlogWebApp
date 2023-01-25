namespace BlogWebApp.BLL.ViewModels.Users
{
    /// <summary>
    /// User Photo data to pass to the view
    /// </summary>
    public class PhotoViewModel
    {
        public string UserId { get; set; }

        public string? PhotoPath { get; set; }

        public bool IsEditUserView { get; set; } = false;
    }
}
