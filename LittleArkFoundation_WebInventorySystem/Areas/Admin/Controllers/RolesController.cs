using LittleArkFoundation_WebInventorySystem.Areas.Admin.Models;
using LittleArkFoundation_WebInventorySystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LittleArkFoundation_WebInventorySystem.Areas.Admin.Controllers
{
    //TODO: Implement RolesModule
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly ConnectionService _connectionService;

        public RolesController(ConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        public async Task<IActionResult> Index(string dbType)
        {
            string connectionString = _connectionService.GetConnectionString(dbType);

            await using (var context = new ApplicationDbContext(connectionString))
            {
                var role = await context.Roles.ToListAsync();

                var viewModel = new RolesViewModel()
                {
                    Roles = role
                };

                return View(viewModel);
            }
        }
    }
}
