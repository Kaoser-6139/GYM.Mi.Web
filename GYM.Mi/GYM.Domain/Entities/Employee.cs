using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.Domain.Entities
{
    public  class Employee : IEntity<Guid>
    {
        public Guid Id { get; set; }
        // Basic Personal
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
       // public string Role { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public string Department { get; set; } = string.Empty;
        public string WorkShift { get; set; } = string.Empty;
        public decimal Salary { get; private set; }
        public bool IsActive { get; set; } = true;

        public string? EmergencyContact { get; private set; }
        public string? BloodGroup { get; private set; }
        public string? Religion { get; private set; }
        public string? Qualification { get; private set; }
        public string? Specialization { get; private set; }
        public int? ExperienceYears { get; private set; }
        public string? LicenseNumber { get; private set; }
        public bool? CPRCertified { get; private set; }
        public string? BankAccount { get; private set; }

        // Trainer Landing Page Settings
        public string? ImageUrl { get; set; }
        public bool ShowOnLandingPage { get; set; } = false;
        public int DisplayOrder { get; set; } = 0;
        public string? ShortBio { get; set; }
        public ICollection<User> Students { get; set; } = new List<User>();



    }
}
