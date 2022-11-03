using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.DTO;
using Core.Service;

namespace SensorService
{
    public class MachineService : IMachineService
    {
        private readonly SensorContext _context;

        public MachineService(SensorContext context)
        {
            _context = context;
        }

        void IMachineService.Edit(Machine Machine)
        {
            _context.Update(Machine);
            _context.SaveChanges();
        }

        private IQueryable<MachineDTO> GetQueryDTO()
        {
            IQueryable<Machine> tb_Machine = _context.Machine;

            var query = from Machine in tb_Machine
                        orderby Machine.Id ascending

                        select new MachineDTO
                        {
                            Id = Machine.Id,
                            Name = Machine.Name,
                            CreatedAt = Machine.CreatedAt,
                            UpdatedAt = Machine.UpdatedAt,
                        };

            return query;

            //var query = from Machine in tb_Machine
            //            select Machine;
            //var query2 = _context.Machine
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
            IQueryable<Machine> tb_Machine = _context.Machine;
            var query = (from Machine in tb_Machine
                         select new SelectListItemDTO()
                         {
                             Key = Machine.Id,
                             Value = Machine.Name
                         }).Distinct().ToList();

            return query;
        }
        private IQueryable<Machine> GetQuery()
        {
            IQueryable<Machine> tb_Machine = _context.Machine;
            var query = from Machine in tb_Machine
                        select Machine;

            //var query2 = _context.Machine
            //       .Select(x => new Contact
            //       {
            //           Id = x.Contact.Id,
            //           FirstName = x.Contact.FirstName,
            //       });
            return query;

        }

        Machine IMachineService.Get(int idMachine)
        {
            return GetQuery().Where(x => x.Id.Equals(idMachine)).FirstOrDefault();
        }

        IEnumerable<Machine> IMachineService.GetAll()
        {
            return GetQuery();
        }

        IEnumerable<MachineDTO> IMachineService.GetAllDTO()
        {
            return GetQueryDTO();
        }

        IEnumerable<Machine> IMachineService.GetByName(string name)
        {
            return GetQuery().Where(x => x.Name.Equals(name));
        }

        int IMachineService.Insert(Machine Machine)
        {
            Machine.CreatedAt = DateTime.Now;
            Machine.UpdatedAt = DateTime.Now;

            _context.Add(Machine);
            _context.SaveChanges();
            return Machine.Id;
        }

        void IMachineService.Remove(int idMachine)
        {
            var _Machine = _context.Machine.Find(idMachine);
            _context.Remove(_Machine);
            _context.SaveChanges();
        }

        int IMachineService.GetlastCode()
        {
            return GetQuery().ToList().LastOrDefault().Id + 1;
        }

        List<SelectListItemDTO> IMachineService.GetQueryDropDownList()
        {
            return GetQueryDropDownList();
        }
    }
}
