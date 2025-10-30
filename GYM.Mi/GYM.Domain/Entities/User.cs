using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.Domain.Entities
{
    public class User : IEntity<Guid>
    {
        public Guid Id { get; set; } 

        // Basic Information
        public string? FullName { get; set; }
        public int Age { get; set; }
        public string? Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime EntryDate { get; set; }

        // Physical Attributes
        public double HeightCm { get; set; }
        public double WeightKg { get; set; }
        public double? BodyFatPercent { get; set; }
        public double? BMI { get; set; }

        // Optional AI (Gemini) Profile Data
        public string? MedicalConditions { get; set; }
        public string? InjuryNotes { get; set; }
        public string? PrimaryGoal { get; set; } // e.g., FatLoss, MuscleGain, etc.
        public string? WorkoutPreference { get; set; } // e.g., Home, Gym, Outdoor
        public string? WorkoutTimePreference { get; set; } // e.g., Morning, Evening

    }
}
