using GYM.Domain.Entities;
using GYM.Mi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.Domain.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee, Guid>
    {
        (IList<Employee> data, object total, object totalDisplay) GetPagedEmployee(
            int pageIndex,
            int pageSize,
            string order,
            DataTablesSearch search,
            string? departmentFilter = null,
    bool? isActiveFilter = null
           );

        IList<Employee> GetPublicLandingTrainers(int count);

        IList<Employee> GetPublicTrainers();
    }
}
