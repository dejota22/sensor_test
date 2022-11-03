using Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Service
{
    public interface IContactService
    {
        void Edit(Contact Contact);

        int Insert(Contact Contact);

        Contact Get(int idContact);

        IEnumerable<Contact> GetByName(string name);
        IEnumerable<Contact> GetAll();

        void Remove(int idContact);

        IEnumerable<ContactDTO> GetbyDescendenceName();
        User GetUser(int idUser);
    }
}
