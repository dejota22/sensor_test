using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.DTO;
using Core.Service;

namespace SensorService
{
    public class CompanyService : ICompanyService
    {
        private readonly SensorContext _context;

        public CompanyService(SensorContext context)
        {
            _context = context;
        }

        void ICompanyService.Edit(Company Company)
        {
            Company.UpdatedAt = DateTime.Now;
            _context.Update(Company);
            _context.SaveChanges();
        }

        private IQueryable<CompanyDTO> GetQueryDTO()
        {
            IQueryable<Company> tb_Company = _context.Company;

            var query = from Company in tb_Company
                        orderby Company.Id ascending

                        select new CompanyDTO
                        {
                            Id = Company.Id,
                            LegalName = Company.LegalName,
                            CreatedAt = Company.CreatedAt,
                            UpdatedAt = Company.UpdatedAt,
                        };

            return query;

            //var query = from Company in tb_Company
            //            select Company;
            //var query2 = _context.Company
            //       .Select(x => new Contact
            //       {
            //           Id = x.Contact.Id,
            //           FirstName = x.Contact.FirstName,
            //       });

        }


        /// <summary>
        /// GetQueryDropDownList
        /// </summary>
        /// <returns></returns>
        private List<SelectListItemDTO> GetQueryDropDownList()
        {
            IQueryable<Company> tb_Company = _context.Company;
            var query = (from Company in tb_Company
                         select new SelectListItemDTO()
                         {
                             Key = Company.Id,
                             Value = Company.LegalName
                         }).Distinct().ToList();

            return query;
        }
        private IQueryable<Company> GetQuery()
        {
            IQueryable<Company> tb_Company = _context.Company;
            var query = from Company in tb_Company
                        select Company;

            //var query2 = _context.Company
            //       .Select(x => new Contact
            //       {
            //           Id = x.Contact.Id,
            //           FirstName = x.Contact.FirstName,
            //       });
            return query;

        }

        Company ICompanyService.Get(int idCompany)
        {
            return GetQuery().Where(x => x.Id.Equals(idCompany)).FirstOrDefault();
        }

        IEnumerable<Company> ICompanyService.GetAll()
        {
            return GetQuery();
        }

        IEnumerable<CompanyDTO> ICompanyService.GetAllDTO()
        {
            return GetQueryDTO();
        }

        IEnumerable<Company> ICompanyService.GetByName(string name)
        {
            return GetQuery().Where(x => x.LegalName.Equals(name));
        }

        int ICompanyService.Insert(Company Company)
        {
            Company.CreatedAt = DateTime.Now;
            Company.UpdatedAt = DateTime.Now;

            _context.Add(Company);
            _context.SaveChanges();
            return Company.Id;
        }

        void ICompanyService.Remove(int idCompany)
        {
            var _Company = _context.Company.Find(idCompany);
            _context.Remove(_Company);
            _context.SaveChanges();
        }

        int ICompanyService.GetlastCode()
        {
            return GetQuery().ToList().LastOrDefault().Id + 1;
        }

        List<SelectListItemDTO> ICompanyService.GetQueryDropDownList()
        {
            return GetQueryDropDownList();
        }
    }
}
