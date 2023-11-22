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

        

        #region Equipamento e Agrupamento

        private IQueryable<Motor> GetQuery()
        {
            IQueryable<Motor> tb_Motor = _context.Motor;
            var query = tb_Motor.Include(d => d.MotorDevices).Include(d => d.Motors);

            return query;
        }

        private IQueryable<MotorDTO> GetQueryDTO()
        {
            IQueryable<Motor> tb_Motor = _context.Motor;

            var query = from Motor in tb_Motor
                        orderby Motor.Id ascending

                        select new MotorDTO
                        {
                            Id = Motor.Id,
                            GroupId = Motor.GroupId,
                            Name = Motor.Name,
                            CreatedAt = Motor.CreatedAt,
                            UpdatedAt = Motor.UpdatedAt,
                            IsGrouping = Motor.IsGrouping
                        };

            return query;
        }

        IEnumerable<Motor> IMotorService.GetAll()
        {
            return GetQuery().Where(m => m.GroupId == null);
        }

        IEnumerable<MotorDTO> IMotorService.GetAllDTO()
        {
            return GetQueryDTO().Where(m => m.GroupId == null);
        }

        Motor IMotorService.Get(int idMotor)
        {
            return GetQuery().Where(x => x.Id.Equals(idMotor)).FirstOrDefault();
        }

        IEnumerable<Motor> IMotorService.GetByName(string name)
        {
            return GetQuery().Where(x => x.Name.Equals(name));
        }

        List<SelectListItemDTO> IMotorService.GetQueryDropDownList()
        {
            IQueryable<Motor> tb_Motor = _context.Motor;
            var query = (from Motor in tb_Motor
                         where Motor.GroupId == null 
                         && (Motor.Name != null || Motor.Tag != null)
                         select new SelectListItemDTO()
                         {
                             Key = Motor.Id,
                             Value = String.Concat(Motor.Tag, " - ", Motor.Name)
                         }).Distinct().ToList();

            return query;
        }

        List<SelectListItemDTO> IMotorService.GetQueryDropDownListByTag()
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

        int IMotorService.Insert(Motor Motor)
        {
            Motor.CreatedAt = DateTime.Now;
            Motor.UpdatedAt = DateTime.Now;

            _context.Add(Motor);
            _context.SaveChanges();
            return Motor.Id;
        }

        void IMotorService.Edit(Motor Motor)
        {
            Motor.UpdatedAt = DateTime.Now;

            _context.Update(Motor);
            _context.SaveChanges();
        }

        void IMotorService.Remove(int idMotor)
        {
            var _Motor = _context.Motor.Find(idMotor);
            _context.Remove(_Motor);
            _context.SaveChanges();
        }

        #endregion

        #region Equipamento

        IEnumerable<Motor> IMotorService.GetAllEquipamento()
        {
            return GetQuery().Where(e => e.IsGrouping == false);
        }

        List<SelectListItemDTO> IMotorService.GetQueryDropDownListEquipamento()
        {
            IQueryable<Motor> tb_Motor = _context.Motor;
            var query = (from Motor in tb_Motor
                         where Motor.GroupId == null && Motor.IsGrouping == false
                         && (Motor.Name != null || Motor.Tag != null)
                         select new SelectListItemDTO()
                         {
                             Key = Motor.Id,
                             Value = String.Concat(Motor.Tag, " - ", Motor.Name)
                         }).Distinct().ToList();

            return query;
        }

        #endregion

        #region Agrupamento

        IEnumerable<Motor> IMotorService.GetAllAgrupamento()
        {
            return GetQuery().Where(e => e.IsGrouping == true);
        }

        #endregion
    }
}
