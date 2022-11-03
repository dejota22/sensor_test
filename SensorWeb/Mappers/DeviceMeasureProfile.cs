using AutoMapper;
using Core;
using Core.DTO;
using SensorWeb.Models;

namespace SensorWeb.Mappers
{
    public class DeviceMeasureProfile : Profile
    {
        public DeviceMeasureProfile()
        {
            CreateMap<DeviceMeasureModel, DeviceMeasure>().ReverseMap();
        }
    }
}
