using Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Service
{
    public interface ICompanyAlertContactService
    {
        void Edit(CompanyAlertContact Contact);

        int Insert(CompanyAlertContact Contact);

        CompanyAlertContact Get(int idContact);

        IEnumerable<CompanyAlertContact> GetByCompany(int idCompany);
        IEnumerable<CompanyAlertContact> GetAll();

        void Remove(int idContact);
    }
}
