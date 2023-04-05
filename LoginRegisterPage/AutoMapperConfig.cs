using AutoMapper;
using LoginRegisterPage.Entities;
using LoginRegisterPage.Models;
using static LoginRegisterPage.Models.UserModel;

namespace LoginRegisterPage
{
    public class AutoMapperConfig:Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<User, UserModel>().ReverseMap();
            CreateMap<User, CreateUserModel>().ReverseMap();
        }
    }
}
