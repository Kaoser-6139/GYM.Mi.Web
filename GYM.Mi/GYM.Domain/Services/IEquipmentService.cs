using GYM.Domain.Entities;
using GYM.Mi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.Domain.Services
{
    public interface IEquipmentService
    {
        void AddEquipment(Equipment equipment);
        void DeleteEquipment(Guid id);
        Equipment GetEquipment(Guid id);
        (IList<Equipment> data, int total, int totalDisplay) GetEquipments(int pageIndex, int pageSize, string? v, DataTablesSearch search);
        int GetTotalEquipmentCount();
        void Update(Equipment equipment);
    }
}
