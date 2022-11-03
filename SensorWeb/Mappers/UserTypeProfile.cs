using AutoMapper;
using Core;
using Core.DTO;
using SensorWeb.Models;

namespace SensorWeb.Mappers
{
    public class UserTypeProfile : Profile
    {
        public UserTypeProfile()
        {
            CreateMap<UserTypeModel, UserType>().ReverseMap();
        }
    }
}
