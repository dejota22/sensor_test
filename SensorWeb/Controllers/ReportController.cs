using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using SensorWeb.Models;
using SelectPdf;
using System.Threading.Tasks;
using Core.Service;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Core;
using Google.Protobuf.WellKnownTypes;
using System;
using SensorWeb.Resources;
using Org.BouncyCastle.Asn1.X509;
using System.IO;
using Core.DTO;
using System.Drawing.Printing;
using Microsoft.EntityFrameworkCore;
using SensorService;
using System.Security.Claims;
using Core.Utils;
using System.ComponentModel.Design;
using System.Text;

namespace SensorWeb.Controllers
{
    [Authorize]
    public class ReportController : BaseController
    {
        IMapper _mapper;
        private readonly IStringLocalizer<Resources.CommonResources> _localizer;
        ICompanyService _companyService;
        IReceiveService _receiveService;
        IDeviceConfigurationService _deviceConfigService;
        IDeviceService _deviceService;
        IMotorService _motorService;
        IUserService _userService;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="localizer"></param>
        public ReportController(IMapper mapper, IStringLocalizer<Resources.CommonResources> localizer, 
            ICompanyService CompanyService, IReceiveService ReceiveService, IDeviceConfigurationService DeviceConfigurationService, 
            IDeviceService DeviceService, IMotorService MotorService, IUserService UserService)
        {
            _mapper = mapper;
            _localizer = localizer;
            _companyService = CompanyService;
            _receiveService = ReceiveService;
            _deviceConfigService = DeviceConfigurationService;
            _deviceService = DeviceService;
            _motorService = MotorService;
            _userService = UserService;
        }

        // GET: ReportController
        public ActionResult Index(int? DeviceId, int? MotorId, string? DeviceIdName,
            int? CompanyId, int? UnitId, int? SectorId, int? SubSectorId)
        {
            ReportModel model = new ReportModel();
            ViewBag.DeviceData = new List<ReceiveData>();

            var companies = _companyService.GetAll();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.Get(Convert.ToInt32(userId));

            if (user.UserType.Name != Constants.Roles.Administrator)
            {
                var userCompany = user.Contact.CompanyId;
                companies = companies.Where(x => x.Id == user.Contact.CompanyId || (x.ParentCompanyId == userCompany && x.CompanyTypeId == 3)).ToList();
            }
            else
                companies = companies.ToList();

            ViewBag.UserCompanies = companies;

            ViewBag.MotorSelect = GetListaMotorsFromUser().Select(m => new MotorDropdownModel()
            {
                Id = m.Id,
                IsSelected = MotorId != null && m.Id == MotorId,
                Name = m.Name,
                SectorId = m.Sector?.ParentSectorId == null ? m.SectorId : m.Sector.ParentSectorId,
                SubSectorId = m.Sector?.ParentSectorId == null ? null : m.SectorId,
                UnitId = m.Sector?.CompanyUnitId,
                IsGrouping = m.IsGrouping
            }).ToList();

            if (DeviceId != null && MotorId != null)
            {
                model.DeviceId = DeviceId.Value;
                model.DeviceIdName = DeviceIdName;
                model.MotorId = MotorId.Value;

                var motor = _motorService.Get(MotorId.Value);
                if (motor.IsGrouping)
                {
                    var device = _deviceService.Get(DeviceId.Value);
                    MotorId = device.DeviceMotor.MotorId;
                }

                var dataList = _receiveService.GetDataByDeviceMotor(DeviceId, MotorId);
                if (dataList != null)
                {
                    ViewBag.DeviceData = dataList;
                }
            }

            ViewBag.CompanyId = CompanyId;
            ViewBag.UnitId = UnitId;
            ViewBag.SectorId = SectorId;
            ViewBag.SubSectorId = SubSectorId;

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(ReportModel report)
        {
            if (report.TipoRelatorio == 4)
                return View("DownloadStPDF", report);

            return View("DownloadPDF", report);
        }

        public ActionResult ReportDeviceData(int? DeviceId, int? MotorId, string? DeviceIdName, int? CompanyId,
            int? UnitId, int? SectorId, int? SubSectorId, int? dmotorId, string? deviceIdName)
        {
            ViewBag.DeviceData = new List<ReceiveData>();

            var companies = _companyService.GetAll();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.Get(Convert.ToInt32(userId));

            if (user.UserType.Name != Constants.Roles.Administrator)
            {
                var userCompany = user.Contact.CompanyId;
                companies = companies.Where(x => x.Id == user.Contact.CompanyId || (x.ParentCompanyId == userCompany && x.CompanyTypeId == 3)).ToList();
            }
            else
                companies = companies.ToList();

            ViewBag.UserCompanies = companies;

            var listaMotor = GetListaMotorsFromUser().Select(m => new MotorDropdownModel()
            {
                Id = m.Id,
                IsSelected = MotorId != null && m.Id == MotorId,
                Name = m.Name,
                SectorId = m.Sector?.ParentSectorId == null ? m.SectorId : m.Sector.ParentSectorId,
                SubSectorId = m.Sector?.ParentSectorId == null ? null : m.SectorId,
                UnitId = m.Sector?.CompanyUnitId,
                IsGrouping = m.IsGrouping
            }).ToList();

            ViewBag.MotorSelect = listaMotor;

            if (DeviceId != null && MotorId != null)
            {
                var motor = _motorService.Get(MotorId.Value);
                if (motor.IsGrouping)
                {
                    var device = _deviceService.Get(DeviceId.Value);
                    MotorId = device.DeviceMotor.MotorId;
                }

                ViewBag.DeviceSelect = (KeyValuePair<int?, string>?)new KeyValuePair<int?, string>(DeviceId, DeviceIdName);

                var dataList = _receiveService.GetDataByDeviceMotor(DeviceId, MotorId);
                if (dataList != null)
                {
                    ViewBag.DeviceData = dataList;
                }
            }

            ViewBag.CompanyId = CompanyId;
            ViewBag.UnitId = UnitId;
            ViewBag.SectorId = SectorId;
            ViewBag.SubSectorId = SubSectorId;

            if (dmotorId != null)
            {
                var selectedMotor = listaMotor.FirstOrDefault(m => m.Id == dmotorId);

                if (selectedMotor != null)
                {
                    ViewBag.UnitId = selectedMotor.UnitId;
                    ViewBag.SectorId = selectedMotor.SectorId;
                    ViewBag.SubSectorId = selectedMotor.SubSectorId;
                    ViewBag.MotorId = dmotorId;

                    var deviceVals = deviceIdName.Split(new char[] { '_' });
                    ViewBag.DeviceSelect =
                        (KeyValuePair<int?, string>?)new KeyValuePair<int?, string>(int.Parse(deviceVals[0]), deviceVals[1]);

                    var dataList = _receiveService.GetDataByDeviceMotor(int.Parse(deviceVals[0]), dmotorId);
                    if (dataList != null)
                    {
                        ViewBag.DeviceData = dataList;
                    }
                }
            }

            return View();
        }

        public JsonResult ReportDeviceDataUpdate(int idDataReceive)
        {
            var dadosDataReceive = _receiveService.GetDataDadoByDataReceiveId(idDataReceive);

            return Json(dadosDataReceive);
        }

        public FileResult ReportDeviceDataExport(int idDataReceive)
        {
            var headerDataReceive = _receiveService.GetDataDadoExportByDataReceiveId(idDataReceive);
            var dadosDataReceive = _receiveService.GetDataDadoByDataReceiveId(idDataReceive);

            StringBuilder sb = new StringBuilder();
            sb.Append("Data Hora Recebimento"); sb.Append(";");
            sb.Append("Empresa"); sb.Append(";");
            sb.Append("Unidade"); sb.Append(";");
            sb.Append("Setor"); sb.Append(";");
            sb.Append("Subsetor"); sb.Append(";");
            sb.Append("Equipamento"); sb.Append(";");
            sb.Append("Sensor"); sb.Append(";");
            sb.Append("Tipo"); sb.Append(";");
            sb.Append("RMS"); sb.Append(";");
            sb.Append("Fator de Crista"); sb.Append(";");
            sb.Append("Alarme"); sb.Append(";");
            sb.Append("ODR"); sb.Append(";");
            sb.Append("Freq Cut"); sb.Append(";");
            sb.Append("Filtro"); sb.Append(";");
            sb.Append("Eixo"); sb.Append(";");
            sb.Append("FS"); sb.Append(";");
            sb.Append("Amostras");
            sb.Append("\n");

            foreach (var dado in headerDataReceive)
            {

                sb.Append(dado.dataReceive.ToString("dd/MM/yyyy HH:mm")); sb.Append(";");
                sb.Append(dado.companyName); sb.Append(";");
                sb.Append(dado.unitName); sb.Append(";");
                sb.Append(dado.sectorName); sb.Append(";");
                sb.Append(dado.subSectorName); sb.Append(";");
                sb.Append(dado.motor); sb.Append(";");
                sb.Append(dado.sensor); sb.Append(";");
                sb.Append(dado.tipo); sb.Append(";");
                sb.Append(dado.rms.Replace('.',',')); sb.Append(";");
                sb.Append(dado.fatorCrista.Replace('.', ',')); sb.Append(";");
                sb.Append(dado.alarme); sb.Append(";");
                sb.Append(dado.odr); sb.Append(";");
                sb.Append(dado.freqCut); sb.Append(";");
                sb.Append(dado.filtro); sb.Append(";");
                sb.Append(dado.eixo); sb.Append(";");
                sb.Append(dado.fs); sb.Append(";");
                sb.Append(dado.amostras); sb.Append(";");
                sb.Append("\n");
            }

            sb.Append("\n");

            sb.Append("tempo"); sb.Append(";");
            sb.Append("valor");
            sb.Append("\n");

            foreach (var dado in dadosDataReceive)
            {

                sb.Append(dado.tempo); sb.Append(";");
                sb.Append(dado.valor); sb.Append(";");

                sb.Append("\n");
            }

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
            return File(buffer, "application/octet-stream", $"{DateTime.Now.ToString("dd-MM-yyyy")}_Sinal_no_Tempo_{idDataReceive}.csv");
        }

        public ActionResult ReportDeviceDataRedict(int DeviceId, int MotorId, int DeviceDataId)
        {
            var companies = _companyService.GetAll();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.Get(Convert.ToInt32(userId));

            if (user.UserType.Name != Constants.Roles.Administrator)
            {
                var userCompany = user.Contact.CompanyId;
                companies = companies.Where(x => x.Id == user.Contact.CompanyId || (x.ParentCompanyId == userCompany && x.CompanyTypeId == 3)).ToList();
            }
            else
                companies = companies.ToList();

            ViewBag.UserCompanies = companies;

            var listaMotor = GetListaMotorsFromUser().Select(m => new MotorDropdownModel()
            {
                Id = m.Id,
                IsSelected = MotorId != null && m.Id == MotorId,
                Name = m.Name,
                SectorId = m.Sector?.ParentSectorId == null ? m.SectorId : m.Sector.ParentSectorId,
                SubSectorId = m.Sector?.ParentSectorId == null ? null : m.SectorId,
                UnitId = m.Sector?.CompanyUnitId,
                IsGrouping = m.IsGrouping
            }).ToList();

            ViewBag.MotorSelect = listaMotor;

            var device = _deviceService.Get(DeviceId);
            ViewBag.DeviceSelect = (KeyValuePair<int?, string>?)new KeyValuePair<int?, string>(DeviceId, device.Tag);

            ViewBag.DeviceData = new List<ReceiveData>();

            var deviceData = _receiveService.GetData(DeviceDataId);
            if (deviceData != null)
            {
                ViewBag.DeviceData.Add(deviceData);
                ViewBag.DeviceDataId = deviceData.IdReceiveData;
            }

            return View("ReportDeviceData");
        }

        public ActionResult ReportRMSCrista(int? motorId, string? deviceIdName, string? tipo, string? eixo)
        {
            var companies = _companyService.GetAll();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.Get(Convert.ToInt32(userId));

            if (user.UserType.Name != Constants.Roles.Administrator)
            {
                var userCompany = user.Contact.CompanyId;
                companies = companies.Where(x => x.Id == user.Contact.CompanyId || (x.ParentCompanyId == userCompany && x.CompanyTypeId == 3)).ToList();
            }
            else
                companies = companies.ToList();

            ViewBag.UserCompanies = companies;

            var listaMotor = GetListaMotorsFromUser().Select(m => new MotorDropdownModel()
            {
                Id = m.Id,
                Name = m.Name,
                SectorId = m.Sector?.ParentSectorId == null ? m.SectorId : m.Sector.ParentSectorId,
                SubSectorId = m.Sector?.ParentSectorId == null ? null : m.SectorId,
                UnitId = m.Sector?.CompanyUnitId,
                IsGrouping = m.IsGrouping
            }).ToList();

            ViewBag.MotorSelect = listaMotor;

            if (motorId != null)
            {
                var selectedMotor = listaMotor.FirstOrDefault(m => m.Id == motorId);

                if (selectedMotor != null)
                {
                    var deviceVals = deviceIdName.Split(new char[] { '_' });
                    ViewBag.DeviceSelect = 
                        (KeyValuePair<int?, string>?)new KeyValuePair<int?, string>(int.Parse(deviceVals[0]), deviceVals[1]);

                    ViewBag.CompanyId = selectedMotor.CompanyId;
                    ViewBag.UnitId = selectedMotor.UnitId;
                    ViewBag.SectorId = selectedMotor.SectorId;
                    ViewBag.SubSectorId = selectedMotor.SubSectorId;
                    ViewBag.MotorId = motorId;
                    ViewBag.TipoRelatorio = tipo;
                    ViewBag.Eixo = eixo;
                }
            }

            return View();
        }

        public ActionResult ReportOcorrencias()
        {
            var companies = _companyService.GetAll();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.Get(Convert.ToInt32(userId));

            if (user.UserType.Name != Constants.Roles.Administrator)
            {
                var userCompany = user.Contact.CompanyId;
                companies = companies.Where(x => x.Id == user.Contact.CompanyId || (x.ParentCompanyId == userCompany && x.CompanyTypeId == 3)).ToList();
            }
            else
                companies = companies.ToList();

            ViewBag.UserCompanies = companies;

            ViewBag.MotorSelect = GetListaMotorsFromUser().Select(m => new MotorDropdownModel()
            {
                Id = m.Id,
                Name = m.Name,
                SectorId = m.Sector?.ParentSectorId == null ? m.SectorId : m.Sector.ParentSectorId,
                SubSectorId = m.Sector?.ParentSectorId == null ? null : m.SectorId,
                UnitId = m.Sector?.CompanyUnitId,
                IsGrouping = m.IsGrouping
            }).ToList();

            return View(new ReportOcorrenciasModel());
        }

        [HttpPost]
        public ActionResult ReportOcorrencias(ReportOcorrenciasModel model, int? CompanyId, int? UnitId, int? SectorId, int? SubSectorId, int? MotorId)
        {
            var companies = _companyService.GetAll();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.Get(Convert.ToInt32(userId));

            if (user.UserType.Name != Constants.Roles.Administrator)
            {
                var userCompany = user.Contact.CompanyId;
                companies = companies.Where(x => x.Id == user.Contact.CompanyId || (x.ParentCompanyId == userCompany && x.CompanyTypeId == 3)).ToList();
            }
            else
                companies = companies.ToList();

            ViewBag.UserCompanies = companies;

            var newEndDate = model.EndDate;
            if (model.EndDate.HasValue)
                newEndDate = model.EndDate.Value.Date.Add(new TimeSpan(23, 59, 59));

            var modelMotorId = model.MotorId;
            Device device = null;
            if (model.DeviceId != null)
                device = _deviceService.Get(model.DeviceId.Value);
            
            if (modelMotorId != null)
            {
                var motor = _motorService.Get(modelMotorId.Value);
                if (motor.IsGrouping && device != null)
                {
                    modelMotorId = device.DeviceMotor?.MotorId;
                }
            }

            var deviceName = device?.Tag;

            var list = GetDeviceCodeAlarme(model.DeviceId, modelMotorId, model.StartDate, newEndDate, model.Gravidade, model.PageIndex);
            model.DataGlobalModel = list;

            var listTotal = _receiveService.ListDeviceCodeAlarmeCount(model.DeviceId, modelMotorId, model.StartDate, newEndDate, model.Gravidade);
            model.PageTotal = listTotal > 10 ? ((listTotal + 10 - 1) / 10) - 1 : 0;

            if (model.DeviceId != null)
                ViewBag.DeviceSelect = (KeyValuePair<int?, string>?)new KeyValuePair<int?, string>(model.DeviceId, deviceName);

            ViewBag.MotorSelect = GetListaMotorsFromUser().Select(m => new MotorDropdownModel()
            {
                Id = m.Id,
                IsSelected = model.MotorId != null && m.Id == model.MotorId,
                Name = m.Name,
                SectorId = m.Sector?.ParentSectorId == null ? m.SectorId : m.Sector.ParentSectorId,
                SubSectorId = m.Sector?.ParentSectorId == null ? null : m.SectorId,
                UnitId = m.Sector?.CompanyUnitId,
                IsGrouping = m.IsGrouping
            }).ToList();

            ViewBag.CompanyId = CompanyId;
            ViewBag.UnitId = UnitId;
            ViewBag.SectorId = SectorId;
            ViewBag.SubSectorId = SubSectorId;
            ViewBag.MotorId = MotorId;

            return View(model);
        }

        public JsonResult ReportRMSCristaUpdate(int deviceId, int motorId, DateTime startDate, DateTime endDate, int reportType, int eixo)
        {
            var newEndDate = endDate.Date.Add(new TimeSpan(23, 59, 59));

            var motor = _motorService.Get(motorId);
            if (motor.IsGrouping)
            {
                var device = _deviceService.Get(deviceId);
                motorId = device.DeviceMotor.MotorId;
            }

            var dadosDataReceive = _receiveService.GetDataUnionGlobalByDateType(deviceId, motorId, startDate, newEndDate, reportType, eixo);
            var limites = _deviceConfigService.GetLimitesAccSpd(deviceId, motorId, reportType);

            var limiteRed = limites != null ? limites["red"].Value : 0;
            var limiteYel = limites != null ? limites["yel"].Value : 0;

            return Json(new { dgraf = dadosDataReceive, limitered = limiteRed, limiteyel = limiteYel });
        }

        public static string GetSetupInfo(string attr, int index)
        {
            if (attr == "eixo")
            {
                switch(index)
                {
                    case 1:
                        return "X";
                    case 2:
                        return "Y";
                    case 3:
                        return "Z";
                    case 4:
                        return "XY";
                    case 5:
                        return "XZ";
                    case 6:
                        return "YZ";
                    case 7:
                        return "XYZ";
                }
            }
            else if (attr == "freq")
            {
                switch (index)
                {
                    case 0:
                        return "6K6";
                    case 1:
                        return "2K6";
                    case 2:
                        return "1K3";
                    case 3:
                        return "0K5";
                    case 4:
                        return "0K2";
                    case 5:
                        return "0K1";
                    case 6:
                        return "K67";
                }
            }

            return "";
        }

        private List<DataGlobalModel> GetDeviceCodeAlarme(int? deviceId, int? motorId, DateTime? startDate, 
            DateTime? endDate, string gravidade, int pageIndex = 0)
        {
            if (motorId != null)
            {
                var motor = _motorService.Get(motorId.Value);
                if (motor.IsGrouping)
                {
                    var device = _deviceService.Get(deviceId.Value);
                    motorId = device.DeviceMotor.MotorId;
                }
            }

            var skip = pageIndex * 10;
            var receiveDataAndGlobal = _receiveService
                .ListDeviceCodeAlarme(deviceId, motorId, startDate, endDate, gravidade, skip);

            return receiveDataAndGlobal.ToList();
        }

        private IEnumerable<Motor> GetListaMotorsFromUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.Get(Convert.ToInt32(userId));
            var userCompany = user.Contact.CompanyId;
            var companies = _companyService.GetAll().Where(x => x.ParentCompanyId == userCompany).ToList();

            var listaMotors = _motorService.GetAllEquipamento();

            if (user.UserType.Name == Constants.Roles.Supervisor || user.UserType.Name == Constants.Roles.User)
            {
                listaMotors = listaMotors.Where(x => x.CompanyId == userCompany).ToList();
            }
            else if (user.UserType.Name == Constants.Roles.Sysadmin)
            {
                listaMotors = listaMotors.Where(x => x.CompanyId == userCompany || companies.Any(y => y.Id == x.CompanyId)).ToList();
            }

            return listaMotors;
        }
    }
}
