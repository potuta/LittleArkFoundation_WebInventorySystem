using LittleArkFoundation_WebInventorySystem.Areas.Admin.Models;

namespace LittleArkFoundation_WebInventorySystem.Models
{
    public class HomeViewModel
    {
        public List<BloodInventoryModel>? BloodInventory { get; set; }
        public List<BloodRequestsModel>? RecentRequests { get; set; }
    }
}

