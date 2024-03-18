using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.DTO;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace SensorService
{
    public class UserTypeService : IUserTypeService
    {
        private readonly SensorContext _context;

        public UserTypeService(SensorContext context)
        {
            _context = context;
        }

        void IUserTypeService.Edit(UserType userType)
        {
            _context.Update(userType);
            _context.SaveChanges();
        }

        private IQueryable<UserTypeDTO> GetQueryDTO()
        {
            IQueryable<UserType> tb_userType = _context.UserType;

            var query = from userType in tb_userType
                        orderby userType.Id ascending

                        select new UserTypeDTO
                        {
                            Id = userType.Id,
                            Name = userType.Name,
                            CreatedAt = userType.CreatedAt,
                            UpdatedAt = userType.UpdatedAt,
                        };

            return query;

            //var query = from userType in tb_userType
            //            select userType;
            //var query2 = _context.userType
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
            IQueryable<UserType> tb_userType = _context.UserType;
            var query = (from userType in tb_userType
                         select new SelectListItemDTO()
                         {
                             Key = userType.Id,
                             Value = userType.Name == "Administrador" ? "Superuser" :
                                userType.Name == "Sysadmin" ? "Administrador" : 
                                userType.Name == "Usuário" ? "Analista" : userType.Name,
                         }).Distinct().ToList();

            return query;
        }
        private IQueryable<UserType> GetQuery()
        {
            IQueryable<UserType> tb_userType = _context.UserType;
            var query = from userType in tb_userType
                        select userType;

            //var query2 = _context.userType
            //       .Select(x => new Contact
            //       {
            //           Id = x.Contact.Id,
            //           FirstName = x.Contact.FirstName,
            //       });
            return query;

        }

        UserType IUserTypeService.Get(int iduserType)
        {
            return GetQuery().Where(x => x.Id.Equals(iduserType)).FirstOrDefault();
        }

        IEnumerable<UserType> IUserTypeService.GetAll()
        {
            return GetQuery();
        }

        IEnumerable<UserTypeDTO> IUserTypeService.GetAllDTO()
        {
            return GetQueryDTO();
        }

        IEnumerable<UserType> IUserTypeService.GetByName(string name)
        {
            return GetQuery().Where(x => x.Name.Equals(name));
        }

        int IUserTypeService.Insert(UserType userType)
        {
            userType.CreatedAt = DateTime.Now;
            userType.UpdatedAt = DateTime.Now;

            _context.Add(userType);
            _context.SaveChanges();
            return userType.Id;
        }

        void IUserTypeService.Remove(int iduserType)
        {
            var _userType = _context.UserType.Find(iduserType);
            _context.Remove(_userType);
            _context.SaveChanges();
        }

        int IUserTypeService.GetlastCode()
        {
            return GetQuery().ToList().LastOrDefault().Id + 1;
        }

        List<SelectListItemDTO> IUserTypeService.GetQueryDropDownList()
        {
            return GetQueryDropDownList();
        }

        List<SelectListItemDTO> IUserTypeService.GetQueryDropDownListStrict(string userId)
        {
            var user = _context.User.Include(u => u.Contact).FirstOrDefault(u => u.Id == Convert.ToInt32(userId));

            IQueryable<UserType> tb_userType = _context.UserType;
            var query = tb_userType.ToList();

            if (user.UserTypeId == 4)
            {
                query = query.Where(t => t.Name == "Supervisor" || t.Name == "Usuário").ToList();
            }
            else if (user.UserTypeId == 3)
            {
                query = query.Where(t => t.Name == "Usuário").ToList();
            }

            var select = query.Select(t => new SelectListItemDTO()
            {
                Key = t.Id,
                Value = t.Name == "Administrador" ? "Superuser" :
                t.Name == "Sysadmin" ? "Administrador" :
                t.Name == "Usuário" ? "Analista" : t.Name,
            }).Distinct().ToList();

            return select;
        }
    }
}
