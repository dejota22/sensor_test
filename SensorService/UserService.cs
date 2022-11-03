using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.DTO;
using Core.Service;

namespace SensorService
{
  public  class UserService : IUserService
    {
        private readonly SensorContext _context;

        public UserService(SensorContext context)
        {
            _context = context;
        }

        void IUserService.Edit(User user)
        {
            //user.Contact = new Contact
            //{
            //    FirstName = user.Contact.FirstName,
            //    Email = user.Email,
            //    CompanyId = user.Contact.CompanyId,
            //    IsActive = 1,
            //    CreatedAt = user.CreatedAt,
            //    UpdatedAt = user.UpdatedAt,
            //    Surname = user.Contact.Surname,
            //    Cpf = user.Contact.Cpf,
            //    Rg = user.Contact.Rg,
            //    Id = user.Contact.Id
            //};
            // user.IsActive = 1;

            //user.UserTypeId = user.UserTypeId;

            user.UpdatedAt = DateTime.Now;

            _context.Update(user);
            _context.SaveChanges();
        }

        private IQueryable<UserDTO> GetQueryDTO()
        {
            IQueryable<User> tb_user = _context.User;
            //var query = from user in tb_user
            //            select user;

            var query = from user in tb_user
                        where user.Contact.Id.Equals(user.ContactId)
                        orderby user.Contact.FirstName descending

                        select new UserDTO
                        {
                            Id = user.Id,
                            Email = user.Email,
                            IsActive = user.IsActive,
                            CreatedAt = user.CreatedAt,
                            UpdatedAt = user.UpdatedAt,

                            Contact = new ContactDTO()
                            {
                                FirstName = user.Contact.FirstName
                            }
                        };

            //var query2 = _context.User
            //       .Select(x => new Contact
            //       {
            //           Id = x.Contact.Id,
            //           FirstName = x.Contact.FirstName,
            //       });
            return query;

        }

        private IQueryable<User> GetQuery()
        {
            IQueryable<User> tb_user = _context.User;
            //var query = from user in tb_user
            //            select user;

            var query = from user in tb_user
                        where user.Contact.Id.Equals(user.ContactId)
                        orderby user.Contact.FirstName descending

                        select new User
                        {
                            Id = user.Id,
                            Email = user.Email,
                            IsActive = user.IsActive,
                            CreatedAt = user.CreatedAt,
                            UpdatedAt = user.UpdatedAt,
                            Password = user.Password,
                            UserTypeId = user.UserTypeId,
                            ContactId = user.ContactId,

                            Contact = new Contact()
                            {
                                FirstName = user.Contact.FirstName,
                                CompanyId = user.Contact.CompanyId,
                                CreatedAt = user.Contact.CreatedAt,
                                UpdatedAt = user.Contact.UpdatedAt,
                                Surname = user.Contact.Surname,
                                Cpf = user.Contact.Cpf,
                                Rg = user.Contact.Rg,
                                Email = user.Contact.Email,
                                IsActive = user.Contact.IsActive,
                                Id = user.Contact.Id
                            }
                        };

            //var query2 = _context.User
            //       .Select(x => new Contact
            //       {
            //           Id = x.Contact.Id,
            //           FirstName = x.Contact.FirstName,
            //       });
            return query;

            //IQueryable<User> tb_user = _context.User;
            //var query = from user in tb_user
            //            select user;

            //var query2 = _context.User
            //       .Select(x => new Contact
            //       {
            //           Id = x.Contact.Id,
            //           FirstName = x.Contact.FirstName,
            //       });
        }

        User IUserService.Get(int idUser)
        {
            return GetQuery().Where(x => x.Id.Equals(idUser)).FirstOrDefault();
        }

        User IUserService.Login(string login,string pass)
        {
            return GetQuery().Where(x => x.Email.Equals(login) &&  x.Password.Equals(pass)).FirstOrDefault();
        }

        IEnumerable<User> IUserService.GetAll()
        {
            return GetQuery();
        }

        IEnumerable<UserDTO> IUserService.GetAllDTO()
        {
            return GetQueryDTO();
        }

        IEnumerable<UserDTO> IUserService.GetbyDescendenceName()
        {
            IQueryable<User> tb_user = _context.User;

            var query = from user in tb_user
                        where user.Contact.FirstName.StartsWith(user.Contact.FirstName)
                        orderby user.Contact.FirstName descending

                        select new UserDTO
                        {
                            Contact = new ContactDTO()
                            {
                                FirstName = user.Contact.FirstName
                            }
                        };

            return query;
        }

        IEnumerable<User> IUserService.GetByName(string name)
        {
            return GetQuery().Where(x => x.Contact.FirstName.Equals(name));
        }

        int IUserService.Insert(User user)
        {
            user.Contact = new Contact
            {
                FirstName = user.Contact.FirstName,                
                Email = user.Email,
                CompanyId = user.Contact.CompanyId,
                IsActive = 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Surname = user.Contact.Surname,
                Cpf = user.Contact.Cpf,
                Rg = user.Contact.Rg
            };

            user.UserTypeId = user.UserTypeId;
            user.IsActive = 1;
            
            _context.Add(user);
            _context.SaveChanges();
            return user.Id;
        }

        void IUserService.Remove(int iduser)
        {
            var _user = _context.User.Find(iduser);
            _context.Remove(_user);
            _context.SaveChanges();
        }

        /// <summary>
        /// GetQueryDropDownList
        /// </summary>
        /// <returns></returns>
        private List<SelectListItemDTO> GetQueryDropDownList()
        {
            IQueryable<User> tb_user = _context.User;
            var query = (from User in tb_user
                         where User.Contact.Id.Equals(User.ContactId)
                         select new SelectListItemDTO()
                         {
                             Key = User.Id,
                             Value = String.Concat(User.Contact.FirstName ," ", User.Contact.Surname)
                         }).Distinct().ToList();

            return query;
        }

        List<SelectListItemDTO> IUserService.GetQueryDropDownList()
        {
            return GetQueryDropDownList();
        }
    }
}
