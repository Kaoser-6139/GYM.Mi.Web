using AutoMapper;
using GYM.Domain.Entities;
using GYM.Mi.Areas.Admin.Models;

namespace GYM.Mi
{
    public class WebProfile:Profile
    {
        public WebProfile() 
        { 
            CreateMap<AddUserModel,User>().ReverseMap();
            CreateMap<UpdateUserMode, User>().ReverseMap(); 
        }
    }
}
