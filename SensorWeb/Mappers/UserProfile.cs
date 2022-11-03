using AutoMapper;
using Core;
using Core.DTO;
using SensorWeb.Models;

namespace SensorWeb.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserModel, User>().ReverseMap();
        }
    }
}
