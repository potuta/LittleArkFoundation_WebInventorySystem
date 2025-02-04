namespace LittleArkFoundation_WebInventorySystem.Models
{
    public class HospitalRequestsModel
    {
        public int RequestID { get; set; }
        public string HospitalName { get; set; }
        public string BloodType { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public DateTime RequestDate { get; set; }
    }
}