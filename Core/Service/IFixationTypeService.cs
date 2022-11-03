using Core.DTO;
using System.Collections.Generic;

namespace Core.Service
{
    public interface IFixationTypeService
    {
        void Edit(FixationType FixationType);

        int Insert(FixationType FixationType);

        FixationType Get(int idFixationType);

        void Remove(int idFixationType);

        IEnumerable<FixationType> GetByName(string name);
        IEnumerable<FixationType> GetAll();

        IEnumerable<FixationTypeDTO> GetAllDTO();

        int GetlastCode();

        List<SelectListItemDTO> GetQueryDropDownList();
    }
}
