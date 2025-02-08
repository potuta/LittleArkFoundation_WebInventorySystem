using LittleArkFoundation_WebInventorySystem.Data;
using LittleArkFoundation_WebInventorySystem.Data.Repositories;

namespace LittleArkFoundation_WebInventorySystem.Models
{
    public class UsersViewModel
    {
        public IEnumerable<UsersModel>? Users { get; set; }
        public UsersModel? NewUser { get; set; }
        public IEnumerable<RolesModel>? Roles { get; set; }
    }
}