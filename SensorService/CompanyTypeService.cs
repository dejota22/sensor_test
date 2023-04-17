using System.Collections.Generic;
using Core.DTO;
using Core.Service;
using Core.Utils;

namespace SensorService
{
    public class CompanyTypeService : ICompanyTypeService
    {
        List<SelectListItemDTO> ICompanyTypeService.GetQueryDropDownList(string companyType)
        {
            var list = new List<SelectListItemDTO>();

            if (companyType == Constants.CompanyTypes.Administrator)
            {
                list.Add(new SelectListItemDTO() { Key = 1, Value = "Administrador" });
                list.Add(new SelectListItemDTO() { Key = 2, Value = "Distribuidor" });
                list.Add(new SelectListItemDTO() { Key = 3, Value = "Consumidor Final" });
            }
            else if (companyType == Constants.CompanyTypes.Distribuidor)
            {
                list.Add(new SelectListItemDTO() { Key = 3, Value = "Consumidor Final" });
            }

            return list;
        }
    }
}
