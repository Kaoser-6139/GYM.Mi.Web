using System.ComponentModel.DataAnnotations;

namespace GYM.Mi.Areas.Admin.Models
{
    public class UpdateUserModel
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public string? Gender { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "Please insert a valid date")]
        public DateTime EntryDate { get; set; }

        [Required]
        public double HeightCm { get; set; }
        [Required]
        public double WeightKg { get; set; }
        
        public double? BodyFatPercent { get; set; }
        
        public double? BMI { get; set; }


        
        public string? MedicalConditions { get; set; }
        
        public string? InjuryNotes { get; set; }
        [Required]
        public string? PrimaryGoal { get; set; }
        [Required]
        public string? WorkoutPreference { get; set; }
        [Required]
        public string? WorkoutTimePreference { get; set; }


        // Membership Plan
        public string? PlanName { get; set; }
        public decimal Amount { get; set; }
        public int DurationMonths { get; set; }
    }
}
