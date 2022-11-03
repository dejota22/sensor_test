using Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Service
{
    public interface IUserService
    {
        void Edit(User user);

        int Insert(User user);

        User Get(int idUser);

        void Remove(int iduser);

        IEnumerable<User> GetByName(string name);
        IEnumerable<User> GetAll();

        IEnumerable<UserDTO> GetAllDTO();

        IEnumerable<UserDTO> GetbyDescendenceName();

        User Login(string login, string pass);

        List<SelectListItemDTO> GetQueryDropDownList();
    }
}
