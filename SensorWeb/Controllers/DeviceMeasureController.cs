using AutoMapper;
using Core;
using Core.DTO;
using Core.Service;
using Core.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SensorService;
using SensorWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SensorWeb.Controllers
{
    [Authorize]
    public class DeviceMeasureController : BaseController
    {
        IDeviceConfigurationService _deviceMeasureService;
        IMotorService _motorService;
        IUserService _userService;
        ICompanyService _companyService;
        IMapper _mapper;
        private readonly ILogger<DeviceMeasureController> _logger;
        private readonly IStringLocalizer<Resources.CommonResources> _localizer;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="DeviceMeasureService"></param>
        /// <param name="mapper"></param>
        /// <param name="localizer"></param>
        public DeviceMeasureController(IDeviceConfigurationService DeviceMeasureService,
                                   IMotorService MotorService,
                                   IUserService UserService,
                                   ICompanyService CompanyService,
                                   IMapper mapper,
                                   IStringLocalizer<Resources.CommonResources> localizer,
                                   ILogger<DeviceMeasureController> logger)
        {
            _deviceMeasureService = DeviceMeasureService;
            _motorService = MotorService;
            _userService = UserService;
            _companyService = CompanyService;
            _mapper = mapper;
            _localizer = localizer;
            _logger = logger;
        }

        public ActionResult Index(int? DeviceId, int? MotorId)
        {
            try
            {
                //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //var user = _userService.Get(Convert.ToInt32(userId));
                //var userCompany = user.Contact.CompanyId;
                //var companies = _companyService.GetAll().Where(x => x.ParentCompanyId == userCompany).ToList();

                //var listaMotors = _motorService.GetAll();

                //var listaMotorModel = _mapper.Map<List<MotorModel>>(listaMotors);

                //if (user.UserType.Name != Constants.Roles.Administrator)
                //{
                //    listaMotorModel = listaMotorModel.Where(x => x.CompanyId == userCompany || companies.Any(y => y.Id == x.CompanyId)).ToList();
                //}

                var configModel = new DeviceConfigurationModel();

                if (DeviceId != null && MotorId != null)
                    configModel = _deviceMeasureService.GetLast(DeviceId.Value, MotorId.Value);
                else
                    configModel = _deviceMeasureService.GetLast(0, 0);

                return View(configModel);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Erro:{ex.Message}");
                throw;
            }
        }

        // POST: DeviceMeasureController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DeviceConfigurationModel deviceMeasureModel)
        {
            try
            {
                if (deviceMeasureModel.DeviceId != null && deviceMeasureModel.MotorId != null)
                {
                    var configModel = _deviceMeasureService.GetLast(deviceMeasureModel.DeviceId.Value, deviceMeasureModel.MotorId.Value);
                    if (configModel != null)
                    {
                        deviceMeasureModel.Id = configModel.Id;
                        _deviceMeasureService.Edit(deviceMeasureModel.GetDeviceConfigurationFromModel());
                    }
                }
                else
                {
                    //if (ModelState.IsValid)
                    //{
                    deviceMeasureModel.Id = _deviceMeasureService.GetlastCode();
                    //var deviceMeasure = _mapper.Map<DeviceMeasure>(deviceMeasureModel);
                    var deviceMeasure = deviceMeasureModel.GetDeviceConfigurationFromModel();
                    _deviceMeasureService.Insert(deviceMeasure);
                    //}
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View();
            }
        }
    }
}
