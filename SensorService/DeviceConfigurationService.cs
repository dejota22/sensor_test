using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
        private readonly IMotorService _motor;

        public DeviceConfigurationService(SensorContext context)
        {
            _context = context;

            _device = new DeviceService(context);
            _motor = new MotorService(context);
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

        public DeviceConfigurationModel GetLastBySensorCode(string sensorCode)
        {
            var config = new DeviceConfigurationModel();
            var device = _device.GetByCode(sensorCode);

            if (device != null)
            {
                var motorId = device.DeviceMotor?.MotorId;

                if (motorId != null)
                {
                    config = GetLast(device.Id, motorId.Value);

                    if (config != null)
                    {
                        var sectorId = device.DeviceMotor?.Motor.SectorId;

                        config.companyName = device.Company?.TradeName;
                        config.MotorId = motorId;
                        config.motorName = device.DeviceMotor?.Motor.Name;
                        config.DeviceId = device.Id;
                        config.deviceTag = device.Tag;

                        if (sectorId != null)
                        {
                            var sector = _context.CompanyUnitSector.Include(s => s.CompanyUnit)
                                .Include(s => s.ParentSector).FirstOrDefault(s => s.Id == sectorId);

                            config.unitName = sector.CompanyUnit?.Name;
                            config.sectorName = sector.ParentSectorId == null ? sector.Name : sector.ParentSector.Name;
                            config.subSectorName = sector.ParentSectorId == null ? "" : sector.Name;  
                        }
                            
                    }
                }
            }

            return config;
        }

        public DeviceConfigurationModel GetLast(int deviceId, int motorId)
        {
            var config = new DeviceConfigurationModel();
            var configs = GetQuery().Where(c => c.MotorId == motorId && c.DeviceId == deviceId)
                .AsNoTracking().ToList();
            if (configs.Any())
            {
                config = config.GetModelFromEntity(configs.LastOrDefault());
                config.isEdit = true;
                config.DeviceConfigurationHorariosEnviosCard = GetHoras(config.Id);
            }

            IQueryable<DeviceConfigurationSpecialRead> tb_deviceConfigurationSpecialRead = _context.DeviceConfigurationSpecialRead;
            var usrSetup = tb_deviceConfigurationSpecialRead.Where(c => c.MotorId == motorId && c.DeviceId == deviceId).FirstOrDefault();
            if (usrSetup != null)
            {
                config.usr_amostras = usrSetup.usr_amostras;
                config.usr_eixo = usrSetup.usr_eixo;
                config.usr_filtro = usrSetup.usr_filtro;
                config.usr_freq_cut = usrSetup.usr_freq_cut;
                config.usr_fs = usrSetup.usr_fs;
                config.usr_odr = usrSetup.usr_odr;
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
        public int Insert(DeviceConfiguration deviceConfiguration, DeviceConfigurationSpecialRead usrSetup)
        {
            if (usrSetup != null)
            {
                deviceConfiguration = new DeviceConfiguration()
                {
                    DeviceId = usrSetup.DeviceId,
                    MotorId = usrSetup.MotorId,
                };

                _context.DeviceConfigurationSpecialRead.Add(usrSetup);
            }

            deviceConfiguration.CreatedAt = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
            deviceConfiguration.config = true;

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

            return tb_device_horas.Where(d => d.DeviceConfigurationId == deviceConfigId).AsNoTracking().ToList();
        }

        public void Edit(DeviceConfiguration deviceConfiguration, DeviceConfigurationSpecialRead usrSetup)
        {
            if (usrSetup == null)
            {
                var contextEntityHours = _context.DeviceConfigurationHorariosEnviosCard.Where(h => h.DeviceConfigurationId == deviceConfiguration.Id)
                .AsNoTracking().ToList();

                foreach (var hr in contextEntityHours)
                {
                    if (deviceConfiguration.DeviceConfigurationHorariosEnviosCard.Any(h => h.Id == hr.Id) == false)
                    {
                        _context.DeviceConfigurationHorariosEnviosCard.Remove(hr);
                        _context.SaveChanges();
                    }
                }

                _context.DeviceConfiguration.Update(deviceConfiguration);
            }
            else
            {
                _context.DeviceConfigurationSpecialRead.Update(usrSetup);
            }

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

        public IDictionary<string, decimal?> GetLimitesAccSpd(int deviceId, int motorId, int reportType)
        {
            var result = GetQuery().Where(d => d.MotorId == motorId && d.DeviceId == deviceId).FirstOrDefault();
            if (result != null && reportType != 3)
            {
                Dictionary<string, decimal?> results = new Dictionary<string, decimal?>();
                if (reportType == 1)
                {
                    results.Add("red", result.rms_acc_red);
                    results.Add("yel", result.rms_acc_yel);
                }
                else if (reportType == 2)
                {
                    results.Add("red", result.rms_spd_red);
                    results.Add("yel", result.rms_spd_yel);
                }

                return results;
            }
            

            return null;
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
