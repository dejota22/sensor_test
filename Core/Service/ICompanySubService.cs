using Core.DTO;
using System.Collections.Generic;

namespace Core.Service
{
    public interface ICompanySubService
    {
        void Edit(CompanySub companySub);

        int Insert(CompanySub companySub);

        CompanySub Get(int idcardanShaftType);

        void Remove(int idcardanShaftType);

        IEnumerable<CompanySub> GetByName(string name);
        IEnumerable<CompanySub> GetAll();

        IEnumerable<CardanShaftTypeDTO> GetAllDTO();

        int GetlastCode();

        List<SelectListItemDTO> GetQueryDropDownList();
    }
}
