using AutoMapper;
using GYM.Domain.Dtos;
using GYM.Domain.Entities;
using GYM.Mi.Areas.Admin.Models;

namespace GYM.Mi
{
    public class WebProfile:Profile
    {
        public WebProfile() 
        { 
            //Users
            CreateMap<AddUserModel,User>().ReverseMap();
            CreateMap<UpdateUserModel, User>().ReverseMap(); 


            //Equipments
            CreateMap<AddEquipmentModel, Equipment>().ReverseMap();
            CreateMap<UpdateEquipmentModel,Equipment>().ReverseMap();

            //Employee
            CreateMap<AddEmployeeModel, Employee>().ReverseMap();
            CreateMap<UpdateEmployeeModel, Employee>().ReverseMap();

            //ManageStudentsForTrainer
            CreateMap<ManageStudentsForTrainerModel,Employee>().ReverseMap();

            //Blog
            CreateMap<AddBlogModel, Blog>();
            CreateMap<UpdateBlogModel, Blog>();
            CreateMap<Blog, UpdateBlogModel>();
            CreateMap<Blog, BlogDetailsModel>();

            //Advanced search
            CreateMap<UserSearchModel, UserSearchDto>();
        }
    }
}
