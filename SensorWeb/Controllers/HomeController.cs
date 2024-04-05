using AutoMapper;
using Core;
using Core.DTO;
using Core.Service;
using Core.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SensorService;
using SensorWeb.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
        ICompanyUnitService _unitService;
        IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<Resources.CommonResources> _localizer;

        public HomeController(IDeviceService deviceService, IMotorService motorService,
                                IUserService userService,
                                ICompanyService companyService,
                                IReceiveService receiveService,
                                ICompanyUnitService unitService,
                                IMapper mapper,
                                IMemoryCache cache,
                                IStringLocalizer<Resources.CommonResources> localizer,
                                ILogger<HomeController> logger)
        {
            _deviceService = deviceService;
            _motorService = motorService;
            _userService = userService;
            _companyService = companyService;
            _receiveService = receiveService;
            _unitService = unitService;
            _mapper = mapper;
            _cache = cache;
            _localizer = localizer;
            _logger = logger;
        }

        public IActionResult Index(int idCompany)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var companies = _companyService.GetAll();
            var user = _userService.Get(Convert.ToInt32(userId));

            if (user.UserType.Name != Constants.Roles.Administrator)
            {
                var userCompany = user.Contact.CompanyId;
                companies = companies.Where(x => x.Id == user.Contact.CompanyId || (x.ParentCompanyId == userCompany && x.CompanyTypeId == 3)).ToList();
            }
            else
                companies = companies.ToList();

            ViewBag.UserCompanies = companies;

            var allMotor = _cache.GetOrCreate($"UserMotorCache{userId}_{idCompany}", entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                entry.SetPriority(CacheItemPriority.High);

                var listaEquips = _motorService.GetAllEquipamento();

                return FilterMotorsByUser(user, listaEquips, companies);
            });

            if (idCompany != 0)
            {
                allMotor = allMotor.Where(x => x.CompanyId == idCompany).ToList();
            }

            var listaUnits = allMotor.Where(m => m.SectorId != null && m.MotorDevices.Any())
                .Select(m => new HomeDashModel()
                {
                    UnitId = m.Sector.CompanyUnitId,
                    UnitName = m.Sector.CompanyUnit.Name,
                    CompanyId = m.CompanyId,
                    CompanyName = m.Company.LegalName
                }).GroupBy(m => m.UnitId)
                .Select(g => g.First());

            var semUnits = allMotor.Where(m => m.SectorId == null && m.MotorDevices.Any())
                .Select(m => new HomeDashModel()
                {
                    UnitId = 0,
                    UnitName = "Sensores Sem Unidade",
                    CompanyId = m.CompanyId,
                    CompanyName = m.Company.LegalName
                }).GroupBy(m => m.CompanyId)
                .Select(g => g.First());

            //allMotor = allMotor.Where(m => m.SectorId == null 
            //    || listaUnits.Any(u => u.UnitId == m.Sector.CompanyUnitId)).ToList();

            if (semUnits != null && semUnits.Any())
            {
                listaUnits = listaUnits.Union(semUnits);
            }

            var cacheEntry = _cache.GetOrCreate($"HomeDashKey{userId}_{idCompany}", entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                entry.SetPriority(CacheItemPriority.High);

                return PopulateHomeDashCache(allMotor);
            });

            listaUnits = listaUnits.Where(u => (u.UnitId == 0 && cacheEntry.Any(c => c.CompanyId == u.CompanyId && c.UnitId == 0)) 
                || (u.UnitId != 0 && cacheEntry.Any(c => c.UnitId == u.UnitId)))
                .OrderBy(u => u.CompanyName);

            ViewBag.IdCompany = idCompany;

            return View(listaUnits);
        }

        public JsonResult HomeDashLvl1_Upd(int idUnit, int idCompany, int idCompanyFilter)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cacheEntry = _cache.GetOrCreate($"HomeDashKey{userId}_{idCompanyFilter}", entry =>
            {
                var user = _userService.Get(Convert.ToInt32(userId));
                var userCompany = user.Contact.CompanyId;
                var companies = _companyService.GetAll().Where(x => x.ParentCompanyId == userCompany).ToList();
                var listaEquips = _motorService.GetAllEquipamento();

                var allMotor = FilterMotorsByUser(user, listaEquips, companies);

                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                entry.SetPriority(CacheItemPriority.High);

                return PopulateHomeDashCache(allMotor);
            });

            cacheEntry = cacheEntry.Where(c => c.UnitId == idUnit && c.CompanyId == idCompany);

            return Json(cacheEntry);
        }

        public IActionResult HomeDashLvl2(int idCompany, int idUnit, string lvlAlert = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var companies = _companyService.GetAll();
            var user = _userService.Get(Convert.ToInt32(userId));

            if (user.UserType.Name != Constants.Roles.Administrator)
            {
                var userCompany = user.Contact.CompanyId;
                companies = companies.Where(x => x.Id == user.Contact.CompanyId || (x.ParentCompanyId == userCompany && x.CompanyTypeId == 3)).ToList();
            }
            else
                companies = companies.ToList();

            ViewBag.UserCompanies = companies;

            var allMotor = _cache.GetOrCreate($"UserMotorCache{userId}", entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                entry.SetPriority(CacheItemPriority.High);

                var listaEquips = _motorService.GetAllEquipamento();

                return FilterMotorsByUser(user, listaEquips, companies);
            });

            IEnumerable<HomeDashModel> listaSectors;

            if (idUnit != 0)
            {
                listaSectors = allMotor
                .Where(m => m.SectorId != null && m.Sector.CompanyUnitId == idUnit && m.MotorDevices.Any())
                .Select(m => new HomeDashModel()
                {
                    UnitId = m.Sector.CompanyUnitId,
                    UnitName = m.Sector.CompanyUnit.Name,
                    SectorId = m.Sector.ParentSectorId == null ? m.Sector.Id : m.Sector.ParentSectorId,
                    SectorName = m.Sector.ParentSectorId == null ? m.Sector.Name : m.Sector.ParentSector.Name,
                    SubSectorId = m.Sector.ParentSectorId != null ? (int?)m.Sector.Id : null,
                    SubSectorName = m.Sector.ParentSectorId != null ? m.Sector.Name : null
                }).GroupBy(m => new { m.SectorId, m.SubSectorId })
                .Select(g => g.First());
            }
            else
            {
                listaSectors = allMotor
                .Where(m => m.SectorId != null && m.MotorDevices.Any())
                .Select(m => new HomeDashModel()
                {
                    UnitId = m.Sector.CompanyUnitId,
                    UnitName = m.Sector.CompanyUnit.Name,
                    SectorId = m.Sector.ParentSectorId == null ? m.Sector.Id : m.Sector.ParentSectorId,
                    SectorName = m.Sector.ParentSectorId == null ? m.Sector.Name : m.Sector.ParentSector.Name,
                    SubSectorId = m.Sector.ParentSectorId != null ? (int?)m.Sector.Id : null,
                    SubSectorName = m.Sector.ParentSectorId != null ? m.Sector.Name : null
                }).GroupBy(m => new { m.SectorId, m.SubSectorId })
                .Select(g => g.First());
            }

            var cacheEntry = _cache.GetOrCreate($"HomeDash2Key{userId}{idUnit}", entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                entry.SetPriority(CacheItemPriority.High);

                return PopulateHomeLvl2Cache(idUnit, allMotor);
            });

            if (string.IsNullOrWhiteSpace(lvlAlert) == false)
            {
                var tempCacheEntry = cacheEntry.Where(m => m.AlertStatus == lvlAlert);
                listaSectors = listaSectors.Where(s => (s.SubSectorId != null && tempCacheEntry.Any(m => m.SubSectorId == s.SubSectorId)) 
                    || (s.SubSectorId == null && tempCacheEntry.Any(m => m.SectorId == s.SectorId)));
                ViewBag.AlertStatus = lvlAlert;
            }

            ViewBag.IdCompany = idCompany;
            ViewBag.IdUnit = idUnit;

            return View(listaSectors);
        }

        public JsonResult HomeDashLvl2_Upd(int idUnit, int? idSector, int? idSubSector, int? idCompany)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var allMotor = _cache.GetOrCreate($"UserMotorCache{userId}", entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                entry.SetPriority(CacheItemPriority.High);

                var user = _userService.Get(Convert.ToInt32(userId));
                var userCompany = user.Contact.CompanyId;
                var companies = _companyService.GetAll().Where(x => x.ParentCompanyId == userCompany).ToList();
                var listaEquips = _motorService.GetAllEquipamento();

                return FilterMotorsByUser(user, listaEquips, companies);
            });

            var cacheEntry = _cache.GetOrCreate($"HomeDash2Key{idUnit}", entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                entry.SetPriority(CacheItemPriority.High);

                return PopulateHomeLvl2Cache(idUnit, allMotor);
            });

            cacheEntry = cacheEntry.Where(c => c.UnitId == idUnit && c.SectorId == idSector && c.SubSectorId == idSubSector);

            return Json(cacheEntry);
        }

        public IActionResult HomeDashLvl3(int? idSector, int? idSubSector, int? idCompany, string lvlAlert = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var companies = _companyService.GetAll();
            var user = _userService.Get(Convert.ToInt32(userId));

            if (user.UserType.Name != Constants.Roles.Administrator)
            {
                var userCompany = user.Contact.CompanyId;
                companies = companies.Where(x => x.Id == user.Contact.CompanyId || (x.ParentCompanyId == userCompany && x.CompanyTypeId == 3)).ToList();
            }
            else
                companies = companies.ToList();

            ViewBag.UserCompanies = companies;

            var allMotor = _cache.GetOrCreate($"UserMotorCache{userId}", entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                entry.SetPriority(CacheItemPriority.High);

                var listaEquips = _motorService.GetAllEquipamento();

                return FilterMotorsByUser(user, listaEquips, companies);
            });

            if (idSubSector != null)
                idSector = idSubSector.Value;

            if (idCompany != null)
                allMotor = allMotor.Where(m => m.CompanyId == idCompany).ToList();

            var listaMotors = allMotor
            .Where(m => m.SectorId == idSector && m.MotorDevices.Any())
                .Select(m => new HomeDashModel()
                {
                    MotorId = m.Id,
                    MotorName = m.Name,
                    UnitId = m.Sector?.CompanyUnitId,
                    UnitName = m.Sector?.CompanyUnit.Name,
                    SectorId = m.Sector?.ParentSectorId == null ? m.Sector?.Id : m.Sector?.ParentSectorId,
                    SectorName = m.Sector?.ParentSectorId == null ? m.Sector?.Name : m.Sector?.ParentSector.Name,
                    SubSectorId = m.Sector?.ParentSectorId != null ? (int?)m.Sector?.Id : null,
                    SubSectorName = m.Sector?.ParentSectorId != null ? m.Sector?.Name : null,
                    CompanyId = ViewBag.IdCompany = m.CompanyId,
                    CompanyName = m.Company?.LegalName
                }).GroupBy(m => m.MotorId)
            .Select(g => g.First());

            var idSector4Cache = idSector;
            if (idSector == null)
            {
                idSector4Cache = 0;
                ViewBag.IdCompany = null;
            }

            var cacheEntry = _cache.GetOrCreate($"HomeDash3Key{userId}{idSector4Cache}", entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                entry.SetPriority(CacheItemPriority.High);

                return PopulateHomeLvl3Cache(idSector, allMotor);
            });

            if (lvlAlert != null)
            {
                var tempCacheEntry = cacheEntry.Where(m => m.AlertStatus == lvlAlert);
                listaMotors = listaMotors.Where(s => tempCacheEntry.Any(m => m.MotorId == s.MotorId));
                ViewBag.AlertStatus = lvlAlert;
            }

            listaMotors = listaMotors.Where(u => cacheEntry.Any(c => c.MotorId == u.MotorId))
                .OrderBy(u => u.MotorName);

            return View(listaMotors);
        }

        public JsonResult HomeDashLvl3_Upd(int idMotor, int idSector)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var allMotor = _cache.GetOrCreate($"UserMotorCache{userId}", entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                entry.SetPriority(CacheItemPriority.High);

                var user = _userService.Get(Convert.ToInt32(userId));
                var userCompany = user.Contact.CompanyId;
                var companies = _companyService.GetAll().Where(x => x.ParentCompanyId == userCompany).ToList();
                var listaEquips = _motorService.GetAllEquipamento();

                return FilterMotorsByUser(user, listaEquips, companies);
            });

            var cacheEntry = _cache.GetOrCreate($"HomeDash3Key{userId}{idSector}", entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                entry.SetPriority(CacheItemPriority.High);

                return PopulateHomeLvl3Cache(idSector, allMotor);
            });

            cacheEntry = cacheEntry.Where(c => c.MotorId == idMotor);

            return Json(cacheEntry);
        }

        public IActionResult HomeDashLvl4(int idMotor, string lvlAlert = null)
        {
            IEnumerable<HomeDashModel> listaMotors = new List<HomeDashModel>();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var companies = _companyService.GetAll();
            var user = _userService.Get(Convert.ToInt32(userId));

            if (user.UserType.Name != Constants.Roles.Administrator)
            {
                var userCompany = user.Contact.CompanyId;
                companies = companies.Where(x => x.Id == user.Contact.CompanyId || (x.ParentCompanyId == userCompany && x.CompanyTypeId == 3)).ToList();
            }
            else
                companies = companies.ToList();

            ViewBag.UserCompanies = companies;

            var listaEquips = _motorService.GetAllEquipamento();

            var listaMotor = FilterMotorsByUser(user, listaEquips, companies);
            ViewBag.MotorSelect = listaMotor.Select(m => new MotorDropdownModel()
            {
                Id = m.Id,
                IsSelected = m.Id == idMotor,
                Name = m.Name,
                SectorId = m.Sector?.ParentSectorId == null ? m.SectorId : m.Sector.ParentSectorId,
                SubSectorId = m.Sector?.ParentSectorId == null ? null : m.SectorId,
                UnitId = m.Sector?.CompanyUnitId,
                IsGrouping = m.IsGrouping
            }).ToList();

            listaMotors = listaMotor
            .Where(m => m.Id == idMotor && m.MotorDevices.Any())
            .Select(m => new HomeDashModel()
            {
                MotorId = m.Id,
                MotorName = m.Name,
                UnitId = m.Sector?.CompanyUnitId,
                UnitName = m.Sector?.CompanyUnit.Name,
                SectorId = m.Sector == null ? null : m.Sector.ParentSectorId == null ? m.Sector.Id : m.Sector.ParentSectorId,
                SectorName = m.Sector == null ? null : m.Sector.ParentSectorId == null ? m.Sector.Name : m.Sector.ParentSector.Name,
                SubSectorId = m.Sector?.ParentSectorId != null ? (int?)m.Sector.Id : null,
                SubSectorName = m.Sector?.ParentSectorId != null ? m.Sector.Name : null,
                CompanyId = ViewBag.IdCompany = m.CompanyId,
                CompanyName = m.Company?.LegalName
            }).GroupBy(m => m.SectorId)
            .Select(g => g.First());

            var cacheEntry = PopulateHomeLvl4Cache(idMotor);

            if (lvlAlert != null)
            {
                cacheEntry = cacheEntry.Where(m => m.alertaAccX == lvlAlert || m.alertaAccY == lvlAlert || m.alertaAccZ == lvlAlert
                    || m.alertaSpdX == lvlAlert || m.alertaSpdY == lvlAlert || m.alertaSpdZ == lvlAlert);
                ViewBag.AlertStatus = lvlAlert;
            }

            ViewBag.ListaSensors = cacheEntry;

            return View(listaMotors);
        }

        public IActionResult Index22()
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
                        receiveDataAndGlobal.Add(new DataAndGlobalModel() { id = data.id, alarm = data.alarme, dataReceive = data.data_hora.Value });
                    }
                }
            }
            if (allGlobal != null && allGlobal.Any())
            {
                foreach (var global in allGlobal)
                {
                    if (receiveDataAndGlobal.Any(rdg => rdg.id == global.id) == false)
                    {
                        receiveDataAndGlobal.Add(new DataAndGlobalModel() { id = global.id, alarm = global.alrm, dataReceive = global.data_hora.Value });
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

        public Dictionary<string, int> GetLastDeviceCodeAlarme2()
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
                        receiveDataAndGlobal.Add(new DataAndGlobalModel() { id = data.id, alarm = data.alarme, dataReceive = data.data_hora.Value });
                    }
                }
            }
            if (allGlobal != null && allGlobal.Any())
            {
                foreach (var global in allGlobal)
                {
                    if (receiveDataAndGlobal.Any(rdg => rdg.id == global.id) == false)
                    {
                        receiveDataAndGlobal.Add(new DataAndGlobalModel() { id = global.id, alarm = global.alrm, dataReceive = global.data_hora.Value });
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
                MotorModel model = new MotorModel();
                var modelName = "";

                if (d.DeviceMotor.Motor.GroupId != null)
                {
                    var motor = _motorService.Get(d.DeviceMotor.Motor.GroupId.Value);
                    modelName = _motorService.Get(d.DeviceMotor.Motor.GroupId.Value).Name;
                    model.Id = motor.Id;
                    model.UnitId = motor.Sector?.CompanyUnitId;
                    model.SectorId = motor.SectorId;
                }
                else
                {
                    modelName = d.DeviceMotor.Motor.Name;
                    model.Id = d.DeviceMotor.Motor.Id;
                    model.UnitId = d.DeviceMotor.Motor.Sector?.CompanyUnitId;
                    model.SectorId = d.DeviceMotor.Motor.SectorId;
                }

                model.Name = modelName;
                model.DeviceId = d.Id;
                model.Device = d;

                if (model.UnitId != null)
                {
                    var unit = _unitService.Get(model.UnitId.Value);
                    model.Unit = unit;
                }

                if (deviceCodeAndAlarm.Any(dca => dca.Key == d.Code))
                {
                    model.Alarm = deviceCodeAndAlarm[d.Code];
                }

                list.Add(model);
            }

            return list;
        }

        private IEnumerable<HomeDashModel> PopulateHomeDashCache(IEnumerable<Motor> userMotors)
        {
            var listaAlerts = new List<DataGlobalHomeModel>();

            //foreach (var motor in userMotors)
            //{
                var alerts = _receiveService.ListDeviceAlarmesAgregado(null, null);
                var deviceAlerts = alerts.Where(a => a.alerta != "Sem Sinal")
                    .GroupBy(a => a.deviceId);

                if (alerts != null && alerts.Any(a => a.alerta == "Sem Sinal"))
                {
                    foreach (var alertSS in alerts.Where(a => a.alerta == "Sem Sinal"))
                    {
                        listaAlerts.Add(alertSS);
                    }
                }

                foreach (var alert in deviceAlerts)
                {
                    listaAlerts.Add(alert.First());
                }
            //}

            var listaUnit = listaAlerts.GroupBy(g => new { g.unitId, g.alerta })
                .Select(g => new HomeDashModel()
                {
                    CompanyId = g.FirstOrDefault()?.companyId,
                    UnitId = g.Key.unitId.HasValue ? g.Key.unitId.Value : 0,
                    AlertStatus = g.Key.alerta,
                    UnitName = g.FirstOrDefault()?.unitName,
                    AlertQtd = g.Count()
                });

            return listaUnit;
        }

        private IEnumerable<HomeDashModel> PopulateHomeLvl2Cache(int idUnit, List<Motor> userMotors)
        {
            var listaAlerts = new List<DataGlobalHomeModel>();
            var userMotorsToIterate = idUnit != 0 ? userMotors.Where(m => m.Sector?.CompanyUnitId == idUnit)
                : userMotors;

            foreach (var motor in userMotorsToIterate)
            {
                var alerts = _receiveService.ListDeviceAlarmesAgregado(null, motor.Id);
                var deviceAlerts = alerts.Where(a => a.alerta != "Sem Sinal")
                    .GroupBy(a => a.deviceId);

                if (alerts != null && alerts.Any(a => a.alerta == "Sem Sinal"))
                {
                    foreach (var alertSS in alerts.Where(a => a.alerta == "Sem Sinal"))
                    {
                        listaAlerts.Add(alertSS);
                    }
                }

                foreach (var alert in deviceAlerts)
                {
                    listaAlerts.Add(alert.First());
                }
            }

            var listaUnit = listaAlerts.GroupBy(g => new { g.sectorId, g.subSectorId, g.alerta })
                .Select(g => new HomeDashModel()
                {
                    SectorId = g.Key.sectorId,
                    SubSectorId = g.Key.subSectorId,
                    AlertStatus = g.Key.alerta,
                    UnitId = g.FirstOrDefault() != null ? g.First().unitId.Value : 0,
                    UnitName = g.FirstOrDefault()?.unitName,
                    AlertQtd = g.Count()
                });

            return listaUnit;
        }

        private IEnumerable<HomeDashModel> PopulateHomeLvl3Cache(int? idSector, List<Motor> userMotors)
        {
            var listaAlerts = new List<DataGlobalHomeModel>();

            foreach (var motor in userMotors.Where(m => m.SectorId == idSector))
            {
                var alerts = _receiveService.ListDeviceAlarmesAgregado(null, motor.Id);
                var deviceAlerts = alerts.Where(a => a.alerta != "Sem Sinal")
                    .GroupBy(a => a.deviceId);

                if (alerts != null && alerts.Any(a => a.alerta == "Sem Sinal"))
                {
                    foreach (var alertSS in alerts.Where(a => a.alerta == "Sem Sinal"))
                    {
                        listaAlerts.Add(alertSS);
                    }
                }
                
                foreach (var alert in deviceAlerts)
                {
                    listaAlerts.Add(alert.First());
                }
            }

            var listaUnit = listaAlerts.GroupBy(g => new { g.motorId, g.alerta })
                .Select(g => new HomeDashModel()
                {
                    MotorId = g.Key.motorId,
                    AlertStatus = g.Key.alerta,
                    SectorId = g.FirstOrDefault() != null ? g.First().sectorId : 0,
                    SubSectorId = g.FirstOrDefault() != null ? g.First().subSectorId : 0,
                    UnitId = g.FirstOrDefault() != null ? g.First().unitId : 0,
                    UnitName = g.FirstOrDefault()?.unitName,
                    AlertQtd = g.Count()
                });

            return listaUnit;
        }

        private IEnumerable<DataGlobalHomeModel> PopulateHomeLvl4Cache(int idMotor)
        {
            var listaCache = new List<DataGlobalHomeModel>();
            var listaSensores = _receiveService.ListDeviceAlarmes(null, idMotor).ToList();

            foreach(var signalsBySensor in listaSensores.GroupBy(s => s.deviceId))
            {
                var lastSignal = signalsBySensor.FirstOrDefault();
                if (lastSignal != null)
                {
                    if (lastSignal.alertaAccX == "")
                    {
                        var lastAlertaAccX = signalsBySensor.FirstOrDefault(s => s.alertaAccX != "");
                        if (lastAlertaAccX != null)
                            lastSignal.alertaAccX = lastAlertaAccX.alertaAccX;
                    }
                    if (lastSignal.alertaAccY == "")
                    {
                        var lastAlertaAccY = signalsBySensor.FirstOrDefault(s => s.alertaAccY != "");
                        if (lastAlertaAccY != null)
                            lastSignal.alertaAccY = lastAlertaAccY.alertaAccY;
                    }
                    if (lastSignal.alertaAccZ == "")
                    {
                        var lastAlertaAccZ = signalsBySensor.FirstOrDefault(s => s.alertaAccZ != "");
                        if (lastAlertaAccZ != null)
                            lastSignal.alertaAccZ = lastAlertaAccZ.alertaAccZ;
                    }
                    if (lastSignal.alertaSpdX == "")
                    {
                        var lastAlertaSpdX = signalsBySensor.FirstOrDefault(s => s.alertaSpdX != "");
                        if (lastAlertaSpdX != null)
                            lastSignal.alertaSpdX = lastAlertaSpdX.alertaSpdX;
                    }
                    if (lastSignal.alertaSpdY == "")
                    {
                        var lastAlertaSpdY = signalsBySensor.FirstOrDefault(s => s.alertaSpdY != "");
                        if (lastAlertaSpdY != null)
                            lastSignal.alertaSpdY = lastAlertaSpdY.alertaSpdY;
                    }
                    if (lastSignal.alertaSpdZ == "")
                    {
                        var lastAlertaSpdZ = signalsBySensor.FirstOrDefault(s => s.alertaSpdZ != "");
                        if (lastAlertaSpdZ != null)
                            lastSignal.alertaAccZ = lastAlertaSpdZ.alertaSpdZ;
                    }

                    listaCache.Add(lastSignal);
                }
            }

            return listaCache;
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
