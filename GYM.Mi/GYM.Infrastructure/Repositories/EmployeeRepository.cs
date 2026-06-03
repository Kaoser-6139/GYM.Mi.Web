using GYM.Domain.Entities;
using GYM.Domain.Repositories;
using GYM.Mi.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GYM.Infrastructure.Repositories
{
    public class EmployeeRepository : Repository<Employee, Guid>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public (IList<Employee> data, object total, object totalDisplay) GetPagedEmployee(
                  int pageIndex,
                  int pageSize,
                  string order,
                  DataTablesSearch search,
                  string? departmentFilter = null,
                  bool? isActiveFilter = null)
        {
            var term = search.Value?.Trim();

            
            Expression<Func<Employee, bool>> filter = x =>
                // department filter
                (string.IsNullOrWhiteSpace(departmentFilter) || x.Department == departmentFilter)

                // active filter
                && (!isActiveFilter.HasValue || x.IsActive == isActiveFilter.Value)

                // search filter
                && (string.IsNullOrWhiteSpace(term)
                    || (x.FirstName ?? "").Contains(term)
                    || (x.LastName ?? "").Contains(term)
                    || (x.Gender ?? "").Contains(term)
                    || (x.PhoneNumber ?? "").Contains(term)
                    || (x.Email ?? "").Contains(term)
                    || (x.Address ?? "").Contains(term)
                    || (x.Department ?? "").Contains(term)
                    || (x.WorkShift ?? "").Contains(term)

                    // nullable strings safe
                    || (x.EmergencyContact ?? "").Contains(term)
                    || (x.BloodGroup ?? "").Contains(term)
                    || (x.Religion ?? "").Contains(term)
                    || (x.Qualification ?? "").Contains(term)
                    || (x.Specialization ?? "").Contains(term)
                    || (x.LicenseNumber ?? "").Contains(term)
                    || (x.BankAccount ?? "").Contains(term)

                    // nullable values
                    || (x.ExperienceYears.HasValue && x.ExperienceYears.Value.ToString().Contains(term))
                    || (x.CPRCertified.HasValue && x.CPRCertified.Value.ToString().Contains(term))

                    || x.Salary.ToString().Contains(term)
                    || x.IsActive.ToString().Contains(term)
                );

            
            return GetDynamic(filter, order, null, pageIndex, pageSize, true);
        }

        public IList<Employee> GetPublicLandingTrainers(int count)
        {
            var result = Get(
                x => x.Department == "Trainer"
                     && x.IsActive
                     && x.ShowOnLandingPage,
                x => x.OrderBy(e => e.DisplayOrder)
                      .ThenBy(e => e.FirstName),
                null,
                1,
                count,
                true
            );

            return result.data;
        }
        public IList<Employee> GetPublicTrainers()
        {
            return Get(
                x => x.Department == "Trainer"
                     && x.IsActive
                     && x.ShowOnLandingPage,
                x => x.OrderBy(e => e.DisplayOrder)
                      .ThenBy(e => e.FirstName),
                null,
                true
            );
        }

    }
}
