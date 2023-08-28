using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.DTO;
using Core.Service;

namespace SensorService
{
  public  class CompanyAlertContactService : ICompanyAlertContactService
    {
        private readonly SensorContext _context;

        public CompanyAlertContactService(SensorContext context)
        {
            _context = context;
        }

        void ICompanyAlertContactService.Edit(CompanyAlertContact contact)
        {
            _context.Update(contact);
            _context.SaveChanges();
        }

        private IQueryable<CompanyAlertContact> GetQuery()
        {
            IQueryable<CompanyAlertContact> tb_contact = _context.CompanyAlertContact;
            var query = from contact in tb_contact
                        select contact;

            return query;

        }

        CompanyAlertContact ICompanyAlertContactService.Get(int idcontact)
        {
            return GetQuery().Where(x => x.Id.Equals(idcontact)).FirstOrDefault();
        }

        IEnumerable<CompanyAlertContact> ICompanyAlertContactService.GetAll()
        {
            return GetQuery();
        }

        IEnumerable<CompanyAlertContact> ICompanyAlertContactService.GetByCompany(int idCompany)
        {
            return GetQuery().Where(x => x.CompanyId.Equals(idCompany));
        }

        int ICompanyAlertContactService.Insert(CompanyAlertContact contact)
        {
            _context.Add(contact);
            _context.SaveChanges();
            return contact.Id;
        }

        void ICompanyAlertContactService.Remove(int idcontact)
        {
            var _contact = _context.CompanyAlertContact.Find(idcontact);
            _context.Remove(_contact);
            _context.SaveChanges();
        }
    }
}
