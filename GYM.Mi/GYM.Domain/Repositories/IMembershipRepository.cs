using GYM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.Domain.Repositories
{
    public  interface IMembershipRepository:IRepository<Membership,Guid>
    {
        // User এর সব membership history
        IList<Membership> GetByUserId(Guid userId);

        // User এর current active membership
        Membership? GetActiveByUserId(Guid userId);

        // Admin pending payment list
        IList<Membership> GetPending();

        // Admin dashboard revenue
        decimal GetTotalRevenue();
    }
}
