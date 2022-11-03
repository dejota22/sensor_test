using Core.DTO;
using System.Collections.Generic;

namespace Core.Service
{
    public interface IMachineService
    {
        void Edit(Machine Machine);

        int Insert(Machine Machine);

        Machine Get(int idMachine);

        void Remove(int idMachine);

        IEnumerable<Machine> GetByName(string name);
        IEnumerable<Machine> GetAll();

        IEnumerable<MachineDTO> GetAllDTO();

        int GetlastCode();

        List<SelectListItemDTO> GetQueryDropDownList();
    }
}
