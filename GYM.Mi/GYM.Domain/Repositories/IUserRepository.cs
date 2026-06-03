using GYM.Domain.Entities;
using GYM.Mi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.Domain.Repositories
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        
        (IList<User> data, int total, int totalDisplay) GetPagedusers(int pageIndex, 
            int pageSize, string? order, DataTablesSearch search);

        // Trainer User Relationship (Trainers Controller )
        (IList<User> data, int total, int totalDisplay) GetPagedUnassignedUsers(
            int pageIndex, int pageSize, string v, DataTablesSearch search);
        List<User> GetByTrainerId(Guid id);
    }
}
