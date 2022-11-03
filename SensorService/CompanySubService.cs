using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.DTO;
using Core.Service;

namespace SensorService
{
    public class CompanySubService : ICompanySubService
    {
        private readonly SensorContext _context;

        public CompanySubService(SensorContext context)
        {
            _context = context;
        }

        void ICompanySubService.Edit(CompanySub companySub)
        {
            _context.Update(companySub);
            _context.SaveChanges();
        }

        private IQueryable<CardanShaftTypeDTO> GetQueryDTO()
        {
            IQueryable<CompanySub> tb_CardanShaftType = _context.CompanySub;

            //var query = from CompanySub in tb_CardanShaftType
            //            orderby CompanySub.Id ascending

            //            select new CardanShaftTypeDTO
            //            {
            //                Id = CompanySub.Id,
            //                Name = CompanySub.Name,
            //                CreatedAt = CompanySub.CreateDate,

            //            };

            return null; ;

            //var query = from CompanySub in tb_CardanShaftType
            //            select CompanySub;
            //var query2 = _context.CompanySub
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
            IQueryable<CompanySub> tb_CardanShaftType = _context.CompanySub;
            var query = (from CompanySub in tb_CardanShaftType
                         select new SelectListItemDTO()
                         {
                             Key = CompanySub.Id,
                             Value = CompanySub.IdCompany.ToString() + CompanySub.IdCompanySub.ToString() 
                         }).Distinct().ToList();

            return query;
        }
        private IQueryable<CompanySub> GetQuery()
        {
            IQueryable<CompanySub> tb_CardanShaftType = _context.CompanySub;
            var query = from CompanySub in tb_CardanShaftType
                        select CompanySub;

            //var query2 = _context.CompanySub
            //       .Select(x => new Contact
            //       {
            //           Id = x.Contact.Id,
            //           FirstName = x.Contact.FirstName,
            //       });
            return query;

        }

        CompanySub ICompanySubService.Get(int idcardanShaftType)
        {
            return GetQuery().Where(x => x.Id.Equals(idcardanShaftType)).FirstOrDefault();
        }

        IEnumerable<CompanySub> ICompanySubService.GetAll()
        {
            return GetQuery();
        }

        IEnumerable<CardanShaftTypeDTO> ICompanySubService.GetAllDTO()
        {
            return GetQueryDTO();
        }

        IEnumerable<CompanySub> ICompanySubService.GetByName(string name)
        {
            return null;// GetQuery().Where(x => x.Name.Equals(name));
        }

        int ICompanySubService.Insert(CompanySub companySub)
        {
            companySub.CreateDate = DateTime.Now;            

            _context.Add(companySub);
            _context.SaveChanges();
            return companySub.Id;
        }

        void ICompanySubService.Remove(int idcardanShaftType)
        {
            var _cardanShaftType = _context.CompanySub.Find(idcardanShaftType);
            _context.Remove(_cardanShaftType);
            _context.SaveChanges();
        }

        int ICompanySubService.GetlastCode()
        {
            return GetQuery().ToList().LastOrDefault().Id + 1;
        }

        List<SelectListItemDTO> ICompanySubService.GetQueryDropDownList()
        {
            return GetQueryDropDownList();
        }
    }
}
