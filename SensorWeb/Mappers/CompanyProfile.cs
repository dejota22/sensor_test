using AutoMapper;
using Core;
using Core.DTO;
using SensorWeb.Models;

namespace SensorWeb.Mappers
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<CompanyModel, Company>().ReverseMap();
        }
    }

    public class CompanyUnitProfile : Profile
    {
        public CompanyUnitProfile()
        {
            CreateMap<CompanyUnitModel, CompanyUnit>().ReverseMap();
        }
    }
}
