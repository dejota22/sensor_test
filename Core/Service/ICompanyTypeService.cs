using Core.DTO;
using System.Collections.Generic;

namespace Core.Service
{
    public interface ICompanyTypeService
    {
        void Edit(CompanyType companyType);

        int Insert(CompanyType companyType);

        CompanyType Get(int iduserType);

        void Remove(int iduserType);

        IEnumerable<CompanyType> GetByName(string name);
        IEnumerable<CompanyType> GetAll();

        IEnumerable<CompanyTypeDTO> GetAllDTO();

        int GetlastCode();

        List<SelectListItemDTO> GetQueryDropDownList();
    }
}
