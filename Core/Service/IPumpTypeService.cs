using Core.DTO;
using System.Collections.Generic;

namespace Core.Service
{
    public interface IPumpTypeService
    {
        void Edit(PumpType PumpType);

        int Insert(PumpType PumpType);

        PumpType Get(int idPumpType);

        void Remove(int idPumpType);

        IEnumerable<PumpType> GetByName(string name);
        IEnumerable<PumpType> GetAll();

        IEnumerable<PumpTypeDTO> GetAllDTO();

        int GetlastCode();

        List<SelectListItemDTO> GetQueryDropDownList();
    }
}
