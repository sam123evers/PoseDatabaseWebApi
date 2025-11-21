namespace PoseDatabaseWebApi.Data.Dto
{
    public class UpdateUserDto
    {
        public int UserDataId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
    }
}
