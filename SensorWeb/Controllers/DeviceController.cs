using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using SensorWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Logging;
using QRCoder;
using Microsoft.AspNetCore.Authorization;
using Core.Utils;
using SensorService;
using System.Security.Claims;
using Core.DTO;

namespace SensorWeb.Controllers
{
    [Authorize]
    public class DeviceController : BaseController
    {
        IDeviceService _deviceService;
        IUserService _userService;
        ICompanyService _companyService;
        IMapper _mapper;
        private readonly IStringLocalizer<Resources.CommonResources> _localizer;

        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly ILogger<DeviceController> _logger;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="DeviceService"></param>
        /// <param name="mapper"></param>
        /// <param name="localizer"></param>
        public DeviceController(IDeviceService DeviceService,
                                IUserService UserService, 
                                ICompanyService CompanyService,
                                  IMapper mapper,
                                  IStringLocalizer<Resources.CommonResources> localizer,
                                  IWebHostEnvironment webHostEnvironment,
                                  ILogger<DeviceController> logger)
        {
            _deviceService = DeviceService;
            _userService = UserService;
            _companyService = CompanyService;
            _mapper = mapper;
            _localizer = localizer;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        // GET: DeviceController
        public ActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.Get(Convert.ToInt32(userId));
            var userCompany = user.Contact.CompanyId;
            var companies = _companyService.GetAll().Where(x => x.ParentCompanyId == userCompany).ToList();

            var listaDevices = _deviceService.GetAll().Where(x => x.CompanyId == userCompany || companies.Any(y => y.Id == x.CompanyId)).ToList();
            var listaDeviceModel = _mapper.Map<List<DeviceModel>>(listaDevices);

            foreach (var DeviceModel in listaDeviceModel)
            {
                DeviceModel.QrCodeImg = QrCodeGen(DeviceModel.Code);
            }

            return View(listaDeviceModel.OrderBy(x => x.Id));
        }

        // GET: DeviceController/Details/5
        public ActionResult Details(int id)
        {
            Device Device = _deviceService.Get(id);
            DeviceModel deviceModel = _mapper.Map<DeviceModel>(Device);
            //ViewData["imgQrCode"] = deviceModel.Tag + ".png";
            //ViewData["imgQrCode"] = Convert.ToBase64String(QrCodeGen(deviceModel.Tag));
            if (deviceModel != null && !String.IsNullOrEmpty(deviceModel.Code))
                deviceModel.QrCodeImg = QrCodeGen(deviceModel.Code);

            return View(deviceModel);
        }

        public ActionResult Create()
        {
            //DeviceModel Device = new DeviceModel()
            //{
            //    Id = _DeviceService.GetlastCode()
            //};

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCopy(DeviceModel model)
        {
            model.Id = 0;
            return View("Create", model);
        }

        // POST: DeviceController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Constants.Roles.Administrator)]
        public ActionResult Create(DeviceModel deviceModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    deviceModel.Id = _deviceService.GetlastCode();
                    var Device = _mapper.Map<Device>(deviceModel);
                    _deviceService.Insert(Device);

                    _logger.LogInformation($"Insert Device OK");
                    string webRootPath = _webHostEnvironment.WebRootPath;
                    string contentRootPath = _webHostEnvironment.ContentRootPath;

                    _logger.LogInformation($"getPath OK");

                    string path = "";
                    path = Path.Combine(webRootPath, "Resources", "QrCodeTags/");

                    _logger.LogInformation(path);
                    //or path = Path.Combine(contentRootPath , "wwwroot" ,"CSS" );                    
                    //try
                    //{
                    //    // Generate a Simple BarCode image and save as PDF
                    //    QRCodeWriter.CreateQrCode(deviceModel.Tag, 500, QRCodeWriter.QrErrorCorrectionLevel.Medium).SaveAsPng(path + deviceModel.Tag + ".png");

                    //}
                    //catch(Exception ex)
                    //{
                    //    _logger.LogInformation($"Erro:{ex.Message}");
                    //    throw;

                    //}
                    // ViewData["imgQrCode"] = deviceModel.Tag + ".png";

                    if (deviceModel != null)
                        ViewData["imgQrCode"] = QrCodeGen(deviceModel.Code);


                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View();
            }
        }


        public byte[] QrCodeGen(string qrTexto)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrTexto, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            return BitmapToBytes(qrCodeImage);
        }
        private static Byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        // GET: DeviceController/Edit/5
        public ActionResult Edit(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.Get(Convert.ToInt32(userId));
            var userCompany = user.Contact.CompanyId;

            Device device = _deviceService.Get(id);

            DeviceModel deviceModel = _mapper.Map<DeviceModel>(device);
            //ViewData["imgQrCode"] = deviceModel.Tag + ".png";
            if (deviceModel != null)
            {
                deviceModel.QrCodeImg = QrCodeGen(deviceModel.Code);
                deviceModel.Companies = _companyService.GetAll().Where(x => x.ParentCompanyId == userCompany || x.Id == userCompany)
                    .Select(y => new SelectListItemDTO()
                    {
                        Key = y.Id,
                        Value = y.TradeName
                    }).Distinct().ToList();
            }
            return View(deviceModel);
        }

        // POST: DeviceController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, DeviceModel DeviceModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Device = _mapper.Map<Device>(DeviceModel);
                    _deviceService.Edit(Device);

                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DeviceController/Delete/5
        public ActionResult Delete(int id)
        {
            Device Device = _deviceService.Get(id);
            DeviceModel DeviceModel = _mapper.Map<DeviceModel>(Device);
            return View(DeviceModel);
        }

        // POST: DeviceController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, DeviceModel DeviceModel)
        {
            try
            {
                _deviceService.Remove(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public JsonResult GetDeviceData(int id)
        {
            Device device = _deviceService.Get(id);
            DeviceModel deviceModel = _mapper.Map<DeviceModel>(device);

            return Json(deviceModel);
        }
    }
}
