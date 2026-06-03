using GYM.Domain.Entities;
using GYM.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.Infrastructure.Repositories
{
    public class MembershipRepository : Repository<Membership, Guid>, IMembershipRepository
    {
        public MembershipRepository(ApplicationDbContext context) : base(context)
        {
        }
        public IList<Membership> GetByUserId(Guid userId)
        {
            return Get(
                m => m.UserId == userId,
                q => q.OrderByDescending(m => m.CreatedAt),
                null,
                true
            );
        }

        public Membership? GetActiveByUserId(Guid userId)
        {
            return Get(
                m =>
                    m.UserId == userId &&
                    m.PaymentStatus == "Active" &&
                    m.ExpiryDate != null &&
                    m.ExpiryDate >= DateTime.UtcNow,
                q => q.OrderByDescending(m => m.StartDate),
                null,
                true
            ).FirstOrDefault();
        }

        public IList<Membership> GetPending()
        {
            return Get(
                m => m.PaymentStatus == "Pending",
                q => q.OrderByDescending(m => m.CreatedAt),
                q => q.Include(m => m.User),
                true
            );
        }

        public decimal GetTotalRevenue()
        {
            return Get(
                m => m.PaymentStatus == "Active" || m.PaymentStatus == "Expired",
                null,
                null,
                true
            ).Sum(m => m.Amount);
        }
    }
}
