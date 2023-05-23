using Core.DTO;
using System.Collections.Generic;

namespace Core.Service
{
    public interface IDeviceMeasureService
    {
        void Edit(DeviceMeasure deviceMeasure);

        int Insert(DeviceMeasure deviceMeasure);

        DeviceMeasure Get(int id);

        DeviceMeasure GetByDeviceId(int deviceId);

        IEnumerable<DeviceMeasureDTO> GetByMotorid(int motorId);
        void Remove(int idDeviceMeasure);

        IEnumerable<DeviceMeasure> GetByName(string name);
        IEnumerable<DeviceMeasure> GetAll();

        IEnumerable<DeviceMeasureDTO> GetAllDTO();

        int GetlastCode();

        //DeviceMeasure GetByReadDataType(string readData);

        List<SelectListItemDTO> GetQueryDropDownList();

        List<SelectListItemDTO> GetQueryDropDownList(int IdMotor);

        List<SelectListItemDTO> GetQueryDropDownListByDeviceId(int deviceId);
    }
}
