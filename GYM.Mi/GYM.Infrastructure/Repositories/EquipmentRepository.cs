using GYM.Domain.Entities;
using GYM.Domain.Repositories;
using GYM.Mi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.Infrastructure.Repositories
{
    public class EquipmentRepository:Repository<Equipment,Guid>, IEquipmentRepository
    {
        public EquipmentRepository(ApplicationDbContext context):base(context)
        {

        }

        public (IList<Equipment> data, int total, int totalDisplay) GetPagedEquipments(int pageIndex, int pageSize, string? order, DataTablesSearch search)
        {
            if (string.IsNullOrWhiteSpace(search.Value))
            {
                return GetDynamic(null, order, null, pageIndex, pageSize, true);
            }
            else
            {
                return GetDynamic(
                    x => x.Name.Contains(search.Value) ||
                         x.CategoryName.Contains(search.Value) ||
                         x.SerialNumber.Contains(search.Value) ||
                         x.Barcode.Contains(search.Value) ||
                         x.AvailabilityStatus.Contains(search.Value) ||
                         x.Condition.Contains(search.Value) ||
                         x.Location.Contains(search.Value) ||
                         x.Description.Contains(search.Value),
                    order, null, pageIndex, pageSize, true);
            }

        }

    }
}
