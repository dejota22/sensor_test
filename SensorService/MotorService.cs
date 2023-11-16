using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.DTO;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace SensorService
{
    public class MotorService : IMotorService
    {
        private readonly SensorContext _context;

        public MotorService(SensorContext context)
        {
            _context = context;
        }

        void IMotorService.Edit(Motor Motor)
        {
            Motor.UpdatedAt = DateTime.Now;

            _context.Update(Motor);
            _context.SaveChanges();
        }



        private IQueryable<MotorDTO> GetQueryDTO()
        {
            IQueryable<Motor> tb_Motor = _context.Motor;

            var query = from Motor in tb_Motor
                        orderby Motor.Id ascending

                        select new MotorDTO
                        {
                            Id = Motor.Id,
                            Name = Motor.Name,
                            CreatedAt = Motor.CreatedAt,
                            UpdatedAt = Motor.UpdatedAt,
                        };

            return query;

            //var query = from Motor in tb_Motor
            //            select Motor;
            //var query2 = _context.Motor
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
            IQueryable<Motor> tb_Motor = _context.Motor;
            var query = (from Motor in tb_Motor
                         where Motor.Name != null || Motor.Tag != null
                         select new SelectListItemDTO()
                         {
                             Key = Motor.Id,
                             Value = String.Concat(Motor.Tag, " - ", Motor.Name)
                         }).Distinct().ToList();

            return query;
        }

        /// <summary>
        /// GetQueryDropDownList
        /// </summary>
        /// <returns></returns>
        private List<SelectListItemDTO> GetQueryDropDownListByTag()
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

        private IQueryable<Motor> GetQuery()
        {
            IQueryable<Motor> tb_Motor = _context.Motor;
            var query = tb_Motor.Include(d => d.MotorDevices);

            return query;
        }

        Motor IMotorService.Get(int idMotor)
        {
            return GetQuery().Where(x => x.Id.Equals(idMotor)).FirstOrDefault();
        }

        IEnumerable<Motor> IMotorService.GetAll()
        {
            return GetQuery();
        }

        IEnumerable<MotorDTO> IMotorService.GetAllDTO()
        {
            return GetQueryDTO();
        }

        IEnumerable<Motor> IMotorService.GetByName(string name)
        {
            return GetQuery().Where(x => x.Name.Equals(name));
        }

        int IMotorService.Insert(Motor Motor)
        {
            Motor.CreatedAt = DateTime.Now;
            Motor.UpdatedAt = DateTime.Now;

            _context.Add(Motor);
            _context.SaveChanges();
            return Motor.Id;
        }

        void IMotorService.Remove(int idMotor)
        {
            var _Motor = _context.Motor.Find(idMotor);
            _context.Remove(_Motor);
            _context.SaveChanges();
        }

        int IMotorService.GetlastCode()
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

        List<SelectListItemDTO> IMotorService.GetQueryDropDownList()
        {
            return GetQueryDropDownList();
        }


        List<SelectListItemDTO> IMotorService.GetQueryDropDownListByTag()
        {
            return GetQueryDropDownListByTag();
        }

        Motor IMotorService.GetBymotorTag(string motorTag)
        {
            return GetQuery().Where(x => x.Tag.Equals(motorTag)).FirstOrDefault();
        }
    }
}
