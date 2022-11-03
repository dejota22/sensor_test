using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.DTO;
using Core.Service;

namespace SensorService
{
    public class PumpTypeService : IPumpTypeService
    {
        private readonly SensorContext _context;

        public PumpTypeService(SensorContext context)
        {
            _context = context;
        }

        void IPumpTypeService.Edit(PumpType pumpType)
        {
            _context.Update(pumpType);
            _context.SaveChanges();
        }

        private IQueryable<PumpTypeDTO> GetQueryDTO()
        {
            IQueryable<PumpType> tb_PumpType = _context.PumpType;

            var query = from PumpType in tb_PumpType
                        orderby PumpType.Id ascending

                        select new PumpTypeDTO
                        {
                            Id = PumpType.Id,
                            Name = PumpType.Name,
                            CreatedAt = PumpType.CreatedAt,
                            UpdatedAt = PumpType.UpdatedAt,
                        };

            return query;

            //var query = from PumpType in tb_PumpType
            //            select PumpType;
            //var query2 = _context.PumpType
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
            IQueryable<PumpType> tb_PumpType = _context.PumpType;
            var query = (from PumpType in tb_PumpType
                         select new SelectListItemDTO()
                         {
                             Key = PumpType.Id,
                             Value = PumpType.Name
                         }).Distinct().ToList();

            return query;
        }
        private IQueryable<PumpType> GetQuery()
        {
            IQueryable<PumpType> tb_PumpType = _context.PumpType;
            var query = from PumpType in tb_PumpType
                        select PumpType;

            //var query2 = _context.PumpType
            //       .Select(x => new Contact
            //       {
            //           Id = x.Contact.Id,
            //           FirstName = x.Contact.FirstName,
            //       });
            return query;

        }

        PumpType IPumpTypeService.Get(int idpumpType)
        {
            return GetQuery().Where(x => x.Id.Equals(idpumpType)).FirstOrDefault();
        }

        IEnumerable<PumpType> IPumpTypeService.GetAll()
        {
            return GetQuery();
        }

        IEnumerable<PumpTypeDTO> IPumpTypeService.GetAllDTO()
        {
            return GetQueryDTO();
        }

        IEnumerable<PumpType> IPumpTypeService.GetByName(string name)
        {
            return GetQuery().Where(x => x.Name.Equals(name));
        }

        int IPumpTypeService.Insert(PumpType pumpType)
        {
            pumpType.CreatedAt = DateTime.Now;
            pumpType.UpdatedAt = DateTime.Now;

            _context.Add(pumpType);
            _context.SaveChanges();
            return pumpType.Id;
        }

        void IPumpTypeService.Remove(int idpumpType)
        {
            var _pumpType = _context.PumpType.Find(idpumpType);
            _context.Remove(_pumpType);
            _context.SaveChanges();
        }

        int IPumpTypeService.GetlastCode()
        {
            return GetQuery().ToList().LastOrDefault().Id + 1;
        }

        List<SelectListItemDTO> IPumpTypeService.GetQueryDropDownList()
        {
            return GetQueryDropDownList();
        }
    }
}
