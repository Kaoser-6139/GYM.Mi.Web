using GYM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.Domain.Services
{
    public  interface IMembershipService
    {
        // Welcome form submit হলে pending membership create হবে
        void CreateMembership(Membership membership);

        // Admin cash payment confirm করলে membership active হবে
        void ApproveMembership(Guid membershipId, DateTime startDate, string approvedBy);

        // User profile/dashboard এর জন্য active membership
        Membership? GetActiveMembership(Guid userId);

        // User payment history এর জন্য
        IList<Membership> GetMembershipHistory(Guid userId);

        // Admin pending payment list এর জন্য
        IList<Membership> GetPendingMemberships();

        // Admin dashboard revenue এর জন্য
        decimal GetTotalRevenue();
    }
}
