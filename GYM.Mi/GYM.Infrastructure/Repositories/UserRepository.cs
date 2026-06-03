using GYM.Domain.Entities;
using GYM.Domain.Repositories;
using GYM.Mi.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.Infrastructure.Repositories
{
    public class UserRepository : Repository<User, Guid>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        

        public (IList<User> data, int total, int totalDisplay) GetPagedusers(int pageIndex,
            int pageSize, string order, DataTablesSearch search)
        {
            if (string.IsNullOrWhiteSpace(search.Value))
                return GetDynamic(null, order, null, pageIndex, pageSize, true);
            else
                return GetDynamic(
          x => x.FullName.Contains(search.Value) ||
          x.Gender.Contains(search.Value) ||
          x.PhoneNumber.Contains(search.Value) ||
          x.PrimaryGoal.Contains(search.Value) ||
          x.WorkoutPreference.Contains(search.Value),
         order, null, pageIndex, pageSize, true);

        }

        // Trainer User Relationship (Trainers Controller )
        public (IList<User> data, int total, int totalDisplay) GetPagedUnassignedUsers(
            int pageIndex, int pageSize, string order, DataTablesSearch search)
        {
            if (string.IsNullOrWhiteSpace(search.Value))
            {
                return GetDynamic(x => x.TrainerEmployeeId == null, order, null, pageIndex, pageSize, true);
            }

            return GetDynamic(
                x => x.TrainerEmployeeId == null &&
                    (
                        (x.FullName ?? "").Contains(search.Value) ||
                        (x.Gender ?? "").Contains(search.Value) ||
                        (x.PhoneNumber ?? "").Contains(search.Value) ||
                        (x.PrimaryGoal ?? "").Contains(search.Value) ||
                        (x.WorkoutPreference ?? "").Contains(search.Value)
                    ),
                order, null, pageIndex, pageSize, true);
        }

        public List<User> GetByTrainerId(Guid id)
        {
            return GetDynamic(
                x => x.TrainerEmployeeId == id,
                orderBy: "FullName asc",
                include: null,
                pageIndex: 1,
                pageSize: int.MaxValue,
                isTrackingOff: true
            ).data.ToList();
        }
    }
}
