using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.DTO;
using Core.Service;

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
                         select new SelectListItemDTO()
                         {
                             Key = Motor.Id,
                             Value = Motor.Name
                         }).Distinct().ToList();

            return query;
        }

        /// <summary>
        /// GetQueryDropDownList
        /// </summary>
        /// <returns></returns>
        private List<SelectListItemDTO> GetQueryDropDownListByTag()
        {
            IQueryable<Motor> tb_Motor = _context.Motor;
            var query = (from Motor in tb_Motor
                         where Motor.Device.Id.Equals(Motor.DeviceId)
                         select new SelectListItemDTO()
                         {
                             Key = Motor.Id,
                             Value = String.Concat(Motor.Tag, " - ",  Motor.Name, " - Sensor: ", Motor.Device.Code)
                         }).Distinct().ToList();

            return query;
        }

        private IQueryable<Motor> GetQuery()
        {
            IQueryable<Motor> tb_Motor = _context.Motor;
            IQueryable<Device> tb_Device = _context.Device;

            var results = from Motor in tb_Motor
                        join Device in tb_Device on Motor.DeviceId equals Device.Id
                        select new { Motor, Device };

            foreach(var result in results)
            {
                result.Motor.Device = result.Device;
            }

            return results.Select(x => x.Motor);

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
