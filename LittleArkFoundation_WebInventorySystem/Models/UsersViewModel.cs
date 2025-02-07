using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LittleArkFoundation_WebInventorySystem.Models
{
    public class UsersViewModel
    {
        public IEnumerable<UsersModel>? Users { get; set; }

        //[BindRequired]
        public UsersModel NewUser { get; set; }
    }
}