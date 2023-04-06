using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;
using System.Threading;

namespace SensorWeb.Controllers
{
    public class BaseController : Controller
    {
        //public  IStringLocalizer<Resources.CommonResources> _localizer;

        //public BaseController(IStringLocalizer<Resources.CommonResources> localizer)
        //{
        //    _localizer = localizer;
        //}

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

            //if (cookieValueFromReq == "pt")
            //    ViewData["selected-lang"] = "Potuguês";

            //if (cookieValueFromReq == "en")
            //    ViewData["selected-lang"] = "English";

            //if (cookieValueFromReq == "es")
            //    ViewData["selected-lang"] = "Spanish";
        }
    }
}
