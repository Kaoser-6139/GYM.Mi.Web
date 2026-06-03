using GYM.Domain;
using GYM.Domain.Dtos;
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


        public int GetTotalUsersCount()
        {
            return _applicationUnitOfWork.UserRepository.GetCount();
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
        // Trainer User Relationship (Trainers Controller )
        public (IList<User> data, int total, int totalDisplay) GetAvailableUsers(int pageIndex, int pageSize, string order, DataTablesSearch search)
        {
            return _applicationUnitOfWork.UserRepository.GetPagedUnassignedUsers(pageIndex, pageSize, order ?? "FullName asc", search);
        }

        public void AssignToTrainer(Guid userId, Guid trainerId)
        {
            var user = _applicationUnitOfWork.UserRepository.GetById(userId);
            if (user == null) throw new Exception("User not found");

            user.TrainerEmployeeId = trainerId;
            _applicationUnitOfWork.UserRepository.Update(user);
            _applicationUnitOfWork.Save();
        }

        public List<User> GetAssignedUsers(Guid id)
        {
            return _applicationUnitOfWork.UserRepository.GetByTrainerId(id);
        }

        public void UnassignFromTrainer(Guid userId)
        {
            var user = _applicationUnitOfWork.UserRepository.GetById(userId);
            if (user == null) throw new Exception("User not found");

            user.TrainerEmployeeId = null;

            _applicationUnitOfWork.UserRepository.Update(user);
            _applicationUnitOfWork.Save();
        }

        public async Task<(IList<UserListDto> data, int total, int totalDisplay)> GetUsersSP( int pageIndex, int pageSize, string? order,
                                                                                               DataTablesSearch search,  UserSearchDto searchItem)
        {
            return await _applicationUnitOfWork.GetUsersSP(
                pageIndex,
                pageSize,
                order,
                search,
                searchItem);
        }
    }
}
