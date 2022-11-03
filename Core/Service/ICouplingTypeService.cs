using Core.DTO;
using System.Collections.Generic;

namespace Core.Service
{
    public interface ICouplingTypeService
    {
        void Edit(CouplingType couplingType);

        int Insert(CouplingType couplingType);

        CouplingType Get(int idCouplingType);

        void Remove(int idCouplingType);

        IEnumerable<CouplingType> GetByName(string name);
        IEnumerable<CouplingType> GetAll();

        IEnumerable<CouplingTypeDTO> GetAllDTO();

        int GetlastCode();

        List<SelectListItemDTO> GetQueryDropDownList();
    }
}
