using AutoMapper;
using Core;
using Core.DTO;
using Core.Service;
using Core.Utils;
using Microsoft.AspNetCore.Authorization;
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
using System.Security.Claims;
using System.Threading;

namespace SensorWeb.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        IMotorService _motorService;
        IUserService _userService;
        ICompanyService _companyService;
        IReceiveService _receiveService;
        IMapper _mapper;
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<Resources.CommonResources> _localizer;

        public HomeController(IMotorService motorService,
                                IUserService userService,
                                ICompanyService companyService,
                                IReceiveService receiveService,
                                IMapper mapper,
                                IStringLocalizer<Resources.CommonResources> localizer,
                                ILogger<HomeController> logger)
        {
            _motorService = motorService;
            _userService = userService;
            _companyService = companyService;
            _receiveService = receiveService;
            _mapper = mapper;
            _localizer = localizer;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var listaMotores = _motorService.GetAll().ToList();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.Get(Convert.ToInt32(userId));
            var userCompany = user.Contact.CompanyId;
            var companies = _companyService.GetAll().Where(x => x.ParentCompanyId == userCompany).ToList();

            if (user.UserType.Name != Constants.Roles.Administrator)
            {
                listaMotores = listaMotores.Where(x => x.CompanyId.Value == userCompany || companies.Any(y => y.Id == x.CompanyId.Value)).ToList();
            }

            var deviceCodeAndAlarm = GetLastDeviceCodeAlarme();

            List<MotorModel> listaMotorModel = GetMotorModels(listaMotores, deviceCodeAndAlarm);

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

        public KeyValuePair<string,int> GetLastDeviceCodeAlarme()
        {
            ReceiveData receiveData = null;
            ReceiveGlobal receiveGlobal = null;

            var allData = _receiveService.GetAllData().ToList();
            if (allData != null && allData.Any())
            {
                receiveData = allData.OrderByDescending(d => d.DataReceive).FirstOrDefault();
            }

            var allGlobal = _receiveService.GetAllGlobal().ToList();
            if (allGlobal != null && allGlobal.Any())
            {
                receiveGlobal = allGlobal.OrderByDescending(d => d.DataReceive).FirstOrDefault();
            }

            var deviceCode = string.Empty;
            var alarm = 0;
            if (receiveData != null && receiveGlobal != null)
            {
                deviceCode = receiveData.DataReceive > receiveGlobal.DataReceive ? receiveData.id : receiveGlobal.id;
                alarm = receiveData.DataReceive > receiveGlobal.DataReceive ? receiveData.alarme : receiveGlobal.alrm;
            }
            else if (receiveData != null)
            {
                deviceCode = receiveData.id;
                alarm = receiveData.alarme;
            }
                
            else if (receiveGlobal != null)
            {
                deviceCode = receiveGlobal.id;
                alarm = receiveGlobal.alrm;
            }
                
            return new KeyValuePair<string,int>(deviceCode,alarm);
        }

        private List<MotorModel> GetMotorModels(List<Motor> listaMotores, KeyValuePair<string, int> deviceCodeAndAlarm)
        {
            List<MotorModel> list = new List<MotorModel>();

            foreach(var m in listaMotores)
            {
                MotorModel model = new MotorModel();

                model.Name = m.Name;
                model.DeviceId = m.DeviceId;
                model.Device = m.Device;

                if (m.Device.Code == deviceCodeAndAlarm.Key)
                {
                    model.Alarm = deviceCodeAndAlarm.Value;
                }

                list.Add(model);
            }

            return list;
        }
    }
}
