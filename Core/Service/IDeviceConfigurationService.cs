using Core.DTO;
using System.Collections.Generic;

namespace Core.Service
{
    public interface IDeviceConfigurationService
    {
        void Edit(DeviceConfiguration deviceConfiguration, DeviceConfigurationSpecialRead usrSetup);

        int Insert(DeviceConfiguration deviceConfiguration, DeviceConfigurationSpecialRead usrSetup);

        DeviceConfiguration Get(int id);

        DeviceConfigurationModel GetLastBySensorCode(string sensorCode);

        DeviceConfigurationModel GetLast(int deviceId, int motorId);

        DeviceConfiguration GetByDeviceId(int deviceId);

        IEnumerable<DeviceConfiguration> GetByMotorid(int motorId);
        void Remove(int idDeviceConfiguration);

        IEnumerable<DeviceConfiguration> GetByName(string name);
        IEnumerable<DeviceConfiguration> GetAll();

        IEnumerable<DeviceConfiguration> GetAllDTO();

        int GetlastCode();

        List<SelectListItemDTO> GetQueryDropDownList();

        List<SelectListItemDTO> GetQueryDropDownList(int IdMotor);

        List<SelectListItemDTO> GetQueryDropDownListByDeviceId(int deviceId);

        DeviceConfigurationSpecialRead GetUsrSetup(int motorId, int deviceId);

        List<DeviceConfigurationHorariosEnviosCard> GetHoras(int deviceConfigId);

        IDictionary<string, decimal?> GetLimitesAccSpd(int deviceId, int motorId, int reportType);
    }
}
