using System.Diagnostics;
using LittleArkFoundation_WebInventorySystem.Data;
using LittleArkFoundation_WebInventorySystem.Models;
using Microsoft.AspNetCore.Mvc;
using LittleArkFoundation_WebInventorySystem.Data;
using Microsoft.EntityFrameworkCore;

namespace LittleArkFoundation_WebInventorySystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ConnectionService _connectionService;

        public HomeController(ConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        public async Task<IActionResult> Index()
        {
            string connectionString = _connectionService.GetConnectionString("main");

            using (var _dbContext = new ApplicationDbContext(connectionString))
            {
                var bloodInventory = await _dbContext.BloodInventory.ToListAsync();
                var recentRequests = await _dbContext.HospitalRequests.OrderByDescending(r => r.RequestDate).Take(5).ToListAsync();

                var viewModel = new HomeViewModel
                {
                    BloodInventory = bloodInventory,
                    RecentRequests = recentRequests
                };

                return View(viewModel);
            }
        }
    }
}
