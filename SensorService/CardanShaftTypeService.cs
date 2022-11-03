using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.DTO;
using Core.Service;

namespace SensorService
{
    public class CardanShaftTypeService : ICardanShaftTypeService
    {
        private readonly SensorContext _context;

        public CardanShaftTypeService(SensorContext context)
        {
            _context = context;
        }

        void ICardanShaftTypeService.Edit(CardanShaftType cardanShaftType)
        {
            _context.Update(cardanShaftType);
            _context.SaveChanges();
        }

        private IQueryable<CardanShaftTypeDTO> GetQueryDTO()
        {
            IQueryable<CardanShaftType> tb_CardanShaftType = _context.CardanShaftType;

            var query = from CardanShaftType in tb_CardanShaftType
                        orderby CardanShaftType.Id ascending

                        select new CardanShaftTypeDTO
                        {
                            Id = CardanShaftType.Id,
                            Name = CardanShaftType.Name,
                            CreatedAt = CardanShaftType.CreatedAt,
                            UpdatedAt = CardanShaftType.UpdatedAt,
                        };

            return query;

            //var query = from CardanShaftType in tb_CardanShaftType
            //            select CardanShaftType;
            //var query2 = _context.CardanShaftType
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
            IQueryable<CardanShaftType> tb_CardanShaftType = _context.CardanShaftType;
            var query = (from CardanShaftType in tb_CardanShaftType
                         select new SelectListItemDTO()
                         {
                             Key = CardanShaftType.Id,
                             Value = CardanShaftType.Name
                         }).Distinct().ToList();

            return query;
        }
        private IQueryable<CardanShaftType> GetQuery()
        {
            IQueryable<CardanShaftType> tb_CardanShaftType = _context.CardanShaftType;
            var query = from CardanShaftType in tb_CardanShaftType
                        select CardanShaftType;

            //var query2 = _context.CardanShaftType
            //       .Select(x => new Contact
            //       {
            //           Id = x.Contact.Id,
            //           FirstName = x.Contact.FirstName,
            //       });
            return query;

        }

        CardanShaftType ICardanShaftTypeService.Get(int idcardanShaftType)
        {
            return GetQuery().Where(x => x.Id.Equals(idcardanShaftType)).FirstOrDefault();
        }

        IEnumerable<CardanShaftType> ICardanShaftTypeService.GetAll()
        {
            return GetQuery();
        }

        IEnumerable<CardanShaftTypeDTO> ICardanShaftTypeService.GetAllDTO()
        {
            return GetQueryDTO();
        }

        IEnumerable<CardanShaftType> ICardanShaftTypeService.GetByName(string name)
        {
            return GetQuery().Where(x => x.Name.Equals(name));
        }

        int ICardanShaftTypeService.Insert(CardanShaftType cardanShaftType)
        {
            cardanShaftType.CreatedAt = DateTime.Now;
            cardanShaftType.UpdatedAt = DateTime.Now;

            _context.Add(cardanShaftType);
            _context.SaveChanges();
            return cardanShaftType.Id;
        }

        void ICardanShaftTypeService.Remove(int idcardanShaftType)
        {
            var _cardanShaftType = _context.CardanShaftType.Find(idcardanShaftType);
            _context.Remove(_cardanShaftType);
            _context.SaveChanges();
        }

        int ICardanShaftTypeService.GetlastCode()
        {
            return GetQuery().ToList().LastOrDefault().Id + 1;
        }

        List<SelectListItemDTO> ICardanShaftTypeService.GetQueryDropDownList()
        {
            return GetQueryDropDownList();
        }
    }
}
