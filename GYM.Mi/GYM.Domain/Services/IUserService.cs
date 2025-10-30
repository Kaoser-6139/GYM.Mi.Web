using GYM.Domain.Entities;
using GYM.Mi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.Domain.Services
{
    public interface IUserService
    {
        void AddUser(User user);
        void DeleteUser(Guid id);
        User GetUser(Guid id);
        (IList<User> data, int total, int totalDisplay) GetUsers(int pageIndex, int pageSize, string? v, DataTablesSearch search);
        void Update(User author);
    }
}
