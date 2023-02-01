using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SensorService;
using SensorWeb.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace SensorWeb.Controllers
{
    public class HomeController : BaseController
    {
        IMotorService _motorService;
        IMapper _mapper;
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<Resources.CommonResources> _localizer;

        public HomeController(IMotorService motorService,
                                IMapper mapper,
                                IStringLocalizer<Resources.CommonResources> localizer,
                                ILogger<HomeController> logger)
        {
            _motorService = motorService;
            _mapper = mapper;
            _localizer = localizer;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var listaMotores = _motorService.GetAll();
            var listaMotorModel = _mapper.Map<List<MotorModel>>(listaMotores);

            return View(listaMotorModel.OrderBy(x => x.Id));
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
