using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Core;
using Core.DTO;
using Core.Service;
using Microsoft.EntityFrameworkCore;

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
            IQueryable<Company> tb_Company = _context.Company.Include(c => c.ParentCompany);
            var query = from Company in tb_Company
                        select Company;

            return query;
        }
        private IQueryable<Company> GetQueryFull()
        {
            IQueryable<Company> tb_Company = _context.Company.Include(c => c.ParentCompany)
                .ThenInclude(c => c.Contact).ThenInclude(c => c.Company);
            var query = from Company in tb_Company
                        select Company;

            return query;

        }

        Company ICompanyService.Get(int idCompany)
        {
            return GetQuery().Where(x => x.Id.Equals(idCompany)).FirstOrDefault();
        }

        IEnumerable<Company> ICompanyService.GetAll(bool full = false)
        {
            if (full == false)
                return GetQuery();
            else
                return GetQueryFull();
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

        List<SelectListItemDTO> ICompanyService.GetQueryDropDownListStrict(string userId)
        {
            var user = _context.User.Include(u => u.Contact).FirstOrDefault(u => u.Id == Convert.ToInt32(userId));

            IQueryable<Company> tb_Company = _context.Company;
            var query = tb_Company.ToList();
            
            if (user.UserTypeId != 1)
            {
                var userCompany = user.Contact.CompanyId;
                var companies = _context.Company.Where(x => x.ParentCompanyId == userCompany).ToList();
                query = query.Where(c => c.Id == user.Contact.CompanyId || companies.Any(y => y.Id == c.Id)).ToList();
            }
            
            var sel = query.Select(c => new SelectListItemDTO()
                {
                    Key = c.Id,
                    Value = c.LegalName
                }).Distinct().ToList();

            return sel;
        }

        List<SelectListCustomItemDTO> ICompanyService.GetQueryDropDownListStrictCustom(string userId)
        {
            var user = _context.User.Include(u => u.Contact).FirstOrDefault(u => u.Id == Convert.ToInt32(userId));

            IQueryable<Company> tb_Company = _context.Company;
            var query = tb_Company.ToList();

            if (user.UserTypeId != 1)
            {
                var userCompany = user.Contact.CompanyId;
                var companies = _context.Company.Where(x => x.ParentCompanyId == userCompany).ToList();
                query = query.Where(c => c.Id == user.Contact.CompanyId || companies.Any(y => y.Id == c.Id)).ToList();
            }

            var sel = query.Select(c => new SelectListCustomItemDTO()
            {
                Key = c.Id,
                Value = c.LegalName,
                CompanyId = c.CompanyTypeId
            }).Distinct().ToList();

            return sel;
        }

        string ICompanyService.GetRelatedLocks(int idEmpresa)
        {
            string locks = "";

            var contacts = _context.Contact.Where(c => c.CompanyId == idEmpresa);
            var devices = _context.Device.Where(c => c.CompanyId == idEmpresa);
            var motors = _context.Motor.Where(c => c.CompanyId == idEmpresa);
            var units = _context.CompanyUnit.Where(c => c.CompanyId == idEmpresa);

            if (contacts.Any())
            {
                locks += "pelos contatos: ";
                locks +=  string.Join(",", contacts.Select(c => c.Email));
            }
            if (devices.Any())
            {
                locks += " e pelos sensores: ";
                locks += string.Join(",", devices.Select(c => c.Tag));
            }
            if(motors.Any())
            {
                locks += " e pelos equipamentos: ";
                locks += string.Join(",", motors.Select(c => c.Name));
            }
            if (units.Any())
            {
                locks += " e pelas unidades: ";
                locks += string.Join(",", units.Select(c => c.Name));
            }

            return $"Esta empresa não pode ser removida. Ela é usada {locks}";
        }
    }
}
