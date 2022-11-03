using Core.DTO;
using System.Collections.Generic;

namespace Core.Service
{    
    public interface IDeviceService
    {
        void Edit(Device device);

        int Insert(Device device);

        Device Get(int idDevice);

        void Remove(int idDevice);

        IEnumerable<Device> GetByName(string name);
        IEnumerable<Device> GetAll();

        IEnumerable<DeviceDTO> GetAllDTO();

        Device GetByDeviceTag(string deviceTag);

        int GetlastCode();

        List<SelectListItemDTO> GetQueryDropDownList();

        List<SelectListItemDTO> GetQueryDropDownListByTag();
    }
}
