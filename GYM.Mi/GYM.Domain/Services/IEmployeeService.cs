using GYM.Domain.Entities;
using GYM.Mi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.Domain.Services
{
    public interface IEmployeeService
    {
        void AddEmployee(Employee employee);
        void DeleteEmployee(Guid id);
        
        Employee GetEmployee(Guid id);
        (IList<Employee> data, object total, object totalDisplay) GetEmployees(
            int pageIndex,
            int pageSize, 
            string v,
            DataTablesSearch search,
            string? departmentFilter = null,
           bool? isActiveFilter = null
            );

        int GetTotalEmployeeCount();
        void Update(Employee employee);

        IList<Employee> GetPublicLandingTrainers(int count);
        IList<Employee> GetPublicTrainers();
    }
}
