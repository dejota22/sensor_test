using Core.DTO;
using System.Collections.Generic;

namespace Core.Service
{
    public interface ICompanyUserService
    {
        void Edit(CompanyUser companyUser);

        int Insert(CompanyUser companyUser);

        CompanyUser Get(int idcardanShaftType);

        void Remove(int idcardanShaftType);

        IEnumerable<CompanyUser> GetByName(string name);
        IEnumerable<CompanyUser> GetAll();

        IEnumerable<CardanShaftTypeDTO> GetAllDTO();

        int GetlastCode();

        List<SelectListItemDTO> GetQueryDropDownList();
    }
}
