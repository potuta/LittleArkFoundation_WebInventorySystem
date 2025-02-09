using System.Data.Common;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using LittleArkFoundation_WebInventorySystem.Data;
using LittleArkFoundation_WebInventorySystem.Data.Repositories;
using Microsoft.Data.SqlClient;

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

            try
            {
                LoggingService.LogInformation($"Login attempt. UserID: {userID}, DateTime: {DateTime.Now}");
                bool result = await new UsersRepository(connectionString).VerifyUserExist(userID, password);

                if (result == false)
                {
                    LoggingService.LogInformation($"Invalid login attempt. UserID: {userID}, DateTime: {DateTime.Now}");
                    TempData["LoginError"] = "Invalid User ID or Password. Please try again.";
                    return RedirectToAction("Index", "Home");
                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                LoggingService.LogError("SQL Error: " + ex.Message);
                return RedirectToAction("Index", "Home");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                LoggingService.LogError("Error: " + ex.Message);
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
                LoggingService.LogInformation($"User logged in. UserID: {userID}, Role: {role}, DateTime: {DateTime.Now}"); 
                return RedirectToAction("SecurePage", role);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            var userIdClaim = User.FindFirst(ClaimTypes.Sid);
            if (userIdClaim != null)
            {
                LoggingService.LogInformation($"User logged out. UserID: {userIdClaim.Value}, DateTime: {DateTime.Now}");
            }
            else
            {
                LoggingService.LogWarning("User logged out but no UserID claim found.");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}