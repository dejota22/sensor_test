using Core.DTO;
using System.Collections.Generic;

namespace Core.Service
{    
    public interface IMotorService
    {
        #region Equipamento e Agrupamento

        IEnumerable<Motor> GetAll();
        IEnumerable<MotorDTO> GetAllDTO();
        Motor Get(int idMotor);
        IEnumerable<Motor> GetByName(string name);
        List<SelectListItemDTO> GetQueryDropDownList();
        List<SelectListItemDTO> GetQueryDropDownListByTag();
        int GetlastCode();
        void Edit(Motor motor);
        int Insert(Motor motor);
        void Remove(int idMotor);

        #endregion


        #region Equipamento

        IEnumerable<Motor> GetAllEquipamento();
        List<SelectListItemDTO> GetQueryDropDownListEquipamento();

        #endregion


        #region Agrupamento

        IEnumerable<Motor> GetAllAgrupamento();

        #endregion
    }
}
