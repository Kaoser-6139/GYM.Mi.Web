using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace GYM.Mi.Areas.Admin.Models
{
    public class AddEmployeeModel
    {
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(100, ErrorMessage = "First Name can't exceed 100 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(100, ErrorMessage = "Last Name can't exceed 100 characters.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of Birth is required.")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [StringLength(20, ErrorMessage = "Gender can't exceed 20 characters.")]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone Number is required.")]
        [Phone(ErrorMessage = "Invalid Phone Number format.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email format.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(500, ErrorMessage = "Address can't exceed 500 characters.")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hire Date is required.")]
        public DateTime HireDate { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        [StringLength(100, ErrorMessage = "Department can't exceed 100 characters.")]
        public string Department { get; set; } = string.Empty;

        [Required(ErrorMessage = "Work Shift is required.")]
        [StringLength(50, ErrorMessage = "Work Shift can't exceed 50 characters.")]
        public string WorkShift { get; set; } = string.Empty;

        [Required(ErrorMessage = "Salary is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive number.")]
        public decimal Salary { get;  set; }

        [Required(ErrorMessage = "IsActive field is required.")]
        public bool IsActive { get; set; } = true;

        [StringLength(50, ErrorMessage = "Emergency Contact can't exceed 50 characters.")]
        public string? EmergencyContact { get; set; }

        [StringLength(20, ErrorMessage = "Blood Group can't exceed 20 characters.")]
        public string? BloodGroup { get;  set; }

        [StringLength(50, ErrorMessage = "Religion can't exceed 50 characters.")]
        public string? Religion { get;  set; }

        [StringLength(200, ErrorMessage = "Qualification can't exceed 200 characters.")]
        public string? Qualification { get;  set; }

        [StringLength(100, ErrorMessage = "Specialization can't exceed 100 characters.")]
        public string? Specialization { get;  set; }

        [Range(0, 100, ErrorMessage = "Experience years must be between 0 and 100.")]
        public int? ExperienceYears { get;  set; }

        [StringLength(50, ErrorMessage = "License Number can't exceed 50 characters.")]
        public string? LicenseNumber { get;  set; }

        public bool? CPRCertified { get;  set; }

        [StringLength(50, ErrorMessage = "Bank Account can't exceed 50 characters.")]
        [RegularExpression(@"^\d{10,20}$", ErrorMessage = "Invalid Bank Account format.")]
        public string? BankAccount { get; set; }

        // Trainer Landing Page Settings
        public IFormFile? ImageFile { get; set; }

        public bool ShowOnLandingPage { get; set; } = false;

        [Range(0, int.MaxValue, ErrorMessage = "Display Order must be a positive number.")]
        public int DisplayOrder { get; set; } = 0;

        [StringLength(500, ErrorMessage = "Short Bio can't exceed 500 characters.")]
        public string? ShortBio { get; set; }
    }
}
