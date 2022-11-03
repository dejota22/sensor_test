using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.DTO;
using Core.Service;

namespace SensorService
{
    public class CompanyUserService : ICompanyUserService
    {
        private readonly SensorContext _context;

        public CompanyUserService(SensorContext context)
        {
            _context = context;
        }

        void ICompanyUserService.Edit(CompanyUser companyUser)
        {
            _context.Update(companyUser);
            _context.SaveChanges();
        }

        private IQueryable<CardanShaftTypeDTO> GetQueryDTO()
        {
            IQueryable<CompanyUser> tb_CardanShaftType = _context.CompanyUser;

            //var query = from CompanyUser in tb_CardanShaftType
            //            orderby CompanyUser.Id ascending

            //            select new CardanShaftTypeDTO
            //            {
            //                Id = CompanyUser.Id,
            //                Name = CompanyUser.Name,
            //                CreatedAt = CompanyUser.CreateDate,

            //            };

            return null; ;

            //var query = from CompanyUser in tb_CardanShaftType
            //            select CompanyUser;
            //var query2 = _context.CompanyUser
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
            IQueryable<CompanyUser> tb_CardanShaftType = _context.CompanyUser;
            var query = (from CompanyUser in tb_CardanShaftType
                         select new SelectListItemDTO()
                         {
                             Key = CompanyUser.Id,
                             Value = CompanyUser.IdCompany.ToString() + CompanyUser.IdUser.ToString() 
                         }).Distinct().ToList();

            return query;
        }
        private IQueryable<CompanyUser> GetQuery()
        {
            IQueryable<CompanyUser> tb_CardanShaftType = _context.CompanyUser;
            var query = from CompanyUser in tb_CardanShaftType
                        select CompanyUser;

            //var query2 = _context.CompanyUser
            //       .Select(x => new Contact
            //       {
            //           Id = x.Contact.Id,
            //           FirstName = x.Contact.FirstName,
            //       });
            return query;

        }

        CompanyUser ICompanyUserService.Get(int idcardanShaftType)
        {
            return GetQuery().Where(x => x.Id.Equals(idcardanShaftType)).FirstOrDefault();
        }

        IEnumerable<CompanyUser> ICompanyUserService.GetAll()
        {
            return GetQuery();
        }

        IEnumerable<CardanShaftTypeDTO> ICompanyUserService.GetAllDTO()
        {
            return GetQueryDTO();
        }

        IEnumerable<CompanyUser> ICompanyUserService.GetByName(string name)
        {
            return null;// GetQuery().Where(x => x.Name.Equals(name));
        }

        int ICompanyUserService.Insert(CompanyUser companyUser)
        {
            companyUser.CreateDate = DateTime.Now;            

            _context.Add(companyUser);
            _context.SaveChanges();
            return companyUser.Id;
        }

        void ICompanyUserService.Remove(int idcardanShaftType)
        {
            var _cardanShaftType = _context.CompanyUser.Find(idcardanShaftType);
            _context.Remove(_cardanShaftType);
            _context.SaveChanges();
        }

        int ICompanyUserService.GetlastCode()
        {
            return GetQuery().ToList().LastOrDefault().Id + 1;
        }

        List<SelectListItemDTO> ICompanyUserService.GetQueryDropDownList()
        {
            return GetQueryDropDownList();
        }
    }
}
