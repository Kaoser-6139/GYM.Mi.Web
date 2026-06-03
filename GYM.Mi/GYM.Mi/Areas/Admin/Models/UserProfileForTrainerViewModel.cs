using GYM.Domain.Entities;

namespace GYM.Mi.Areas.Admin.Models
{
    public class UserProfileForTrainerViewModel
    {
        public User User { get; set; } = null!;

        public string TrainerName { get; set; } = "No Trainer Assigned";

        public string TrainerPhone { get; set; } = "N/A";

        public MembershipViewModel Membership { get; set; } = new MembershipViewModel();
    }
}

