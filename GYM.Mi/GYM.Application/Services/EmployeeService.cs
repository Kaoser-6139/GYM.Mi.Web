using GYM.Domain;
using GYM.Domain.Entities;
using GYM.Domain.Services;
using GYM.Mi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.Application.Services
{
    public  class EmployeeService:IEmployeeService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        public EmployeeService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public void AddEmployee(Employee employee)
        {
            _applicationUnitOfWork.EmployeeRepository.Add(employee);
            _applicationUnitOfWork.Save();
        }

        public void DeleteEmployee(Guid id)
        {
            _applicationUnitOfWork.EmployeeRepository.Remove(id);
            _applicationUnitOfWork.Save();
        }

        public Employee GetEmployee(Guid id)
        {
           return  _applicationUnitOfWork.EmployeeRepository.GetById(id);

        }

        public (IList<Employee> data, object total, object totalDisplay) GetEmployees(int pageIndex, int pageSize, string order, DataTablesSearch search, string? departmentFilter = null, bool? isActiveFilter = null)
        {
          return _applicationUnitOfWork.EmployeeRepository.GetPagedEmployee(pageIndex, pageSize, order, search, departmentFilter, isActiveFilter);
        }

        public int GetTotalEmployeeCount()
        {
            return _applicationUnitOfWork.EmployeeRepository.GetCount();
        }

        public void Update(Employee employee)
        {
            _applicationUnitOfWork.EmployeeRepository.Update(employee);
            _applicationUnitOfWork.Save();
        }
        public IList<Employee> GetPublicLandingTrainers(int count)
        {
            return _applicationUnitOfWork.EmployeeRepository.GetPublicLandingTrainers(count);
        }
        public IList<Employee> GetPublicTrainers()
        {
            return _applicationUnitOfWork.EmployeeRepository.GetPublicTrainers();
        }
    }
}
