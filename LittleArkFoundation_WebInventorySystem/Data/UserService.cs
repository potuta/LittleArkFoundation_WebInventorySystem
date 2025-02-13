using Microsoft.AspNetCore.Identity;
using LittleArkFoundation_WebInventorySystem.Areas.Admin.Models;

namespace LittleArkFoundation_WebInventorySystem.Data
{
    public class UserService
    {
        private readonly UserManager<UsersModel> _userManager;

        public UserService() { }
        
        public UserService(UserManager<UsersModel> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> isUserExistAsync(int userId, string password)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return false;
            }

            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<UsersModel?> GetUserAsync(int userId)
        {
            return await _userManager.FindByIdAsync(userId.ToString());
        }

    }
}
