namespace LittleArkFoundation_WebInventorySystem.Models
{
    public class BloodInventoryModel
    {
        public int BloodID { get; set; }
        public string BloodType { get; set; }
        public int BloodQuantity { get; set; }
        public DateTime ExpiryDate { get; set; }
    }   
}