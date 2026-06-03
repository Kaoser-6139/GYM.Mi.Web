using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace GYM.Mi.Areas.Admin.Models
{
    public class UpdateEmployeeModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;      
        public DateTime DateOfBirth { get; set; }        
        public string Gender { get; set; } = string.Empty;

     
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;  
        public DateTime HireDate { get; set; }

        
        public string Department { get; set; } = string.Empty;       
        public string WorkShift { get; set; } = string.Empty;
        public decimal Salary { get; set; }       
        public bool IsActive { get; set; } = true;
        
        public string? EmergencyContact { get; set; }
        public string? BloodGroup { get; set; }
        public string? Religion { get; set; }

        
        public string? Qualification { get; set; }     
        public string? Specialization { get; set; }       
        public int? ExperienceYears { get; set; }      
        public string? LicenseNumber { get; set; }
        public bool? CPRCertified { get; set; }

        public string? BankAccount { get; set; }

        // Trainer Landing Page Settings
        public string? ImageUrl { get; set; }

        public IFormFile? ImageFile { get; set; }

        public bool ShowOnLandingPage { get; set; } = false;

        [Range(0, int.MaxValue, ErrorMessage = "Display Order must be a positive number.")]
        public int DisplayOrder { get; set; } = 0;

        [StringLength(500, ErrorMessage = "Short Bio can't exceed 500 characters.")]
        public string? ShortBio { get; set; }
    }
}
