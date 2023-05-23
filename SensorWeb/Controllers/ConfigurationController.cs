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
    public class ConfigurationController : BaseController
    {
        IDeviceMeasureService _deviceMeasureService;
        IMotorService _motorService;
        IUserService _userService;
        ICompanyService _companyService;
        IMapper _mapper;
        private readonly ILogger<ConfigurationController> _logger;
        private readonly IStringLocalizer<Resources.CommonResources> _localizer;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="DeviceMeasureService"></param>
        /// <param name="mapper"></param>
        /// <param name="localizer"></param>
        public ConfigurationController(IDeviceMeasureService DeviceMeasureService,
                                   IMotorService MotorService,
                                   IUserService UserService,
                                   ICompanyService CompanyService,
                                   IMapper mapper,
                                   IStringLocalizer<Resources.CommonResources> localizer,
                                   ILogger<ConfigurationController> logger)
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
    }
}
