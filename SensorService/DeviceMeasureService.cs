using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.DTO;
using Core.Service;

namespace SensorService
{
    public class DeviceMeasureService : IDeviceMeasureService
    {
        private readonly SensorContext _context;
        private readonly IDeviceService _device;

        public DeviceMeasureService(SensorContext context)
        {
            _context = context;
        }

        //void IUserTypeService.Edit(UserType userType)
        //{
        //    _context.Update(userType);
        //    _context.SaveChanges();
        //}

        private IQueryable<DeviceMeasureDTO> GetQueryDTO(int IdMotor)
        {
            IQueryable<DeviceMeasure> tb_userType = _context.DeviceMeasure;

            var query = from register in tb_userType                        
                        where register.MotorId == IdMotor
                        orderby register.Id ascending

                        select new DeviceMeasureDTO
                        {                            
                            Id = register.Id,
                            MotorId = register.Id,
                            Frequency = register.Frequency,
                            Axies = register.Axies,
                            Battery = register.Battery,
                            Bdr = register.Bdr,
                            Cutoff = register.Cutoff,
                            Hours = register.Hours,
                            Lines = register.Lines,
                            Lpf = register.Lpf,
                            Rms = register.Rms,
                            RmsMax = register.RmsMax,
                            RmsMin = register.RmsMin,
                            Temperature = register.Temperature,
                            VelocityMin = register.VelocityMin,
                            VelocityMax = register.VelocityMax,
                            AccelerationMin = register.AccelerationMin,
                            AccelerationMax = register.AccelerationMax,
                            CrestFactorMin = register.CrestFactorMin,
                            CrestFactorMax = register.CrestFactorMax,
                            Sent = register.Sent,
                            CreatedAt = register.CreatedAt                            
                        };            

            return query;

            //var query = from userType in tb_userType
            //            select userType;
            //var query2 = _context.userType
            //       .Select(x => new Contact
            //       {
            //           Id = x.Contact.Id,
            //           FirstName = x.Contact.FirstName,
            //       });

        }

        /// <summary>
        /// GetQueryDropDownList
        /// </summary>
        /// <returns></returns>
        private List<SelectListItemDTO> GetQueryDropDownList()
        {
            IQueryable<DeviceMeasure> tb_userType = _context.DeviceMeasure;
            var query = (from deviceMeasure in tb_userType
                         select new SelectListItemDTO()
                         {
                             Key = deviceMeasure.Id,
                             Value = deviceMeasure.CreatedAt.ToString("dd/MM/yyyy H:mm:ss") + " " + deviceMeasure.Id 
                         }).Distinct().ToList();

            return query;
        }

        /// <summary>
        /// GetQueryDropDownList
        /// </summary>
        /// <returns></returns>
        private List<SelectListItemDTO> GetQueryDropDownList(int IdMotor)
        {
            IQueryable<DeviceMeasure> tb_userType = _context.DeviceMeasure;
            var query = (from deviceMeasure in tb_userType
                         where deviceMeasure.MotorId == IdMotor
                         select new SelectListItemDTO()
                         {
                             Key = deviceMeasure.Id,
                             Value = deviceMeasure.CreatedAt.ToString("dd/MM/yyyy H:mm:ss") + " " + deviceMeasure.Id
                         }).Distinct().ToList();

            return query;
        }

        /// <summary>
        /// GetQueryDropDownList
        /// </summary>
        /// <returns></returns>
        private List<SelectListItemDTO> GetQueryDropDownListByDeviceId(int deviceId)
        {
            IQueryable<DeviceMeasure> tb_userType = _context.DeviceMeasure;
            var query = (from deviceMeasure in tb_userType
                         where deviceMeasure.DeviceId == deviceId
                         select new SelectListItemDTO()
                         {
                             Key = deviceMeasure.Id,
                             Value = deviceMeasure.CreatedAt.ToString("dd/MM/yyyy H:mm:ss") + " - " + deviceMeasure.Id
                         }).Distinct().ToList();

            return query;
        }

        private IQueryable<DeviceMeasure> GetQuery()
        {
            IQueryable<DeviceMeasure> tb_deviceMeasure = _context.DeviceMeasure;
            var query = from deviceMeasure in tb_deviceMeasure
                        select deviceMeasure;

            //var query2 = _context.userType
            //       .Select(x => new Contact
            //       {
            //           Id = x.Contact.Id,
            //           FirstName = x.Contact.FirstName,
            //       });
            return query;

        }

        public DeviceMeasure GetByDeviceId(int deviceId)
        {
            return GetQuery().Where(x => x.DeviceId.Equals(deviceId)).FirstOrDefault();
        }

        public DeviceMeasure Get(int id)
        {
            return GetQuery().Where(x => x.Id.Equals(id)).FirstOrDefault();
        }

        //DeviceMeasure IDeviceMeasureService.GetByReadDataType(string readData)
        //{
        //    return GetQuery().Where(x => x.ReadDataType.Equals(readData)).FirstOrDefault();
        //}

        //UserType IUserTypeService.Get(int iduserType)
        //{
        //    return GetQuery().Where(x => x.Id.Equals(iduserType)).FirstOrDefault();
        //}



        //IEnumerable<UserTypeDTO> IUserTypeService.GetAllDTO()
        //{
        //    return GetQueryDTO();
        //}

        IEnumerable<DeviceMeasureDTO> IDeviceMeasureService.GetByMotorid(int motorId)
        {
            return GetQueryDTO(motorId);
        }
        public int Insert(DeviceMeasure deviceMeasure)
        {
            deviceMeasure.CreatedAt = DateTime.Now;

            _context.Add(deviceMeasure);
            _context.SaveChanges();
            return deviceMeasure.Id;
        }

        //void IUserTypeService.Remove(int iduserType)
        //{
        //    var _userType = _context.UserType.Find(iduserType);
        //    _context.Remove(_userType);
        //    _context.SaveChanges();
        //}

        //int IUserTypeService.GetlastCode()
        //{
        //    return GetQuery().ToList().LastOrDefault().Id + 1;
        //}

        List<SelectListItemDTO> IDeviceMeasureService.GetQueryDropDownList()
        {
            return GetQueryDropDownList();
        }

        public void Edit(DeviceMeasure deviceMeasure)
        {
            throw new NotImplementedException();
        }

        public void Remove(int idDeviceMeasure)
        {
            throw new NotImplementedException();
        }

        

        public IEnumerable<DeviceMeasure> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        IEnumerable<DeviceMeasure> IDeviceMeasureService.GetAll()
        {
            return GetQuery();
        }

        public IEnumerable<DeviceMeasureDTO> GetAllDTO()
        {
            throw new NotImplementedException();
        }

        public int GetlastCode()
        {
                        try
            {
                return GetQuery().ToList().LastOrDefault().Id + 1;
            }
            catch (Exception e)
            {
                return 1;
            }
        }

        List<SelectListItemDTO> IDeviceMeasureService.GetQueryDropDownList(int IdMotor)
        {
            return GetQueryDropDownList(IdMotor);
        }

        List<SelectListItemDTO> IDeviceMeasureService.GetQueryDropDownListByDeviceId(int deviceId)
        {
            return GetQueryDropDownListByDeviceId(deviceId);
        }
    }
}
