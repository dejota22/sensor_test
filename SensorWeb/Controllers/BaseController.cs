using Core;
using Core.DTO;
using Core.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SensorWeb.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;

namespace SensorWeb.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(
           ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            string cookieValueFromReq = Request.Cookies["cultureLang"];

            if (string.IsNullOrEmpty(cookieValueFromReq))
                cookieValueFromReq = "pt";

            var cultureInfo = CultureInfo.GetCultureInfo(cookieValueFromReq);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        public string LoggedUserId
        {
            get
            {
                return User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
        }

        internal IEnumerable<Company> FilterCompaniesByUser(User user, IEnumerable<Company> listaCompany)
        {
            ViewBag.AdminInDistribuidor = false;
            if (user.UserType.Name == Constants.Roles.Sysadmin)
            {
                var userCompany = listaCompany.FirstOrDefault(c => c.Id == user.Contact.CompanyId);
                if (userCompany != null && userCompany.CompanyTypeId == 2)
                {
                    listaCompany = listaCompany.Where(c => c.Id == user.Contact.CompanyId || c.ParentCompanyId == userCompany.Id);

                    ViewBag.AdminInDistribuidor = true;
                }
            }

            if (user.UserType.Name != Constants.Roles.Administrator && user.UserType.Name != Constants.Roles.Sysadmin)
            {
                listaCompany = listaCompany.Where(x => x.Id == user.Contact.CompanyId);
            }

            return listaCompany;
        }

        internal IEnumerable<CompanyUnit> FilterUnitsByUser(User user, IEnumerable<CompanyUnit> listaUnit, IEnumerable<Company> listaCompanies,
            IEnumerable<Contact> supContacts)
        {
            var userCompany = listaCompanies.FirstOrDefault(c => c.Id == user.Contact.CompanyId);

            if (userCompany != null && userCompany.CompanyTypeId == 2)
            {
                var companies = listaCompanies.Where(x => x.ParentCompanyId == userCompany.Id 
                    && x.CompanyTypeId == 3).ToList();
                if (user.UserType.Name == Constants.Roles.Supervisor || user.UserType.Name == Constants.Roles.User
                    || user.UserType.Name == Constants.Roles.Sysadmin)
                {
                    listaUnit = listaUnit.Where(x => x.CompanyId == user.Contact.CompanyId || companies.Any(y => y.Id == x.CompanyId));
                }
            }
            else if (userCompany != null && userCompany.CompanyTypeId == 3)
            {
                if (user.UserType.Name == Constants.Roles.Supervisor)
                {
                    listaUnit = listaUnit.Where(x => x.CompanyId == user.Contact.CompanyId
                        || (x.Company.ParentCompanyId == user.Contact.CompanyId && x.Company.CompanyTypeId == 3));
                }
                else if (user.UserType.Name == Constants.Roles.Sysadmin)
                {
                    var supCompanies = listaCompanies.Where(x => 
                        x.ParentCompany.Contact.Any(c => c.Company.CompanyTypeId == 3 && supContacts.Any(sc => sc.Id == c.Id))).ToList();
                    listaUnit = listaUnit.Where(x => x.CompanyId == user.Contact.CompanyId || supCompanies.Any(y => y.Id == x.CompanyId));
                }
            }

            return listaUnit;
        }

        internal List<Motor> FilterMotorsByUser(User user, IEnumerable<Motor> listaMotor, IEnumerable<Company> listaCompanies)
        {
            var userCompany = user.Contact.CompanyId;

            if (user.UserType.Name == Constants.Roles.Supervisor || user.UserType.Name == Constants.Roles.User)
            {
                listaMotor = listaMotor.Where(x => x.CompanyId == userCompany).ToList();
            }
            else if (user.UserType.Name == Constants.Roles.Sysadmin)
            {
                listaMotor = listaMotor.Where(x => x.CompanyId == userCompany || listaCompanies.Any(y => y.Id == x.CompanyId)).ToList();
            }

            return listaMotor.ToList();
        }

        internal List<Device> FilterDevicesByUser(User user, IEnumerable<Device> listaDevice, IEnumerable<Company> listaCompanies)
        {
            var userCompany = user.Contact.CompanyId;

            if (user.UserType.Name == Constants.Roles.Supervisor || user.UserType.Name == Constants.Roles.User)
            {
                listaDevice = listaDevice.Where(x => x.CompanyId == userCompany).ToList();
            }
            else if (user.UserType.Name == Constants.Roles.Sysadmin)
            {
                listaDevice = listaDevice.Where(x => x.CompanyId == userCompany || listaCompanies.Any(y => y.Id == x.CompanyId)).ToList();
            }

            return listaDevice.ToList();
        }
    }
}
