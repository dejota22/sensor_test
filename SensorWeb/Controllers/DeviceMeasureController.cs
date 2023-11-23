using AutoMapper;
using Core;
using Core.ApiModel.Request;
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
        IDeviceService _deviceService;
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
                                IDeviceService DeviceService,
                                IMotorService MotorService,
                                IUserService UserService,
                                ICompanyService CompanyService,
                                IMapper mapper,
                                IStringLocalizer<Resources.CommonResources> localizer,
                                ILogger<DeviceMeasureController> logger)
        {
            _deviceMeasureService = DeviceMeasureService;
            _deviceService = DeviceService;
            _motorService = MotorService;
            _userService = UserService;
            _companyService = CompanyService;
            _mapper = mapper;
            _localizer = localizer;
            _logger = logger;
        }

        public ActionResult Index(string codeAttempt = null)
        {
            try
            {
                var configModel = new DeviceConfigurationModel();

                if (string.IsNullOrWhiteSpace(codeAttempt) == false)
                    configModel = _deviceMeasureService.GetLastBySensorCode(codeAttempt);
                else
                    configModel = _deviceMeasureService.GetLast(0, 0);

                ViewBag.AllCodes = _deviceService.GetAll().Select(d => d.Code);

                ViewBag.MotorSelect = _motorService.GetAllEquipamento().Select(m => new MotorDropdownModel()
                {
                    Id = m.Id,
                    Name = m.Name,
                    SectorId = m.Sector?.ParentSectorId == null ? m.SectorId : m.Sector.ParentSectorId,
                    SubSectorId = m.Sector?.ParentSectorId == null ? null : m.SectorId,
                    UnitId = m.Sector?.CompanyUnitId
                }).ToList();

                configModel.codigoSensor = codeAttempt;
                return View(configModel);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Erro:{ex.Message}");
                throw;
            }
        }

        // POST: DeviceMeasureController/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(DeviceConfigurationModel deviceMeasureModel)
        {
            try
            {
                PersistDeviceConfiguration(deviceMeasureModel);

                ViewBag.AllCodes = _deviceService.GetAll().Select(d => d.Code);

                return View(deviceMeasureModel);
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Copy(DeviceConfigurationModel deviceMeasureModel)
        {
            try
            {
                var configModel = _deviceMeasureService
                    .GetLast(deviceMeasureModel.DeviceId.Value, deviceMeasureModel.MotorId.Value);

                if (configModel.Id == 0)
                {
                    deviceMeasureModel.isEdit = false;
                }
                else
                {
                    deviceMeasureModel.isEdit = true;
                }

                PersistDeviceConfiguration(deviceMeasureModel);

                return RedirectToAction(nameof(Index), new { codeAttempt = deviceMeasureModel.codigoSensor });
            }
            catch (Exception e)
            {
                return View();
            }
        }

        public JsonResult GetExistingConfig(int idMotor, int idDevice)
        {
            var configModel = _deviceMeasureService.GetLast(idDevice, idMotor);
            var existing = configModel.Id != 0;

            return Json(new { exists = existing });
        }

        private void PersistDeviceConfiguration(DeviceConfigurationModel deviceMeasureModel)
        {
            if (deviceMeasureModel.DeviceId != null && deviceMeasureModel.MotorId != null)
            {
                DeviceConfigurationSpecialRead usrSetup = null;
                if (deviceMeasureModel.isEditUserSetup == true)
                {
                    usrSetup = deviceMeasureModel.GetDeviceConfigurationSpecialReadFromModel();
                    usrSetup.CreatedAt = DateTime.Now;

                    if (deviceMeasureModel.isEdit == true)
                    {
                        var usrSetupFromDB = _deviceMeasureService.GetUsrSetup(deviceMeasureModel.MotorId.Value, deviceMeasureModel.DeviceId.Value);

                        if (usrSetupFromDB != null)
                        {
                            usrSetupFromDB.usr_amostras = usrSetup.usr_amostras;
                            usrSetupFromDB.usr_freq_cut = usrSetup.usr_freq_cut;
                            usrSetupFromDB.usr_eixo = usrSetup.usr_eixo;
                            usrSetupFromDB.usr_filtro = usrSetup.usr_filtro;
                            usrSetupFromDB.usr_fs = usrSetup.usr_fs;
                            usrSetupFromDB.usr_odr = usrSetup.usr_odr;
                        }


                        _deviceMeasureService.Edit(deviceMeasureModel.GetDeviceConfigurationFromModel(), usrSetupFromDB != null ? usrSetupFromDB : usrSetup);
                    }
                    else
                    {
                        _deviceMeasureService.Insert(deviceMeasureModel.GetDeviceConfigurationFromModel(), usrSetup);
                    }
                }
                else
                {
                    if (deviceMeasureModel.isEdit == true)
                    {
                        var configModel = _deviceMeasureService.GetLast(deviceMeasureModel.DeviceId.Value, deviceMeasureModel.MotorId.Value);
                        if (configModel != null)
                        {
                            deviceMeasureModel.Id = configModel.Id;
                            deviceMeasureModel.config = true;

                            _deviceMeasureService.Edit(deviceMeasureModel.GetDeviceConfigurationFromModel(), null);
                        }
                    }
                    else
                    {
                        //if (ModelState.IsValid)
                        //{
                        deviceMeasureModel.Id = _deviceMeasureService.GetlastCode();
                        _deviceMeasureService.Insert(deviceMeasureModel.GetDeviceConfigurationFromModel(), null);
                        //}
                    }
                }
            }
        }
    }
}
