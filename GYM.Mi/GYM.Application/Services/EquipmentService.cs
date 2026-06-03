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
    public  class EquipmentService:IEquipmentService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public EquipmentService(IApplicationUnitOfWork applicationUnitOfWork) 
        { 
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public void AddEquipment(Equipment equipment)
        {
            _applicationUnitOfWork.EquipmentRepository.Add(equipment);
            _applicationUnitOfWork.Save();
        }

        public void DeleteEquipment(Guid id)
        {
            _applicationUnitOfWork.EquipmentRepository.Remove(id);
            _applicationUnitOfWork.Save();
        }

        public Equipment GetEquipment(Guid id)
        {
            return _applicationUnitOfWork.EquipmentRepository.GetById(id);
        }

        public (IList<Equipment> data, int total, int totalDisplay) GetEquipments(int pageIndex, int pageSize, string? order, DataTablesSearch search)
        {
          return _applicationUnitOfWork.EquipmentRepository.GetPagedEquipments(pageIndex, pageSize, order, search);
        }

        public int GetTotalEquipmentCount()
        {
            return _applicationUnitOfWork.EquipmentRepository.GetCount();
        }

        public void Update(Equipment equipment)
        {
            _applicationUnitOfWork.EquipmentRepository.Update(equipment);
            _applicationUnitOfWork.Save();
        }
    }
}
