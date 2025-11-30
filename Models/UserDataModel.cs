namespace PoseDatabaseWebApi.Models
{
    public class UserDataModel
    {
        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
    }
}
