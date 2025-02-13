using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LittleArkFoundation_WebInventorySystem.Areas.Admin.Controllers
{
    //TODO: Implement RolesModule
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
