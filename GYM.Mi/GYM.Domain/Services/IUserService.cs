using GYM.Domain.Dtos;
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
       
        int GetTotalUsersCount();
        User GetUser(Guid id);
        (IList<User> data, int total, int totalDisplay) GetUsers(int pageIndex, int pageSize, string? order, 
                                                               DataTablesSearch search);
        void Update(User author);

        // Trainer User Relationship (Trainers Controller )
        (IList<User> data, int total, int totalDisplay) GetAvailableUsers(int pageIndex, int pageSize, string order,
                                                                           DataTablesSearch search);
        void AssignToTrainer(Guid userId, Guid trainerId);
        List<User> GetAssignedUsers(Guid id);
        void UnassignFromTrainer(Guid userId);

        //Advanced Search
        Task<(IList<UserListDto> data, int total, int totalDisplay)> GetUsersSP( int pageIndex, int pageSize,  string? order,
                                                                                   DataTablesSearch search,UserSearchDto searchItem);
    }   
}
