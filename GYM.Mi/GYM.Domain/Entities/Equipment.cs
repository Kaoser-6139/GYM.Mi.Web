using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.Domain.Entities
{
    public class Equipment:IEntity<Guid>
    {
        public Guid Id { get; set; }

        // Basic Info
        public string Name { get; set; } 
        public string CategoryName { get; set; }

        // Identification
        public string SerialNumber { get; set; } 
        public string Barcode { get; set; } 

        // Price
        public decimal Price { get; set; }

        // Status
        public string AvailabilityStatus { get; set; } = "Available";

        // Purchase & Warranty (Admin input)
        public DateTime? PurchaseDate { get; set; }
        public DateTime? WarrantyExpiryDate { get; set; }

        // Maintenance Info (Admin input)
        public DateTime? LastMaintenanceDate { get; set; }
        public int MaintenanceIntervalDays { get; set; } 
        public DateTime? NextMaintenanceDate
        {
            get
            {
                if (LastMaintenanceDate.HasValue)
                {
                    return LastMaintenanceDate.Value.AddDays(MaintenanceIntervalDays);
                }
                return null;
            }
        }

        // Additional Info
        public string Condition { get; set; } = "Good";
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
