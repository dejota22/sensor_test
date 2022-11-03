using Core.DTO;
using System.Collections.Generic;

namespace Core.Service
{    
    public interface IMotorService
    {
        void Edit(Motor motor);

        int Insert(Motor motor);

        Motor Get(int idMotor);

        void Remove(int idMotor);

        IEnumerable<Motor> GetByName(string name);
        IEnumerable<Motor> GetAll();

        IEnumerable<MotorDTO> GetAllDTO();

        Motor GetBymotorTag(string motorTag);

        int GetlastCode();

        List<SelectListItemDTO> GetQueryDropDownList();

        List<SelectListItemDTO> GetQueryDropDownListByTag();
    }
}
