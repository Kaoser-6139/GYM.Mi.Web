using GYM.Domain.Entities;
using GYM.Domain.Services;
using GYM.Domain;
using GYM.Mi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.Application.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public MembershipService(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void CreateMembership(Membership membership)
        {
            membership.Id = IdentityGenerator.NewSequentialGuid();
            membership.PaymentStatus = "Pending";
            membership.CreatedAt = DateTime.UtcNow;

            _unitOfWork.MembershipRepository.Add(membership);
            _unitOfWork.Save();
        }

        public void ApproveMembership(Guid membershipId, DateTime startDate, string approvedBy)
        {
            var membership = _unitOfWork.MembershipRepository.GetById(membershipId);

            if (membership == null)
            {
                return;
            }

            membership.PaymentStatus = "Active";
            membership.StartDate = startDate;
            membership.ExpiryDate = startDate.AddMonths(membership.DurationMonths);
            membership.ApprovedBy = approvedBy;
            membership.ApprovedAt = DateTime.UtcNow;

            _unitOfWork.MembershipRepository.Update(membership);
            _unitOfWork.Save();
        }

        public Membership? GetActiveMembership(Guid userId)
        {
            return _unitOfWork.MembershipRepository.GetActiveByUserId(userId);
        }

        public IList<Membership> GetMembershipHistory(Guid userId)
        {
            return _unitOfWork.MembershipRepository.GetByUserId(userId);
        }

        public IList<Membership> GetPendingMemberships()
        {
            return _unitOfWork.MembershipRepository.GetPending();
        }

        public decimal GetTotalRevenue()
        {
            return _unitOfWork.MembershipRepository.GetTotalRevenue();
        }
    }
}
