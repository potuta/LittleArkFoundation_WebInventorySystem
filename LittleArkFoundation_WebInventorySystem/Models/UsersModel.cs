namespace LittleArkFoundation_WebInventorySystem.Models
{
    public class UsersModel
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleID { get; set; }
        public DateTime DateCreated { get; set; }
    }
}