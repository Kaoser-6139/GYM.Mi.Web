namespace GYM.Mi.Areas.Admin.Models
{
    public class UpdateEquipmentModel
    {
        public Guid Id { get; set; }

        
        public string Name { get; set; }
        public string CategoryName { get; set; }

        
        public string SerialNumber { get; set; }
        public string Barcode { get; set; }

        
        public decimal Price { get; set; }

       
        public string AvailabilityStatus { get; set; } = "Available";

       
        public DateTime? PurchaseDate { get; set; }
        public DateTime? WarrantyExpiryDate { get; set; }

        
        public DateTime? LastMaintenanceDate { get; set; }
        public int MaintenanceIntervalDays { get; set; }
        public DateTime? NextMaintenanceDate {  get; set; } 
       

      
        public string Condition { get; set; } = "Good";
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
