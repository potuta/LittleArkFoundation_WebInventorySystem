using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LittleArkFoundation_WebInventorySystem.Data;

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
            return View();
        }
    }
}