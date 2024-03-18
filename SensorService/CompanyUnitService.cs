using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.DTO;
using Core.Service;
using Core.Utils;
using Microsoft.EntityFrameworkCore;

namespace SensorService
{
    public class CompanyUnitService : ICompanyUnitService
    {
        private readonly SensorContext _context;

        public CompanyUnitService(SensorContext context)
        {
            _context = context;
        }

        void ICompanyUnitService.Edit(CompanyUnit CompanyUnit)
        {
            CompanyUnit.UpdatedAt = DateTime.Now;
            _context.Update(CompanyUnit);
            _context.SaveChanges();
        }

        private IQueryable<CompanyUnitDTO> GetQueryDTO()
        {
            IQueryable<CompanyUnit> tb_Company = _context.CompanyUnit;

            var query = from Company in tb_Company
                        orderby Company.Id ascending

                        select new CompanyUnitDTO
                        {
                            Id = Company.Id,
                            Name = Company.Name,
                            CompanyId = Company.CompanyId,
                            Company = Company.Company
                        };

            return query;
        }

        private IQueryable<CompanyUnit> GetQuery()
        {
            IQueryable<CompanyUnit> tb_Company = _context.CompanyUnit.Include(c => c.Company)
                .ThenInclude(c => c.ParentCompanyCol);
            var query = from CompanyUnit in tb_Company
                        select CompanyUnit;

            return query;
        }

        private IQueryable<CompanyUnit> GetQueryFull()
        {
            IQueryable<CompanyUnit> tb_Company = _context.CompanyUnit
                .Include(c => c.Company).Include(c => c.CompanyUnitSector).ThenInclude(s => s.SubSectors);
            var query = from CompanyUnit in tb_Company
                        select CompanyUnit;

            return query;
        }

        private IQueryable<CompanyUnitSector> GetSectorQuery()
        {
            IQueryable<CompanyUnitSector> tb_Company = _context.CompanyUnitSector
                .Include(s => s.CompanyUnit).Include(s => s.SubSectors).Include(s => s.ParentSector);
            var query = from CompanyUnitSector in tb_Company
                        select CompanyUnitSector;

            return query;
        }

        CompanyUnit ICompanyUnitService.Get(int idCompanyUnit)
        {
            return GetQueryFull().Where(x => x.Id.Equals(idCompanyUnit)).FirstOrDefault();
        }

        IEnumerable<CompanyUnit> ICompanyUnitService.GetAll()
        {
            return GetQuery();
        }

        IEnumerable<CompanyUnit> ICompanyUnitService.GetAllFull()
        {
            return GetQueryFull();
        }

        IEnumerable<CompanyUnitDTO> ICompanyUnitService.GetAllDTO()
        {
            return GetQueryDTO();
        }

        IEnumerable<CompanyUnit> ICompanyUnitService.GetByName(string name)
        {
            return GetQuery().Where(x => x.Name.Equals(name));
        }

        IEnumerable<CompanyUnitSector> ICompanyUnitService.GetSectorByName(string name)
        {
            return GetSectorQuery().Where(x => x.Name.Equals(name));
        }

        CompanyUnitSector ICompanyUnitService.GetSector(int id)
        {
            return GetSectorQuery().FirstOrDefault(x => x.Id.Equals(id));
        }

        int ICompanyUnitService.Insert(CompanyUnit CompanyUnit)
        {
            CompanyUnit.CreatedAt = DateTime.Now;
            CompanyUnit.UpdatedAt = DateTime.Now;

            _context.Add(CompanyUnit);
            _context.SaveChanges();
            return CompanyUnit.Id;
        }

        int ICompanyUnitService.InsertSector(CompanyUnitSector CompanyUnitSector)
        {
            CompanyUnitSector.CreatedAt = DateTime.Now;
            CompanyUnitSector.UpdatedAt = DateTime.Now;

            _context.Add(CompanyUnitSector);
            _context.SaveChanges();
            return CompanyUnitSector.Id;
        }

        void ICompanyUnitService.Remove(int idUnit)
        {
            var _Unit = _context.CompanyUnit.Find(idUnit);
            _context.Remove(_Unit);
            _context.SaveChanges();
        }

        void ICompanyUnitService.RemoveSector(int idSector)
        {
            var _Sector = _context.CompanyUnitSector.Find(idSector);
            _context.Remove(_Sector);
            _context.SaveChanges();
        }

        int ICompanyUnitService.GetlastCode()
        {
            return GetQuery().ToList().LastOrDefault().Id + 1;
        }

        List<SelectListCustomItemDTO> ICompanyUnitService.GetQueryDropDownList(string userId)
        {
            var user = _context.User.Include(u => u.Contact).Include(u => u.UserType)
                .FirstOrDefault(u => u.Id == Convert.ToInt32(userId));
            var userCompany = user.Contact.CompanyId;
            var companies = _context.Company.Where(x => x.ParentCompanyId == userCompany).ToList();

            var tb_Company = _context.CompanyUnit.ToList();
            if (user.UserType.Name == Constants.Roles.Supervisor || user.UserType.Name == Constants.Roles.User)
            {
                tb_Company = tb_Company.Where(x => x.CompanyId == user.Contact.CompanyId).ToList();
            }
            else if (user.UserType.Name == Constants.Roles.Sysadmin)
            {
                tb_Company = tb_Company.Where(x => x.CompanyId == user.Contact.CompanyId || companies.Any(y => y.Id == x.CompanyId)).ToList();
            }

            var query = tb_Company.Select(c => new SelectListCustomItemDTO()
                         {
                             Key = c.Id,
                             Value = c.Name,
                             CompanyId = c.CompanyId
                         }).Distinct().ToList();

            return query;
        }

        List<SelectListCustomItemDTO> ICompanyUnitService.GetQueryDropDownListSector(string userId)
        {
            var user = _context.User.Include(u => u.Contact).Include(u => u.UserType)
                .FirstOrDefault(u => u.Id == Convert.ToInt32(userId));
            var userCompany = user.Contact.CompanyId;
            var companies = _context.Company.Where(x => x.ParentCompanyId == userCompany).ToList();

            var tb_Sector = _context.CompanyUnitSector.Include(s => s.CompanyUnit)
                .Where(s => s.ParentSectorId == null).ToList();

            if (user.UserType.Name == Constants.Roles.Supervisor || user.UserType.Name == Constants.Roles.User)
            {
                tb_Sector = tb_Sector.Where(x => x.CompanyUnit.CompanyId == user.Contact.CompanyId).ToList();
            }
            else if (user.UserType.Name == Constants.Roles.Sysadmin)
            {
                tb_Sector = tb_Sector.Where(x => x.CompanyUnit.CompanyId == user.Contact.CompanyId || companies.Any(y => y.Id == x.CompanyUnit.CompanyId)).ToList();
            }

            var query = tb_Sector.Select(s => new SelectListCustomItemDTO()
                         {
                             Key = s.Id,
                             Value = s.Name,
                             Unit = s.CompanyUnitId,
                             CompanyId = s.CompanyUnit.CompanyId
                         }).Distinct().ToList();

            return query;
        }

        List<SelectListCustomItemDTO> ICompanyUnitService.GetQueryDropDownListSubSector(string userId)
        {
            var user = _context.User.Include(u => u.Contact).Include(u => u.UserType)
                .FirstOrDefault(u => u.Id == Convert.ToInt32(userId));
            var userCompany = user.Contact.CompanyId;
            var companies = _context.Company.Where(x => x.ParentCompanyId == userCompany).ToList();

            var tb_Sector = _context.CompanyUnitSector.Include(s => s.CompanyUnit)
                .Where(s => s.ParentSectorId != null).ToList();

            if (user.UserType.Name == Constants.Roles.Supervisor || user.UserType.Name == Constants.Roles.User)
            {
                tb_Sector = tb_Sector.Where(x => x.CompanyUnit.CompanyId == user.Contact.CompanyId).ToList();
            }
            else if (user.UserType.Name == Constants.Roles.Sysadmin)
            {
                tb_Sector = tb_Sector.Where(x => x.CompanyUnit.CompanyId == user.Contact.CompanyId || companies.Any(y => y.Id == x.CompanyUnit.CompanyId)).ToList();
            }

            var query = tb_Sector.Select(s => new SelectListCustomItemDTO()
                        {
                             Key = s.Id,
                             Value = s.Name,
                             Unit = s.CompanyUnitId,
                             Sector = s.ParentSectorId,
                             CompanyId = s.CompanyUnit.CompanyId
                         }).Distinct().ToList();

            return query;
        }
    }
}
