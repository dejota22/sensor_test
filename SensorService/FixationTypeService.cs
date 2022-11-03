using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.DTO;
using Core.Service;

namespace SensorService
{
    public class FixationTypeService : IFixationTypeService
    {
        private readonly SensorContext _context;

        public FixationTypeService(SensorContext context)
        {
            _context = context;
        }

        void IFixationTypeService.Edit(FixationType FixationType)
        {
            _context.Update(FixationType);
            _context.SaveChanges();
        }

        private IQueryable<FixationTypeDTO> GetQueryDTO()
        {
            IQueryable<FixationType> tb_FixationType = _context.FixationType;

            var query = from FixationType in tb_FixationType
                        orderby FixationType.Id ascending

                        select new FixationTypeDTO
                        {
                            Id = FixationType.Id,
                            Name = FixationType.Name,
                            CreatedAt = FixationType.CreatedAt,
                            UpdatedAt = FixationType.UpdatedAt,
                        };

            return query;

            //var query = from FixationType in tb_FixationType
            //            select FixationType;
            //var query2 = _context.FixationType
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
            IQueryable<FixationType> tb_FixationType = _context.FixationType;
            var query = (from FixationType in tb_FixationType
                         select new SelectListItemDTO()
                         {
                             Key = FixationType.Id,
                             Value = FixationType.Name
                         }).Distinct().ToList();

            return query;
        }
        private IQueryable<FixationType> GetQuery()
        {
            IQueryable<FixationType> tb_FixationType = _context.FixationType;
            var query = from FixationType in tb_FixationType
                        select FixationType;

            //var query2 = _context.FixationType
            //       .Select(x => new Contact
            //       {
            //           Id = x.Contact.Id,
            //           FirstName = x.Contact.FirstName,
            //       });
            return query;

        }

        FixationType IFixationTypeService.Get(int idFixationType)
        {
            return GetQuery().Where(x => x.Id.Equals(idFixationType)).FirstOrDefault();
        }

        IEnumerable<FixationType> IFixationTypeService.GetAll()
        {
            return GetQuery();
        }

        IEnumerable<FixationTypeDTO> IFixationTypeService.GetAllDTO()
        {
            return GetQueryDTO();
        }

        IEnumerable<FixationType> IFixationTypeService.GetByName(string name)
        {
            return GetQuery().Where(x => x.Name.Equals(name));
        }

        int IFixationTypeService.Insert(FixationType FixationType)
        {
            FixationType.CreatedAt = DateTime.Now;
            FixationType.UpdatedAt = DateTime.Now;

            _context.Add(FixationType);
            _context.SaveChanges();
            return FixationType.Id;
        }

        void IFixationTypeService.Remove(int idFixationType)
        {
            var _FixationType = _context.FixationType.Find(idFixationType);
            _context.Remove(_FixationType);
            _context.SaveChanges();
        }

        int IFixationTypeService.GetlastCode()
        {
            return GetQuery().ToList().LastOrDefault().Id + 1;
        }

        List<SelectListItemDTO> IFixationTypeService.GetQueryDropDownList()
        {
            return GetQueryDropDownList();
        }
    }
}
