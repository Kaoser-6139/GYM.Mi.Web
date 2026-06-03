namespace GYM.Mi.Areas.Admin.Models
{
    public class AddUserModel
    {
        public string? FullName { get; set; }
        public int Age { get; set; }
        public string? Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime EntryDate { get; set; }

     
        public double HeightCm { get; set; }
        public double WeightKg { get; set; }
        public double? BodyFatPercent { get; set; }
        public double? BMI { get; set; }

      
        public string? MedicalConditions { get; set; }
        public string? InjuryNotes { get; set; }
        public string? PrimaryGoal { get; set; } 
        public string? WorkoutPreference { get; set; } 
        public string? WorkoutTimePreference { get; set; } 
    }
}
