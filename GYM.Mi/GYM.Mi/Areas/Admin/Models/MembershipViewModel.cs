using GYM.Domain.Entities;

namespace GYM.Mi.Areas.Admin.Models
{
    
        public class MembershipViewModel
        {
            public Membership? ActiveMembership { get; set; }

            public IList<Membership> MembershipHistory { get; set; } = new List<Membership>();

            public Membership? PendingMembership =>
                MembershipHistory
                    .FirstOrDefault(m => m.PaymentStatus == "Pending");

            public bool HasActiveMembership =>
                ActiveMembership != null && ActiveMembership.IsActive;

            public bool HasPendingMembership =>
                PendingMembership != null;

            public bool CanUseGemini =>
                HasActiveMembership &&
                ActiveMembership != null &&
                (ActiveMembership.PlanName == "Standard" || ActiveMembership.PlanName == "Premium");

            public bool CanUseSpecialSupport =>
                HasActiveMembership &&
                ActiveMembership != null &&
                ActiveMembership.PlanName == "Premium";

            public bool IsMembershipExpired =>
                !HasActiveMembership &&
                MembershipHistory.Any(m =>
                    m.PaymentStatus == "Active" &&
                    m.ExpiryDate.HasValue &&
                    m.ExpiryDate.Value < DateTime.UtcNow);

            public bool CanRenew =>
                !HasActiveMembership && !HasPendingMembership;
        }


}
