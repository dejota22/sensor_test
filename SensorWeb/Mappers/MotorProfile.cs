using AutoMapper;
using Core;
using Core.DTO;
using SensorWeb.Models;

namespace SensorWeb.Mappers
{
    public class MotorProfile : Profile
    {
        public MotorProfile()
        {
            CreateMap<MotorModel, Motor>().ReverseMap();
        }
    }
}
