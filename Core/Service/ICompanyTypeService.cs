using Core.DTO;
using System.Collections.Generic;

namespace Core.Service
{
    public interface ICompanyTypeService
    {
        List<SelectListItemDTO> GetQueryDropDownList(string companyType);
    }
}
