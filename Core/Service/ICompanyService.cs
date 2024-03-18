using Core.DTO;
using System.Collections.Generic;

namespace Core.Service
{
    public interface ICompanyService
    {
        void Edit(Company Company);

        int Insert(Company Company);

        Company Get(int idCompany);

        void Remove(int idCompany);

        IEnumerable<Company> GetByName(string name);
        IEnumerable<Company> GetAll(bool full = false);

        IEnumerable<CompanyDTO> GetAllDTO();

        int GetlastCode();

        List<SelectListItemDTO> GetQueryDropDownList();
        List<SelectListItemDTO> GetQueryDropDownListStrict(string userId);

        string GetRelatedLocks(int idEmpresa);
        List<SelectListCustomItemDTO> GetQueryDropDownListStrictCustom(string userId);
    }
}
