﻿using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.DTO;
using Core.Service;

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
            Device.UpdatedAt = DateTime.Now;

            _context.Update(Device);
            _context.SaveChanges();
        }



        private IQueryable<DeviceDTO> GetQueryDTO()
        {
            IQueryable<Device> tb_Device = _context.Device;

            var query = from Device in tb_Device
                        orderby Device.Id ascending

                        select new DeviceDTO
                        {
                            Id = Device.Id,
                            Code = Device.Code,
                            Tag = Device.Tag,
                            CreatedAt = Device.CreatedAt,
                            UpdatedAt = Device.UpdatedAt,
                        };

            return query;

            //var query = from Device in tb_Device
            //            select Device;
            //var query2 = _context.Device
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
        private List<SelectListItemDTO> GetQueryDropDownListByTag()
        {
            return null;
            //IQueryable<Device> tb_Device = _context.Device;
            //var query = (from Device in tb_Device
            //             select new SelectListItemDTO()
            //             {
            //                 Key = Device.Id,
            //                 Value = String.Concat(Device.Tag, " - ",  Device.Name)
            //             }).Distinct().ToList();

            //return query;
        }

        private IQueryable<Device> GetQuery()
        {
            IQueryable<Device> tb_Device = _context.Device;
            var query = from Device in tb_Device
                        where Device.Company.Id.Equals(Device.CompanyId)
                        select new Device
                        {
                            Code = Device.Code,
                            CompanyId = Device.CompanyId,
                            Tag = Device.Tag,
                            Dados = Device.Dados,
                            Compressor = Device.Compressor,
                            CreatedAt = Device.CreatedAt,
                            DeletedAt =  Device.DeletedAt,
                            ExhaustFan = Device.ExhaustFan,
                            Pump = Device.Pump,
                            Id = Device.Id,
                            Reducer = Device.Reducer,
                            UpdatedAt = Device.UpdatedAt,

                            Company = new Company()
                            {
                                LegalName = Device.Company.LegalName,
                                TradeName = Device.Company.TradeName,
                                Cnpj = Device.Company.Cnpj                                
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


        List<SelectListItemDTO> IDeviceService.GetQueryDropDownListByTag()
        {
            return GetQueryDropDownListByTag();
        }

        Device IDeviceService.GetByDeviceTag(string DeviceTag)
        {
            return GetQuery().Where(x => x.Tag.Equals(DeviceTag)).FirstOrDefault();
        }
    }
}
