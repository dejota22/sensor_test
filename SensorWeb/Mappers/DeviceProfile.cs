using AutoMapper;
using Core;
using Core.DTO;
using SensorWeb.Models;

namespace SensorWeb.Mappers
{
    public class DeviceProfile : Profile
    {
        public DeviceProfile()
        {
            CreateMap<DeviceModel, Device>().ReverseMap();
        }
    }
}
