using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.DTO;
using Core.Service;

namespace SensorService
{
    public class CompressorTypeService : ICompressorTypeService
    {
        private readonly SensorContext _context;

        public CompressorTypeService(SensorContext context)
        {
            _context = context;
        }

        void ICompressorTypeService.Edit(CompressorType compressorType)
        {
            _context.Update(compressorType);
            _context.SaveChanges();
        }

        private IQueryable<CompressorTypeDTO> GetQueryDTO()
        {
            IQueryable<CompressorType> tb_compressorType = _context.CompressorType;

            var query = from compressorType in tb_compressorType
                        orderby compressorType.Id ascending

                        select new CompressorTypeDTO
                        {
                            Id = compressorType.Id,
                            Name = compressorType.Name,
                            CreatedAt = compressorType.CreatedAt,
                            UpdatedAt = compressorType.UpdatedAt,
                        };

            return query;

            //var query = from compressorType in tb_compressorType
            //            select compressorType;
            //var query2 = _context.compressorType
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
            IQueryable<CompressorType> tb_compressorType = _context.CompressorType;
            var query = (from compressorType in tb_compressorType
                         select new SelectListItemDTO()
                         {
                             Key = compressorType.Id,
                             Value = compressorType.Name
                         }).Distinct().ToList();

            return query;
        }
        private IQueryable<CompressorType> GetQuery()
        {
            IQueryable<CompressorType> tb_compressorType = _context.CompressorType;
            var query = from compressorType in tb_compressorType
                        select compressorType;

            //var query2 = _context.compressorType
            //       .Select(x => new Contact
            //       {
            //           Id = x.Contact.Id,
            //           FirstName = x.Contact.FirstName,
            //       });
            return query;

        }

        CompressorType ICompressorTypeService.Get(int idcompressorType)
        {
            return GetQuery().Where(x => x.Id.Equals(idcompressorType)).FirstOrDefault();
        }

        IEnumerable<CompressorType> ICompressorTypeService.GetAll()
        {
            return GetQuery();
        }

        IEnumerable<CompressorTypeDTO> ICompressorTypeService.GetAllDTO()
        {
            return GetQueryDTO();
        }

        IEnumerable<CompressorType> ICompressorTypeService.GetByName(string name)
        {
            return GetQuery().Where(x => x.Name.Equals(name));
        }

        int ICompressorTypeService.Insert(CompressorType compressorType)
        {
            compressorType.CreatedAt = DateTime.Now;
            compressorType.UpdatedAt = DateTime.Now;

            _context.Add(compressorType);
            _context.SaveChanges();
            return compressorType.Id;
        }

        void ICompressorTypeService.Remove(int idcompressorType)
        {
            var _compressorType = _context.CompressorType.Find(idcompressorType);
            _context.Remove(_compressorType);
            _context.SaveChanges();
        }

        int ICompressorTypeService.GetlastCode()
        {
            return GetQuery().ToList().LastOrDefault().Id + 1;
        }

        List<SelectListItemDTO> ICompressorTypeService.GetQueryDropDownList()
        {
            return GetQueryDropDownList();
        }
    }
}
