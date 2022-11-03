using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.DTO;
using Core.Service;

namespace SensorService
{
  public  class ContactService : IContactService
    {
        private readonly SensorContext _context;

        public ContactService(SensorContext context)
        {
            _context = context;
        }

        void IContactService.Edit(Contact contact)
        {
            contact.UpdatedAt = DateTime.Now;

            _context.Update(contact);
            _context.SaveChanges();
        }

        private IQueryable<Contact> GetQuery()
        {
            IQueryable<Contact> tb_contact = _context.Contact;
            var query = from contact in tb_contact
                        select contact;

            return query;

        }

        Contact IContactService.Get(int idcontact)
        {
            return GetQuery().Where(x => x.Id.Equals(idcontact)).FirstOrDefault();
        }

        IEnumerable<Contact> IContactService.GetAll()
        {
            return GetQuery();
        }

        IEnumerable<ContactDTO> IContactService.GetbyDescendenceName()
        {
            IQueryable<Contact> tb_contact = _context.Contact;

            //var query = from contact in tb_contact
            //            where contact.FirstName.StartsWith(contact.FirstName)
            //            orderby contact.FirstName descending

            //            //select new ContactDTO
            //            //{
            //            //    Contact = new Contact()
            //            //    {
            //            //        FirstName = contact.FirstName
            //            //    }
            //            //};

            //return query;
            return null;
        }

        IEnumerable<Contact> IContactService.GetByName(string name)
        {
            return GetQuery().Where(x => x.FirstName.Equals(name));
        }

        int IContactService.Insert(Contact contact)
        {
            //contact.ContactId = 1;

            //contact.Contact = new Contact
            //{
            //    FirstName = "Henrique",
            //    Email = contact.Email,
            //    CompanyId = 1,
            //    IsActive = 1,
            //    CreatedAt = DateTime.Now,
            //    UpdatedAt = DateTime.Now,
            //    Surname = "Sergio"
            //};
            //contact.contactTypeId = 1;

            //_context.Add(contact);
            //_context.SaveChanges();
            //return contact.Id;
            return 0;
        }

        void IContactService.Remove(int idcontact)
        {
            var _contact = _context.Contact.Find(idcontact);
            _context.Remove(_contact);
            _context.SaveChanges();
        }

        User IContactService.GetUser(int idUser)
        {
            throw new NotImplementedException();
        }
    }
}
