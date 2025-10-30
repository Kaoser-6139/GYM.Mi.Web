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
    public class UserService : IUserService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public UserService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public void AddUser(User user)
        {
             _applicationUnitOfWork.UserRepository.Add(user);
            _applicationUnitOfWork.Save();
        }

        public void DeleteUser(Guid id)
        {
            _applicationUnitOfWork.UserRepository.Remove(id);
            _applicationUnitOfWork.Save();
        }

        public User GetUser(Guid id)
        {
            return _applicationUnitOfWork.UserRepository.GetById(id);
        }

        public (IList<User> data, int total, int totalDisplay) GetUsers(int pageIndex, int pageSize, string? order, DataTablesSearch search)
        {
            return _applicationUnitOfWork.UserRepository.GetPagedusers(pageIndex, pageSize, order, search);
        }

        public void Update(User author)
        {
            _applicationUnitOfWork.UserRepository.Update(author);
            _applicationUnitOfWork.Save();
        }
    }
}
