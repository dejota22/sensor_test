using Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Globalization;
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
    }
}
