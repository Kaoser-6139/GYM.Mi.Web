using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.Domain.Dtos
{
    public class UserSearchDto
    {
        public string? MembershipStatus { get; set; }
        public string? MembershipPlan { get; set; }

        public string? Gender { get; set; }
        public string? PrimaryGoal { get; set; }

        public int? AgeFrom { get; set; }
        public int? AgeTo { get; set; }

        public DateTime? EntryDateFrom { get; set; }
        public DateTime? EntryDateTo { get; set; }

        public DateTime? MembershipExpiryFrom { get; set; }
        public DateTime? MembershipExpiryTo { get; set; }

        public DateTime? PaymentRequestedFrom { get; set; }
        public DateTime? PaymentRequestedTo { get; set; }

        public string? TrainerStatus { get; set; }
    }
}
