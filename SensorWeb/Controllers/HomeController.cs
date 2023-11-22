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
        IDeviceService _deviceService;
        IMotorService _motorService;
        IUserService _userService;
        ICompanyService _companyService;
        IReceiveService _receiveService;
        IMapper _mapper;
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<Resources.CommonResources> _localizer;

        public HomeController(IDeviceService deviceService, IMotorService motorService,
                                IUserService userService,
                                ICompanyService companyService,
                                IReceiveService receiveService,
                                IMapper mapper,
                                IStringLocalizer<Resources.CommonResources> localizer,
                                ILogger<HomeController> logger)
        {
            _deviceService = deviceService;
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
            var listaSensores = _deviceService.GetAll().ToList();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.Get(Convert.ToInt32(userId));
            var userCompany = user.Contact.CompanyId;
            var companies = _companyService.GetAll().Where(x => x.ParentCompanyId == userCompany).ToList();

            if (user.UserType.Name != Constants.Roles.Administrator)
            {
                listaSensores = listaSensores.Where(x => x.CompanyId == userCompany || companies.Any(y => y.Id == x.CompanyId)).ToList();
            }

            var deviceCodeAndAlarm = GetLastDeviceCodeAlarme();

            List<MotorModel> listaMotorModel = GetMotorModels(listaSensores, deviceCodeAndAlarm);

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

        public Dictionary<string,int> GetLastDeviceCodeAlarme()
        {
            var returnDictionary = new Dictionary<string, int>();
            var receiveDataAndGlobal = new List<DataAndGlobalModel>();

            var allData = _receiveService.ListDataLastAlarm();
            var allGlobal = _receiveService.ListGlobalLastAlarm();
            
            if (allData != null && allData.Any())
            {
                foreach (var data in allData)
                {
                    if (receiveDataAndGlobal.Any(rdg => rdg.id == data.id) == false)
                    {
                        receiveDataAndGlobal.Add(new DataAndGlobalModel() { id = data.id, alarm = data.alarme, dataReceive = data.DataReceive });
                    }
                }
            }
            if (allGlobal != null && allGlobal.Any())
            {
                foreach (var global in allGlobal)
                {
                    if (receiveDataAndGlobal.Any(rdg => rdg.id == global.id) == false)
                    {
                        receiveDataAndGlobal.Add(new DataAndGlobalModel() { id = global.id, alarm = global.alrm, dataReceive = global.DataReceive });
                    }
                }
            }

            receiveDataAndGlobal = receiveDataAndGlobal.OrderByDescending(rdg => rdg.dataReceive).ToList();
            foreach (var dataAndGlobal in receiveDataAndGlobal)
            {
                if (returnDictionary.Any(rd => rd.Key == dataAndGlobal.id) == false)
                {
                    returnDictionary.Add(dataAndGlobal.id, dataAndGlobal.alarm);
                }
            }
                
            return returnDictionary;
        }

        private List<MotorModel> GetMotorModels(List<Device> listaSensores, Dictionary<string, int> deviceCodeAndAlarm)
        {
            List<MotorModel> list = new List<MotorModel>();

            foreach(var d in listaSensores.Where(s => s.DeviceMotorId != null))
            {
                var modelName = "";
                if (d.DeviceMotor.Motor.GroupId != null)
                {
                    modelName = _motorService.Get(d.DeviceMotor.Motor.GroupId.Value).Name;
                }
                else
                {
                    modelName = d.DeviceMotor.Motor.Name;
                }

                MotorModel model = new MotorModel();

                model.Name = modelName;
                model.DeviceId = d.Id;
                model.Device = d;

                if (deviceCodeAndAlarm.Any(dca => dca.Key == d.Code))
                {
                    model.Alarm = deviceCodeAndAlarm[d.Code];
                }

                list.Add(model);
            }

            return list;
        }
    }

    public class DataAndGlobalModel
    {
        public string id;
        public int alarm;
        public DateTime dataReceive;

        public string motor;
        public string device;
    }
}
