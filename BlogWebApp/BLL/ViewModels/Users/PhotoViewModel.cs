namespace BlogWebApp.BLL.ViewModels.Users
{
    public class PhotoViewModel
    {
        public string UserId { get; set; }

        public string? PhotoPath { get; set; }

        public bool IsEditUserView { get; set; } = false;
    }
}
