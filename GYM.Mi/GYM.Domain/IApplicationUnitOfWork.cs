using GYM.Domain.Dtos;
using GYM.Domain.Repositories;
using GYM.Mi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.Domain
{
    public interface IApplicationUnitOfWork:IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
        public IEquipmentRepository EquipmentRepository { get; }
        public IEmployeeRepository EmployeeRepository { get; }
        public IMembershipRepository MembershipRepository { get; }
        public IBlogRepository BlogRepository { get; }

        Task<(IList<UserListDto> data, int total, int totalDisplay)> GetUsersSP(int pageIndex, int pageSize, string? order, DataTablesSearch search, UserSearchDto searchItem);
    }
}
