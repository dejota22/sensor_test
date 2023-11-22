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

namespace SensorWeb.Controllers
{
    [Authorize]
    public class ReportController : BaseController
    {
        IMapper _mapper;
        private readonly IStringLocalizer<Resources.CommonResources> _localizer;
        IReceiveService _receiveService;
        IDeviceConfigurationService _deviceConfigService;
        IDeviceService _deviceService;
        IMotorService _motorService;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="localizer"></param>
        public ReportController(IMapper mapper, IStringLocalizer<Resources.CommonResources> localizer, 
            IReceiveService ReceiveService, IDeviceConfigurationService DeviceConfigurationService, 
            IDeviceService DeviceService, IMotorService MotorService)
        {
            _mapper = mapper;
            _localizer = localizer;
            _receiveService = ReceiveService;
            _deviceConfigService = DeviceConfigurationService;
            _deviceService = DeviceService;
            _motorService = MotorService;
        }

        // GET: ReportController
        public ActionResult Index(int? DeviceId, int? MotorId, string? DeviceIdName)
        {
            ReportModel model = new ReportModel();

            ViewBag.DeviceData = new List<ReceiveData>();

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

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(ReportModel report)
        {
            if (report.TipoRelatorio == 4)
                return View("DownloadStPDF", report);

            return View("DownloadPDF", report);
        }

        public ActionResult ReportDeviceData(int? DeviceId, int? MotorId, string? DeviceIdName)
        {
            ViewBag.DeviceData = new List<ReceiveData>();

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
                
            return View();
        }

        public JsonResult ReportDeviceDataUpdate(int idDataReceive)
        {
            var dadosDataReceive = _receiveService.GetDataDadoByDataReceiveId(idDataReceive);

            return Json(dadosDataReceive);
        }

        public ActionResult ReportDeviceDataRedict(int DeviceId, int MotorId, int DeviceDataId)
        {
            ViewBag.DeviceData = new List<ReceiveData>();

            var deviceData = _receiveService.GetData(DeviceDataId);
            if (deviceData != null)
            {
                ViewBag.DeviceData.Add(deviceData);
                ViewBag.DeviceDataId = deviceData.IdReceiveData;
            }

            return View("ReportDeviceData");
        }

        public ActionResult ReportRMSCrista()
        {
            return View();
        }

        public ActionResult ReportOcorrencias()
        {
            return View(new ReportOcorrenciasModel());
        }

        [HttpPost]
        public ActionResult ReportOcorrencias(ReportOcorrenciasModel model)
        {
            var newEndDate = model.EndDate;
            if (model.EndDate.HasValue)
                newEndDate = model.EndDate.Value.Date.Add(new TimeSpan(23, 59, 59));

            var modelMotorId = model.MotorId;
            var motor = _motorService.Get(model.MotorId.Value);
            var device = _deviceService.Get(model.DeviceId.Value);
            var deviceName = device.Tag;
            if (motor.IsGrouping)
            {
                modelMotorId = device.DeviceMotor.MotorId;
            }

            var list = GetDeviceCodeAlarme(model.DeviceId, modelMotorId, model.StartDate, newEndDate, model.Gravidade, model.PageIndex);
            model.DataGlobalModel = list;

            var listTotal = _receiveService.ListDeviceCodeAlarmeCount(model.DeviceId, modelMotorId, model.StartDate, newEndDate, model.Gravidade);
            model.PageTotal = listTotal > 10 ? ((listTotal + 10 - 1) / 10) - 1 : 0;

            ViewBag.DeviceSelect = (KeyValuePair<int?, string>?)new KeyValuePair<int?, string>(model.DeviceId, deviceName);

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
            var motor = _motorService.Get(motorId.Value);
            if (motor.IsGrouping)
            {
                var device = _deviceService.Get(deviceId.Value);
                motorId = device.DeviceMotor.MotorId;
            }

            var skip = pageIndex * 10;
            var receiveDataAndGlobal = _receiveService
                .ListDeviceCodeAlarme(deviceId, motorId, startDate, endDate, gravidade, skip);

            return receiveDataAndGlobal.ToList();
        }
    }
}
