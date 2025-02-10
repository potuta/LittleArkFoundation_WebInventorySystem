using System.Data.Common;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using LittleArkFoundation_WebInventorySystem.Data;
using LittleArkFoundation_WebInventorySystem.Data.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace LittleArkFoundation_WebInventorySystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ConnectionService _connectionService;

        public AccountController(ConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(int userID, string password)
        //{
        //    string connectionString = _connectionService.GetConnectionString("main");

        //    try
        //    {
        //        LoggingService.LogInformation($"Login attempt. UserID: {userID}, DateTime: {DateTime.Now}");
        //        bool result = await new UsersRepository(connectionString).VerifyUserExist(userID, password);

        //        if (result == false)
        //        {
        //            LoggingService.LogInformation($"Invalid login attempt. UserID: {userID}, DateTime: {DateTime.Now}");
        //            TempData["LoginError"] = "Invalid User ID or Password. Please try again.";
        //            return RedirectToAction("Index", "Home");
        //        }
        //    }
        //    catch(SqlException ex)
        //    {
        //        Console.WriteLine($"SQL Error: {ex.Message}");
        //        LoggingService.LogError("SQL Error: " + ex.Message);
        //        return RedirectToAction("Index", "Home");
        //    }
        //    catch(Exception ex)
        //    {
        //        Console.WriteLine($"Error: {ex.Message}");
        //        LoggingService.LogError("Error: " + ex.Message);
        //        return RedirectToAction("Index", "Home");
        //    }

        //    var user = await new UsersRepository(connectionString).GetUserAsync(userID);

        //    if (user != null)
        //    {
        //        string role = await new RolesRepository(connectionString).GetRoleNameByRoleID(user.RoleID);

        //        var claims = new List<Claim>
        //        {
        //            new Claim(ClaimTypes.NameIdentifier, user.UserID),
        //            new Claim(ClaimTypes.Role, role)
        //        };

        //        var identity = new ClaimsIdentity(claims, "CookieAuth");
        //        var principal = new ClaimsPrincipal(identity);

        //        //await HttpContext.SignInAsync("CookieAuth", principal);
        //        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        //        LoggingService.LogInformation($"User logged in. UserID: {userID}, Role: {role}, DateTime: {DateTime.Now}"); 
        //        return RedirectToAction("SecurePage", role);
        //    }

        //    return RedirectToAction("Index", "Home");
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(int userID, string password)
        {
            string connectionString = _connectionService.GetConnectionString("main");

            try
            {
                LoggingService.LogInformation($"Login attempt. UserID: {userID}, DateTime: {DateTime.Now}");

                var user = await new UsersRepository(connectionString).GetUserAsync(userID, password);

                if (user == null)
                {
                    LoggingService.LogInformation($"Invalid login attempt. UserID: {userID}, DateTime: {DateTime.Now}");
                    TempData["LoginError"] = "Invalid User ID or Password. Please try again.";
                    return RedirectToAction("Index", "Home");
                }

                string role = await new RolesRepository(connectionString).GetRoleNameByRoleID(user.RoleID);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                    new Claim(ClaimTypes.Role, role)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                LoggingService.LogInformation($"User logged in. UserID: {userID}, Role: {role}, DateTime: {DateTime.Now}");

                // Redirect based on role
                return role switch
                {
                    "Admin" => RedirectToAction("SecurePage", "Admin"),
                    "Donor" => RedirectToAction("SecurePage", "Donor"),
                    _ => RedirectToAction("Index", "Home")
                };
            }
            catch (SqlException ex)
            {
                LoggingService.LogError("SQL Error: " + ex.Message);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                LoggingService.LogError("Error: " + ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> Logout()
        {
            //await HttpContext.SignOutAsync("CookieAuth");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
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

        // TODO: Implement VerifyCode and ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyCode()
        {
            return RedirectToAction("ResetPassword");
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string oldPassword, string newPassword)
        {
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            LoggingService.LogWarning($"Access denied. UserID: {User.FindFirst(ClaimTypes.NameIdentifier)?.Value}, DateTime: {DateTime.Now}");
            return View();
        }
    }
}