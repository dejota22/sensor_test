using Core.DTO;
using System.Collections.Generic;

namespace Core.Service
{
    public interface ICompanyUnitService
    {
        void Edit(CompanyUnit CompanyUnit);

        int Insert(CompanyUnit CompanyUnit);

        int InsertSector(CompanyUnitSector CompanyUnitSector);

        CompanyUnit Get(int idCompanyUnit);

        void Remove(int idCompanyUnit);

        IEnumerable<CompanyUnit> GetByName(string name);
        IEnumerable<CompanyUnit> GetAll();

        IEnumerable<CompanyUnitSector> GetSectorByName(string name);

        IEnumerable<CompanyUnitDTO> GetAllDTO();

        int GetlastCode();

        List<SelectListItemDTO> GetQueryDropDownList();

        void RemoveSector(int idCompanyUnitSector);
    }
}
