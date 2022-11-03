using AutoMapper;
using Core;
using Core.DTO;
using SensorWeb.Models;

namespace SensorWeb.Mappers
{
    public class ContactProfile : Profile
    {
        public ContactProfile()
        {
            CreateMap<ContactModel, Contact>().ReverseMap();
        }
    }
}
