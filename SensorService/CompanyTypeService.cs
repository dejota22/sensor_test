using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core;
using Core.DTO;
using Core.Service;

namespace SensorService
{
    public class CompanyTypeService : ICompanyTypeService
    {
        private readonly SensorContext _context;

        public CompanyTypeService(SensorContext context)
        {
            _context = context;
        }

        void ICompanyTypeService.Edit(CompanyType company)
        {
            _context.Update(company);
            _context.SaveChanges();
        }

        private IQueryable<CompanyTypeDTO> GetQueryDTO()
        {
            IQueryable<CompanyType> tb_companyType = _context.CompanyType;

            var query = from companyType in tb_companyType
                        orderby companyType.Id ascending

                        select new CompanyTypeDTO
                        {
                            Id = companyType.Id,
                            Name = companyType.Name,
                            CreatedAt = companyType.CreatedAt,
                            UpdatedAt = companyType.UpdatedAt,
                        };

            return query;
        }


        /// <summary>
        /// GetQueryDropDownList
        /// </summary>
        /// <returns></returns>
        private List<SelectListItemDTO> GetQueryDropDownList()
        {
            IQueryable<CompanyType> tb_companyType = _context.CompanyType;
            var query = (from companyType in tb_companyType
                         select new SelectListItemDTO()
                         {
                             Key = companyType.Id,
                             Value = companyType.Name
                         }).Distinct().ToList();

            return query;
        }
        private IQueryable<CompanyType> GetQuery()
        {
            IQueryable<CompanyType> tb_companyType = _context.CompanyType;
            var query = from companyType in tb_companyType
                        select companyType;
            return query;

        }

        CompanyType ICompanyTypeService.Get(int iduserType)
        {
            return GetQuery().Where(x => x.Id.Equals(iduserType)).FirstOrDefault();
        }

        IEnumerable<CompanyType> ICompanyTypeService.GetAll()
        {
            return GetQuery();
        }

        IEnumerable<CompanyTypeDTO> ICompanyTypeService.GetAllDTO()
        {
            return GetQueryDTO();
        }

        IEnumerable<CompanyType> ICompanyTypeService.GetByName(string name)
        {
            return GetQuery().Where(x => x.Name.Equals(name));
        }

        int ICompanyTypeService.Insert(CompanyType companyType)
        {
            companyType.CreatedAt = DateTime.Now;
            companyType.UpdatedAt = DateTime.Now;

            _context.Add(companyType);
            _context.SaveChanges();
            return companyType.Id;
        }

        void ICompanyTypeService.Remove(int iduserType)
        {
            var _userType = _context.CompanyType.Find(iduserType);
            _context.Remove(_userType);
            _context.SaveChanges();
        }

        int ICompanyTypeService.GetlastCode()
        {
            return GetQuery().ToList().LastOrDefault().Id + 1;
        }

        List<SelectListItemDTO> ICompanyTypeService.GetQueryDropDownList()
        {
            return GetQueryDropDownList();
        }
    }
}
