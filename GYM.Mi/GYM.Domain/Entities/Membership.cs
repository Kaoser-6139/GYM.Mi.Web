using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.Domain.Entities
{
    public class Membership : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public string PlanName { get; set; } = string.Empty;
        // Basic, Standard, Premium

        public int DurationMonths { get; set; }
        // Basic = 1, Standard = 3, Premium = 6

        public decimal Amount { get; set; }
        // Basic = 1500, Standard = 4000, Premium = 7500

        public string PaymentStatus { get; set; } = "Pending";
        // Pending = user selected plan but admin has not approved payment
        // Active = admin approved payment
        // Expired = membership date finished

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? StartDate { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string? ApprovedBy { get; set; }

        public DateTime? ApprovedAt { get; set; }

        // This will not be stored in database.
        // It will only calculate active status in code.
        public bool IsActive =>
            PaymentStatus == "Active" &&
            ExpiryDate.HasValue &&
            DateTime.UtcNow <= ExpiryDate.Value;

        public int DaysRemaining =>
            IsActive ? (int)(ExpiryDate!.Value - DateTime.UtcNow).TotalDays : 0;

        public int ProgressPercent
        {
            get
            {
                if (!IsActive || StartDate == null || ExpiryDate == null)
                    return 0;

                var total = (ExpiryDate.Value - StartDate.Value).TotalDays;
                var used = (DateTime.UtcNow - StartDate.Value).TotalDays;

                if (total <= 0)
                    return 0;

                return (int)Math.Min(100, (used / total) * 100);
            }
        }
    }

}
