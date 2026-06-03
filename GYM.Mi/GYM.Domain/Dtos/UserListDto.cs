using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.Domain.Dtos
{
    public class UserListDto
    {
        public Guid Id { get; set; }

        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }

        public int Age { get; set; }
        public string? Gender { get; set; }

        public string? PrimaryGoal { get; set; }

        public DateTime EntryDate { get; set; }

        public string? MembershipPlan { get; set; }
        public string? MembershipStatus { get; set; }
        public string? MembershipText { get; set; }

        public string? TrainerName { get; set; }
        public string? TrainerStatus { get; set; }
    }
}
