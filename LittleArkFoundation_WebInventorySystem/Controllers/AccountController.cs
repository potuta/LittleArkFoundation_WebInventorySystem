using System.Data.Common;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace LittleArkFoundation_WebInventorySystem.Controllers
{
    public class AccountController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Index(int userID, string password)
        {
            if (userID == 10001 && password == "admin")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Sid, userID.ToString()),
                    new Claim(ClaimTypes.Role, "Admin")
                };
                
                var identity = new ClaimsIdentity(claims, "CookieAuth");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("CookieAuth", principal);

                Console.WriteLine($"Admin login successful, {userID}, {password}");
                return RedirectToAction("SecurePage", "Admin");
            }
            else if (userID == 20001 && password == "donor")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Sid, userID.ToString()),
                    new Claim(ClaimTypes.Role, "Donor")
                };

                var identity = new ClaimsIdentity(claims, "CookieAuth");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("CookieAuth", principal);

                Console.WriteLine($"Donor login successful, {userID}, {password}");
                return RedirectToAction("SecurePage", "Donor");
            }
            else if (userID == 30001 && password == "hospital")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Sid, userID.ToString()),
                    new Claim(ClaimTypes.Role, "Hospital")
                };

                var identity = new ClaimsIdentity(claims, "CookieAuth");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("CookieAuth", principal);

                Console.WriteLine($"Hospital login successful, {userID}, {password}");
                return RedirectToAction("SecurePage", "Hospital");
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