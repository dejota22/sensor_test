using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.DTO;
using Core.Service;

namespace SensorService
{
    public class CouplingTypeService : ICouplingTypeService
    {
        private readonly SensorContext _context;

        public CouplingTypeService(SensorContext context)
        {
            _context = context;
        }

        void ICouplingTypeService.Edit(CouplingType couplingType)
        {
            _context.Update(couplingType);
            _context.SaveChanges();
        }

        private IQueryable<CouplingTypeDTO> GetQueryDTO()
        {
            IQueryable<CouplingType> tb_CouplingType = _context.CouplingType;

            var query = from CouplingType in tb_CouplingType
                        orderby CouplingType.Id ascending

                        select new CouplingTypeDTO
                        {
                            Id = CouplingType.Id,
                            Name = CouplingType.Name,
                            CreatedAt = CouplingType.CreatedAt,
                            UpdatedAt = CouplingType.UpdatedAt,
                        };

            return query;

            //var query = from CouplingType in tb_CouplingType
            //            select CouplingType;
            //var query2 = _context.CouplingType
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
            IQueryable<CouplingType> tb_CouplingType = _context.CouplingType;
            var query = (from CouplingType in tb_CouplingType
                         select new SelectListItemDTO()
                         {
                             Key = CouplingType.Id,
                             Value = CouplingType.Name
                         }).Distinct().ToList();

            return query;
        }
        private IQueryable<CouplingType> GetQuery()
        {
            IQueryable<CouplingType> tb_CouplingType = _context.CouplingType;
            var query = from CouplingType in tb_CouplingType
                        select CouplingType;

            //var query2 = _context.CouplingType
            //       .Select(x => new Contact
            //       {
            //           Id = x.Contact.Id,
            //           FirstName = x.Contact.FirstName,
            //       });
            return query;

        }

        CouplingType ICouplingTypeService.Get(int idCouplingType)
        {
            return GetQuery().Where(x => x.Id.Equals(idCouplingType)).FirstOrDefault();
        }

        IEnumerable<CouplingType> ICouplingTypeService.GetAll()
        {
            return GetQuery();
        }

        IEnumerable<CouplingTypeDTO> ICouplingTypeService.GetAllDTO()
        {
            return GetQueryDTO();
        }

        IEnumerable<CouplingType> ICouplingTypeService.GetByName(string name)
        {
            return GetQuery().Where(x => x.Name.Equals(name));
        }

        int ICouplingTypeService.Insert(CouplingType CouplingType)
        {
            CouplingType.CreatedAt = DateTime.Now;
            CouplingType.UpdatedAt = DateTime.Now;

            _context.Add(CouplingType);
            _context.SaveChanges();
            return CouplingType.Id;
        }

        void ICouplingTypeService.Remove(int idCouplingType)
        {
            var _CouplingType = _context.CouplingType.Find(idCouplingType);
            _context.Remove(_CouplingType);
            _context.SaveChanges();
        }

        int ICouplingTypeService.GetlastCode()
        {
            return GetQuery().ToList().LastOrDefault().Id + 1;
        }

        List<SelectListItemDTO> ICouplingTypeService.GetQueryDropDownList()
        {
            return GetQueryDropDownList();
        }
    }
}
