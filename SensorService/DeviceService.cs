﻿using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.DTO;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace SensorService
{
    public class DeviceService : IDeviceService
    {
        private readonly SensorContext _context;

        public DeviceService(SensorContext context)
        {
            _context = context;
        }

        void IDeviceService.Edit(Device Device)
        {
            var baseDevice = _context.Device.Find(Device.Id);

            baseDevice.UpdatedAt = DateTime.Now;
            baseDevice.Tag = Device.Tag;
            baseDevice.Code = Device.Code;
            baseDevice.CompanyId = Device.CompanyId;
            baseDevice.DeviceMotorId = Device.DeviceMotor?.Id;
            baseDevice.DeviceMotor = Device.DeviceMotor;
            baseDevice.DeviceMotorMaxChanges = Device.DeviceMotorMaxChanges;

            _context.Update(baseDevice);
            _context.SaveChanges();
        }

        private IQueryable<DeviceDTO> GetQueryDTO()
        {
            IQueryable<Device> tb_Device = _context.Device;

            var query = from Device in tb_Device
                        orderby Device.Id descending

                        select new DeviceDTO
                        {
                            Id = Device.Id,
                            Code = Device.Code,
                            Tag = Device.Tag,
                            CreatedAt = Device.CreatedAt,
                            UpdatedAt = Device.UpdatedAt,
                            Frequency = Device.Frequency,
                            Axies = Device.Axies,
                            Battery = Device.Battery,
                            Bdr = Device.Bdr,
                            Cutoff = Device.Cutoff,
                            Hours = Device.Hours,
                            Lines = Device.Lines,
                            Lpf = Device.Lpf,
                            Rms = Device.Rms,
                            RmsMax = Device.RmsMax,
                            RmsMin = Device.RmsMin,
                            Temperature = Device.Temperature,
                            VelocityMin = Device.VelocityMin,
                            VelocityMax = Device.VelocityMax,
                            AccelerationMin = Device.AccelerationMin,
                            AccelerationMax = Device.AccelerationMax,
                            CrestFactorMin = Device.CrestFactorMin,
                            CrestFactorMax = Device.CrestFactorMax
                        };

            return query;

        }


        /// <summary>
        /// GetQueryDropDownList
        /// </summary>
        /// <returns></returns>
        private List<SelectListItemDTO> GetQueryDropDownList()
        {
            IQueryable<Device> tb_Device = _context.Device;
            var query = (from Device in tb_Device
                         select new SelectListItemDTO()
                         {
                             Key = Device.Id,
                             Value = Device.Tag
                         }).Distinct().ToList();

            return query;
        }

        /// <summary>
        /// GetQueryDropDownList
        /// </summary>
        /// <returns></returns>
        private List<SelectListItemDTO> GetQueryDropDownListByCode()
        {
            IQueryable<Device> tb_Device = _context.Device;
            var query = (from Device in tb_Device
                         select new SelectListItemDTO()
                         {
                             Key = Device.Id,
                             Value = String.Concat(Device.Code, " - ", Device.Tag)
                         }).Distinct().ToList();

            return query;
        }

        public List<SelectListItemDTO> GetQueryDropDownListByTag()
        {
            IQueryable<Device> tb_Device = _context.Device;
            var query = (from Device in tb_Device
                         where Device.DeviceMotor.Id.Equals(Device.DeviceMotorId)
                         select new SelectListItemDTO()
                         {
                             Key = Device.Id,
                             Value = String.Concat(Device.DeviceMotor.Motor.Tag, " - ", Device.DeviceMotor.Motor.Name, " - Sensor: ", Device.Code)
                         }).Distinct().ToList();

            return query;
        }

        private IQueryable<Device> GetQuery()
        {
            IQueryable<Device> tb_Device = _context.Device;
            var query = from Device in tb_Device.Include(d => d.DeviceMotor).ThenInclude(d => d.Motor)
                            .ThenInclude(m => m.Sector)
                        where Device.Company.Id.Equals(Device.CompanyId)
                        select new Device
                        {
                            Code = Device.Code,
                            CompanyId = Device.CompanyId,
                            Tag = Device.DeviceMotorId == null || Device.DeviceMotor.Motor.GroupId == null ? Device.Tag
                                : $"{Device.DeviceMotor.Motor.Name} - {Device.Tag}",
                            Dados = Device.Dados,
                            Compressor = Device.Compressor,
                            CreatedAt = Device.CreatedAt,
                            DeletedAt = Device.DeletedAt,
                            ExhaustFan = Device.ExhaustFan,
                            Pump = Device.Pump,
                            Id = Device.Id,
                            Reducer = Device.Reducer,
                            UpdatedAt = Device.UpdatedAt,
                            Temperature = Device.Temperature,
                            RmsMin = Device.RmsMin,
                            RmsMax = Device.RmsMax,
                            Rms = Device.Rms,
                            Lpf = Device.Lpf,
                            Lines = Device.Lines,
                            Hours = Device.Hours,
                            Cutoff = Device.Cutoff,
                            Bdr = Device.Bdr,
                            Axies = Device.Axies,
                            Battery = Device.Battery,
                            Frequency = Device.Frequency,
                            VelocityMin = Device.VelocityMin,
                            VelocityMax = Device.VelocityMax,
                            AccelerationMin = Device.AccelerationMin,
                            AccelerationMax = Device.AccelerationMax,
                            CrestFactorMax = Device.CrestFactorMax,
                            CrestFactorMin = Device.CrestFactorMin,
                            DeviceMotorMaxChanges = Device.DeviceMotorMaxChanges,
                            DeviceMotorId = Device.DeviceMotorId,
                            DeviceMotor = Device.DeviceMotorId != null ? new DeviceMotor()
                            {
                                Id = Device.DeviceMotor.Id,
                                MotorId = Device.DeviceMotor.MotorId,
                                MeasurementPlan = Device.DeviceMotor.MeasurementPlan,
                                SensorOrientation = Device.DeviceMotor.SensorOrientation,
                                Motor = Device.DeviceMotor.Motor
                                //Motor = Device.DeviceMotor.MotorId != 0 ? new Motor()
                                //{
                                //    Id = Device.DeviceMotor.Motor.Id,
                                //    Name = Device.DeviceMotor.Motor.Name,
                                //    CompanyId = Device.DeviceMotor.Motor.CompanyId,
                                //    GroupId = Device.DeviceMotor.Motor.GroupId,
                                //} : null
                            } : null,
                            Company = new Company()
                            {
                                LegalName = Device.Company.LegalName,
                                TradeName = Device.Company.TradeName,
                                Cnpj = Device.Company.Cnpj,
                                CompanyTypeId = Device.Company.CompanyTypeId,
                                DeviceMotorMaxChanges = Device.Company.DeviceMotorMaxChanges
                            }
                        };

            //var query2 = _context.Device
            //       .Select(x => new Contact
            //       {
            //           Id = x.Contact.Id,
            //           FirstName = x.Contact.FirstName,
            //       });
            return query;

        }

        Device IDeviceService.Get(int idDevice)
        {
            return GetQuery().Where(x => x.Id.Equals(idDevice)).FirstOrDefault();
        }

        Device IDeviceService.GetByCode(string code)
        {
            return GetQuery().Where(x => x.Code.Equals(code)).FirstOrDefault();
        }

        IEnumerable<Device> IDeviceService.GetAll()
        {
            return GetQuery();
        }

        IEnumerable<DeviceDTO> IDeviceService.GetAllDTO()
        {
            return GetQueryDTO();
        }

        IEnumerable<Device> IDeviceService.GetByName(string name)
        {
            return null;
            //return GetQuery().Where(x => x.Name.Equals(name));
        }

        int IDeviceService.Insert(Device Device)
        {
            Device.CreatedAt = DateTime.Now;
            Device.UpdatedAt = DateTime.Now;

            _context.Add(Device);
            _context.SaveChanges();
            return Device.Id;
        }

        void IDeviceService.Remove(int idDevice)
        {
            var _Device = _context.Device.Find(idDevice);
            _context.Remove(_Device);
            _context.SaveChanges();
        }

        int IDeviceService.GetlastCode()
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

        List<SelectListItemDTO> IDeviceService.GetQueryDropDownList()
        {
            return GetQueryDropDownList();
        }


        List<SelectListItemDTO> IDeviceService.GetQueryDropDownListByCode()
        {
            return GetQueryDropDownListByCode();
        }

        Device IDeviceService.GetByDeviceTag(string DeviceTag)
        {
            return GetQuery().Where(x => x.Tag.Equals(DeviceTag)).FirstOrDefault();
        }
    }
}
