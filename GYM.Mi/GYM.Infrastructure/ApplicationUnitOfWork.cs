using Demo.Infrastructure.Utilities;
using GYM.Domain;
using GYM.Domain.Dtos;
using GYM.Domain.Repositories;
using GYM.Mi.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.Infrastructure
{
    public class ApplicationUnitOfWork : UnitOfWork, IApplicationUnitOfWork
    {
        public ApplicationUnitOfWork(ApplicationDbContext dbContext,
            IUserRepository userRepository,
            IEquipmentRepository equipmentRepository,
            IEmployeeRepository employeeRepository,
        IMembershipRepository membershipRepository,
        IBlogRepository blogRepository): base(dbContext)
        {
            UserRepository =userRepository;
            EquipmentRepository =equipmentRepository;
            EmployeeRepository =employeeRepository;
            MembershipRepository = membershipRepository;
            BlogRepository = blogRepository;



        }

        public IUserRepository UserRepository { get; private set; }
        public IEquipmentRepository EquipmentRepository { get;private set; }
        public IEmployeeRepository EmployeeRepository { get; private set;}

        public IMembershipRepository MembershipRepository { get; private set; }
        public IBlogRepository BlogRepository { get; private set; }



        //Advaced Search

        public async Task<(IList<UserListDto> data, int total, int totalDisplay)> GetUsersSP(
    int pageIndex,
    int pageSize,
    string? order,
    DataTablesSearch search,
    UserSearchDto searchItem)
        {
            var procedureName = "GetUsersAdvanced";

            var keyword = string.IsNullOrWhiteSpace(search.Value)
                                ? null
                                 : search.Value;

            var result = await SqlUtility.QueryWithStoredProcedureAsync<UserListDto>(
                procedureName,
                new Dictionary<string, object?>
                {
            { "PageIndex", pageIndex },
            { "PageSize", pageSize },
            { "OrderBy", order },

            { "Keyword", keyword },

            { "MembershipStatus", string.IsNullOrWhiteSpace(searchItem.MembershipStatus) ? null : searchItem.MembershipStatus },
            { "MembershipPlan", string.IsNullOrWhiteSpace(searchItem.MembershipPlan) ? null : searchItem.MembershipPlan },

            { "Gender", string.IsNullOrWhiteSpace(searchItem.Gender) ? null : searchItem.Gender },
            { "PrimaryGoal", string.IsNullOrWhiteSpace(searchItem.PrimaryGoal) ? null : searchItem.PrimaryGoal },

            { "AgeFrom", searchItem.AgeFrom },
            { "AgeTo", searchItem.AgeTo },

            { "EntryDateFrom", searchItem.EntryDateFrom },
            { "EntryDateTo", searchItem.EntryDateTo },

            { "MembershipExpiryFrom", searchItem.MembershipExpiryFrom },
            { "MembershipExpiryTo", searchItem.MembershipExpiryTo },

            { "PaymentRequestedFrom", searchItem.PaymentRequestedFrom },
            { "PaymentRequestedTo", searchItem.PaymentRequestedTo },

            { "TrainerStatus", string.IsNullOrWhiteSpace(searchItem.TrainerStatus) ? null : searchItem.TrainerStatus }
                },
                new Dictionary<string, Type>
                {
            { "Total", typeof(int) },
            { "TotalDisplay", typeof(int) }
                });

            return (
                result.result,
                (int)result.outValues["Total"],
                (int)result.outValues["TotalDisplay"]
            );
        }
    }
}
