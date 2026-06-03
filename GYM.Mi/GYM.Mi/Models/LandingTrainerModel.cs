namespace GYM.Mi.Models
{
    public class LandingTrainerModel
    {
        public string FullName { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public string? Specialization { get; set; }

        public int? ExperienceYears { get; set; }

        public string? ShortBio { get; set; }
    }
}
