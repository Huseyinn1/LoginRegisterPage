using AutoMapper;
using LoginRegisterPage.Entities;
using LoginRegisterPage.Models;

namespace LoginRegisterPage
{
    public class AutoMapperConfig:Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<User, UserModel>().ReverseMap();
        }
    }
}
