using Core.DTO;
using System.Collections.Generic;

namespace Core.Service
{
    public interface IUserTypeService
    {
        void Edit(UserType userType);

        int Insert(UserType userType);

        UserType Get(int iduserType);

        void Remove(int iduserType);

        IEnumerable<UserType> GetByName(string name);
        IEnumerable<UserType> GetAll();

        IEnumerable<UserTypeDTO> GetAllDTO();

        int GetlastCode();

        List<SelectListItemDTO> GetQueryDropDownList();
    }
}
