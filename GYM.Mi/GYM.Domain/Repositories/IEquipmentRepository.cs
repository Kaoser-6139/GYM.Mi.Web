using GYM.Domain.Entities;
using GYM.Mi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.Domain.Repositories
{
    public interface IEquipmentRepository : IRepository<Equipment, Guid>
    {
        (IList<Equipment> data, int total, int totalDisplay) GetPagedEquipments(int pageIndex, int pageSize, string? order, DataTablesSearch search);
    }
}
