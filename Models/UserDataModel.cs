namespace PoseDatabaseWebApi.Models
{
    public class UserDataModel
    {
        public int? UserDataId { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public bool IsDeleted { get; set; }


    }
}
