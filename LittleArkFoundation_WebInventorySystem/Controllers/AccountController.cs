using System.Data.Common;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using LittleArkFoundation_WebInventorySystem.Data;
using LittleArkFoundation_WebInventorySystem.Data.Repositories;

namespace LittleArkFoundation_WebInventorySystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ConnectionService _connectionService;

        public AccountController(ConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        [HttpPost]
        public async Task<IActionResult> Index(int userID, string password)
        {
            string connectionString = _connectionService.GetConnectionString("main");

            bool result = await new UsersRepository(connectionString).VerifyUserExist(userID, password);

            if (result == false)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await new UsersRepository(connectionString).GetUserAsync(userID);

            if (user != null)
            {
                string role = await new RolesRepository(connectionString).GetRoleNameByRoleID(user.RoleID);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Sid, user.UserID.ToString()),
                    new Claim(ClaimTypes.Role, role)
                };

                var identity = new ClaimsIdentity(claims, "CookieAuth");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("CookieAuth", principal);
                return RedirectToAction("SecurePage", role);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Index", "Home");
        }
    }
}