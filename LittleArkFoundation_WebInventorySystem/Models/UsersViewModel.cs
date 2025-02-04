namespace LittleArkFoundation_WebInventorySystem.Models
{
    public class UsersViewModel
    {
        public IEnumerable<UsersModel> Users { get; set; }
        public UsersModel NewUser { get; set; }
    }
}