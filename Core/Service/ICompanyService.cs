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
        IEnumerable<Company> GetAll();

        IEnumerable<CompanyDTO> GetAllDTO();

        int GetlastCode();

        List<SelectListItemDTO> GetQueryDropDownList();
    }
}
