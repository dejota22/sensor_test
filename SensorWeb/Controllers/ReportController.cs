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

namespace SensorWeb.Controllers
{
    [Authorize]
    public class ReportController : BaseController
    {
        IMapper _mapper;
        private readonly IStringLocalizer<Resources.CommonResources> _localizer;
        IReceiveService _receiveService;
        IDeviceConfigurationService _deviceConfigService;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="localizer"></param>
        public ReportController(IMapper mapper, IStringLocalizer<Resources.CommonResources> localizer, IReceiveService ReceiveService, IDeviceConfigurationService DeviceConfigurationService)
        {
            _mapper = mapper;
            _localizer = localizer;
            _receiveService = ReceiveService;
            _deviceConfigService = DeviceConfigurationService;
        }

        // GET: ReportController
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReportModel report)
        {
            var reportView = await this.RenderViewToStringAsync("DownloadPDF", report);

            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();


            converter.Options.CssMediaType = HtmlToPdfCssMediaType.Print;
            //converter.Options.ViewerPreferences.FitWindow = true;
            converter.Options.ViewerPreferences.CenterWindow = true;
            converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

            PdfDocument doc = converter.ConvertHtmlString(reportView);

            byte[] pdf = doc.Save();
            doc.Close();

            FileResult fileResult = new FileContentResult(pdf, "application/pdf")
            {
                FileDownloadName = $"{report.NomeArquivo}.pdf"
            };

            return fileResult;
        }

        public ActionResult ReportDeviceData(int? DeviceId, int? MotorId)
        {
            ViewBag.DeviceData = new List<ReceiveData>();

            if (DeviceId != null && MotorId != null)
            {
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

        public ActionResult ReportRMSCrista()
        {
            return View();
        }

        public JsonResult ReportRMSCristaUpdate(int deviceId, int motorId, DateTime startDate, DateTime endDate, int reportType, int eixo)
        {
            var newEndDate = endDate.Date.Add(new TimeSpan(23, 59, 59));

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
    }
}
