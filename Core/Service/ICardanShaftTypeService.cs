using Core.DTO;
using System.Collections.Generic;

namespace Core.Service
{
    public interface ICardanShaftTypeService
    {
        void Edit(CardanShaftType cardanShaftType);

        int Insert(CardanShaftType cardanShaftType);

        CardanShaftType Get(int idcardanShaftType);

        void Remove(int idcardanShaftType);

        IEnumerable<CardanShaftType> GetByName(string name);
        IEnumerable<CardanShaftType> GetAll();

        IEnumerable<CardanShaftTypeDTO> GetAllDTO();

        int GetlastCode();

        List<SelectListItemDTO> GetQueryDropDownList();
    }
}
