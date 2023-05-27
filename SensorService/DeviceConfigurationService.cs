using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.ApiModel.Request;
using Core.DTO;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace SensorService
{
    public class DeviceConfigurationService : IDeviceConfigurationService
    {
        private readonly SensorContext _context;
        private readonly IDeviceService _device;

        public DeviceConfigurationService(SensorContext context)
        {
            _context = context;
        }

        //void IUserTypeService.Edit(UserType userType)
        //{
        //    _context.Update(userType);
        //    _context.SaveChanges();
        //}

        private IQueryable<DeviceConfiguration> GetQueryDTO(int IdMotor)
        {
            IQueryable<DeviceConfiguration> tb_userType = _context.DeviceConfiguration;

            var query = from register in tb_userType
                        where register.MotorId == IdMotor
                        orderby register.Id ascending
                        select register;         

            return query;

        }

        /// <summary>
        /// GetQueryDropDownList
        /// </summary>
        /// <returns></returns>
        private List<SelectListItemDTO> GetQueryDropDownList()
        {
            IQueryable<DeviceConfiguration> tb_userType = _context.DeviceConfiguration;
            var query = (from deviceConfiguration in tb_userType
                         select new SelectListItemDTO()
                         {
                             Key = deviceConfiguration.Id,
                             Value = deviceConfiguration.CreatedAt.ToString("dd/MM/yyyy H:mm:ss") + " " + deviceConfiguration.Id 
                         }).Distinct().ToList();

            return query;
        }

        /// <summary>
        /// GetQueryDropDownList
        /// </summary>
        /// <returns></returns>
        private List<SelectListItemDTO> GetQueryDropDownList(int IdMotor)
        {
            IQueryable<DeviceConfiguration> tb_userType = _context.DeviceConfiguration;
            var query = (from deviceConfiguration in tb_userType
                         where deviceConfiguration.MotorId == IdMotor
                         select new SelectListItemDTO()
                         {
                             Key = deviceConfiguration.Id,
                             Value = deviceConfiguration.CreatedAt.ToString("dd/MM/yyyy H:mm:ss") + " " + deviceConfiguration.Id
                         }).Distinct().ToList();

            return query;
        }

        /// <summary>
        /// GetQueryDropDownList
        /// </summary>
        /// <returns></returns>
        private List<SelectListItemDTO> GetQueryDropDownListByDeviceId(int deviceId)
        {
            IQueryable<DeviceConfiguration> tb_userType = _context.DeviceConfiguration;
            var query = (from deviceConfiguration in tb_userType
                         where deviceConfiguration.DeviceId == deviceId
                         select new SelectListItemDTO()
                         {
                             Key = deviceConfiguration.Id,
                             Value = deviceConfiguration.CreatedAt.ToString("dd/MM/yyyy H:mm:ss") + " - " + deviceConfiguration.Id
                         }).Distinct().ToList();

            return query;
        }

        private IQueryable<DeviceConfiguration> GetQuery()
        {
            IQueryable<DeviceConfiguration> tb_deviceConfiguration = _context.DeviceConfiguration;
            var query = from deviceConfiguration in tb_deviceConfiguration
                        select deviceConfiguration;

            return query;
        }

        public DeviceConfiguration GetByDeviceId(int deviceId)
        {
            return GetQuery().Where(x => x.DeviceId.Equals(deviceId)).FirstOrDefault();
        }

        public DeviceConfiguration Get(int id)
        {
            return GetQuery().Where(x => x.Id.Equals(id)).FirstOrDefault();
        }

        public DeviceConfigurationModel GetLast(int deviceId, int motorId)
        {
            var config = new DeviceConfigurationModel();
            var configs = GetQuery().Where(c => c.MotorId == motorId && c.DeviceId == deviceId)
                .AsNoTracking().ToList();
            if (configs.Any())
            {
                config = config.GetModelFromEntity(configs.LastOrDefault());
                config.DeviceConfigurationHorariosEnviosCard = GetHoras(config.Id);
            }
                

            IQueryable<DeviceConfigurationLora> tb_deviceConfigurationLora = _context.DeviceConfigurationLora;
            var lora = tb_deviceConfigurationLora.FirstOrDefault();
            if (lora != null)
            {
                config.canal = lora.canal;
                config.sf = lora.sf;
                config.bw = lora.bw;
                config.end = lora.end;
                config.gtw = lora.gtw;
                config.syn = lora.syn;
            }

            return config;
        }

        IEnumerable<DeviceConfiguration> IDeviceConfigurationService.GetByMotorid(int motorId)
        {
            return GetQueryDTO(motorId);
        }
        public int Insert(DeviceConfiguration deviceConfiguration)
        {
            deviceConfiguration.CreatedAt = DateTime.Now;

            _context.DeviceConfiguration.Add(deviceConfiguration);
            _context.SaveChanges();

            return deviceConfiguration.Id;
        }

        List<SelectListItemDTO> IDeviceConfigurationService.GetQueryDropDownList()
        {
            return GetQueryDropDownList();
        }

        
        public DeviceConfigurationSpecialRead GetUsrSetup(int motorId, int deviceId)
        {
            IQueryable<DeviceConfigurationSpecialRead> tb_device_special_read = _context.DeviceConfigurationSpecialRead;
            
            return tb_device_special_read.Where(d => d.DeviceId == deviceId && d.MotorId == motorId).FirstOrDefault();
        }

        public List<DeviceConfigurationHorariosEnviosCard> GetHoras(int deviceConfigId)
        {
            IQueryable<DeviceConfigurationHorariosEnviosCard> tb_device_horas = _context.DeviceConfigurationHorariosEnviosCard;

            return tb_device_horas.Where(d => d.DeviceConfigurationId == deviceConfigId).ToList();
        }

        public void Edit(DeviceConfiguration deviceConfiguration)
        {
            _context.DeviceConfiguration.Update(deviceConfiguration);
            _context.SaveChanges();

        }

        public void Remove(int idDeviceConfiguration)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DeviceConfiguration> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        IEnumerable<DeviceConfiguration> IDeviceConfigurationService.GetAll()
        {
            return GetQuery();
        }

        public IEnumerable<DeviceConfiguration> GetAllDTO()
        {
            throw new NotImplementedException();
        }

        public int GetlastCode()
        {
            try
            {
                var configs = GetQuery().ToList();
                if (configs.Any())
                    return configs.LastOrDefault().Id + 1;

                return 1;
            }
            catch (Exception e)
            {
                return 1;
            }
        }

        List<SelectListItemDTO> IDeviceConfigurationService.GetQueryDropDownList(int IdMotor)
        {
            return GetQueryDropDownList(IdMotor);
        }

        List<SelectListItemDTO> IDeviceConfigurationService.GetQueryDropDownListByDeviceId(int deviceId)
        {
            return GetQueryDropDownListByDeviceId(deviceId);
        }
    }
}
