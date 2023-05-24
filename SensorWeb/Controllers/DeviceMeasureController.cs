using AutoMapper;
using Core;
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
        IDeviceMeasureService _deviceMeasureService;
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
        public DeviceMeasureController(IDeviceMeasureService DeviceMeasureService,
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

        public ActionResult Index(string readDataType, int MotorId)
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

                return View();
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
        public ActionResult Create(DeviceMeasureModel deviceMeasureModel)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                    deviceMeasureModel.Id = _deviceMeasureService.GetlastCode();
                    var deviceMeasure = _mapper.Map<DeviceMeasure>(deviceMeasureModel);
                    _deviceMeasureService.Insert(deviceMeasure);
                //}

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View();
            }
        }
    }
}
