using System.ComponentModel.DataAnnotations;

namespace GYM.Mi.Areas.Admin.Models
{
    public class AddEquipmentModel
    {
        [Required(ErrorMessage = "Equipment name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Serial number is required.")]
        public string SerialNumber { get; set; }

        [Required(ErrorMessage = "Barcode is required.")]
        public string Barcode { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        // Optional Fields
        public string AvailabilityStatus { get; set; } = "Available";

        [Required(ErrorMessage = "Purchase date is required.")]
        public DateTime? PurchaseDate { get; set; }

        [Required(ErrorMessage = "Warranty expiry date is required.")]
        public DateTime? WarrantyExpiryDate { get; set; }

       // [Required(ErrorMessage = "Last maintenance date is required.")]
        public DateTime? LastMaintenanceDate { get; set; }

        //[Required(ErrorMessage = "Maintenance interval (in days) is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Maintenance interval must be greater than zero.")]
        public int MaintenanceIntervalDays { get; set; }

        public DateTime? NextMaintenanceDate { get; set; }

        [Required(ErrorMessage = "Condition is required.")]
        public string Condition { get; set; } = "Good";

        [Required(ErrorMessage = "Location is required.")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;
    }
}

