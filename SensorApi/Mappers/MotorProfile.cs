using AutoMapper;
using Core;
using Core.DTO;
using SensorApi.Models;

namespace SensorApi.Mappers
{
    public class MotorProfile : Profile
    {
        public MotorProfile()
        {
            CreateMap<MotorModel, Motor>().ReverseMap();
        }
    }
}
