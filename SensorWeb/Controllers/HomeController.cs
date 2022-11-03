using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SensorWeb.Models;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace SensorWeb.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<Resources.CommonResources> _localizer;

        public HomeController(ILogger<HomeController> logger, IStringLocalizer<Resources.CommonResources> localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult SetLanguage(string culture, string returnUrl = "Index")
        {
            var cultureInfo = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            CookieOptions option = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1)
            };

            Response.Cookies.Append("cultureLang", culture, option);

            //Response.Cookies.Append(
            //    CookieRequestCultureProvider.DefaultCookieName,
            //    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            //    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

           Console.WriteLine("The new CultureInfo is now: " + CultureInfo.CurrentCulture);
            return Redirect(returnUrl);
        }
    }
}
