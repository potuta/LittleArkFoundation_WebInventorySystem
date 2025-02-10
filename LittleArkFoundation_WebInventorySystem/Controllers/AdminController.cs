using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LittleArkFoundation_WebInventorySystem.Data;
using System.Security.Claims;
using LittleArkFoundation_WebInventorySystem.Models;

namespace LittleArkFoundation_WebInventorySystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly ConnectionService _connectionService;

        public AdminController(ConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult SecurePage()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            string connectionString = _connectionService.GetConnectionString("main");

            using (var context = new ApplicationDbContext(connectionString))
            {
                var user = context.Users.Find(Convert.ToInt32(userIdClaim.Value));
                if (user == null) return NotFound();
                ViewBag.WelcomeMessage = $"Welcome {user.Username}!";
            };
            return View();
        }
    }
}