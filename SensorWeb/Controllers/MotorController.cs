using AutoMapper;
using Core;
using Core.DTO;
using Core.Service;
using Core.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SensorWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SensorWeb.Controllers
{
    [Authorize]
    public class MotorController : BaseController
    {
        IMotorService _MotorService;
        IMapper _mapper;
        IUserService _userService;
        ICompanyService _companyService;
        IDeviceService _deviceService;
        private readonly IStringLocalizer<Resources.CommonResources> _localizer;

        private readonly ILogger<MotorController> _logger;
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="MotorService"></param>
        /// <param name="mapper"></param>
        /// <param name="localizer"></param>
        public MotorController(IMotorService MotorService,
                                  IUserService UserService,
                                  ICompanyService CompanyService,
                                  IDeviceService DeviceService,
                                  IMapper mapper,
                                  IStringLocalizer<Resources.CommonResources> localizer,
                                  ILogger<MotorController> logger)
        {
            _MotorService = MotorService;
            _userService = UserService; ;
            _companyService = CompanyService;
            _deviceService = DeviceService;
            _mapper = mapper;
            _localizer = localizer;
            _logger = logger;
        }

        // GET: MotorController
        public ActionResult Index()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = _userService.Get(Convert.ToInt32(userId));
                var userCompany = user.Contact.CompanyId;
                var companies = _companyService.GetAll().Where(x => x.ParentCompanyId == userCompany).ToList();

                var listaMotors = _MotorService.GetAll();

                var listaMotorModel = _mapper.Map<List<MotorModel>>(listaMotors);

                if (user.UserType.Name != Constants.Roles.Administrator)
                {
                    listaMotorModel = listaMotorModel.Where(x => x.CompanyId == userCompany || companies.Any(y => y.Id == x.CompanyId)).ToList();
                }

                return View(listaMotorModel.OrderBy(x => x.Id));
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Erro:{ex.Message}");
                throw;
            }

        }

        // GET: MotorController/Details/5
        public ActionResult Details(int id)
        {
            Motor Motor = _MotorService.Get(id);
            MotorModel MotorModel = _mapper.Map<MotorModel>(Motor);
            return View(MotorModel);
        }

        // GET: MotorController/Create
        public ActionResult Create()
        {
            var userId = LoggedUserId;
            var user = _userService.Get(Convert.ToInt32(userId));
            var userCompany = user.Contact.CompanyId;
            var companies = _companyService.GetAll().Where(x => x.ParentCompanyId == userCompany).ToList();

            var motorModel = new MotorModel
            {
                Companies = _companyService.GetAll().Where(x => x.ParentCompanyId == userCompany || x.Id == userCompany)
                    .Select(y => new SelectListItemDTO()
                    {
                        Key = y.Id,
                        Value = y.TradeName
                    }).Distinct().ToList(),
                Devices = _deviceService.GetAll().Where(x => x.CompanyId == userCompany || companies.Any(y => y.Id == x.CompanyId))
                    .Select(y => new SelectListItemDTO()
                    {
                        Key = y.Id,
                        Value = y.Tag
                    }).Distinct().ToList()
            };

            return View(motorModel);
        }

        // POST: MotorController/CreateCopy
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCopy(MotorModel motorModel)
        {
            motorModel.Id = 0;
            var userId = LoggedUserId;
            var user = _userService.Get(Convert.ToInt32(userId));
            var userCompany = user.Contact.CompanyId;
            var companies = _companyService.GetAll().Where(x => x.ParentCompanyId == userCompany).ToList();

            motorModel.Companies = _companyService.GetAll().Where(x => x.ParentCompanyId == userCompany || x.Id == userCompany)
                    .Select(y => new SelectListItemDTO()
                    {
                        Key = y.Id,
                        Value = y.TradeName
                    }).Distinct().ToList();

            motorModel.Devices = _deviceService.GetAll().Where(x => x.CompanyId == userCompany || companies.Any(y => y.Id == x.CompanyId))
                    .Select(y => new SelectListItemDTO()
                    {
                        Key = y.Id,
                        Value = y.Tag
                    }).Distinct().ToList();

            return View("Create", motorModel);
        }

        // POST: MotorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MotorModel motorModel)
        {

            //motorModel.CompressorTypeId = 1;

            try
            {
                if (ModelState.IsValid)
                {
                    motorModel.Id = _MotorService.GetlastCode();
                    var Motor = _mapper.Map<Motor>(motorModel);
                    _MotorService.Insert(Motor);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View();
            }
        }

        // GET: MotorController/Edit/5
        public ActionResult Edit(int id)
        {
            var userId = LoggedUserId;
            var user = _userService.Get(Convert.ToInt32(userId));
            var userCompany = user.Contact.CompanyId;
            var companies = _companyService.GetAll().Where(x => x.ParentCompanyId == userCompany).ToList();

            var motor = _MotorService.Get(id);
            var motorModel = _mapper.Map<MotorModel>(motor);

            if (motorModel != null)
            {
                motorModel.Companies = _companyService.GetAll().Where(x => x.ParentCompanyId == userCompany || x.Id == userCompany)
                .Select(y => new SelectListItemDTO()
                {
                    Key = y.Id,
                    Value = y.TradeName
                }).Distinct().ToList();

                motorModel.Devices = _deviceService.GetAll().Where(x => x.CompanyId == userCompany || companies.Any(y => y.Id == x.CompanyId))
                .Select(y => new SelectListItemDTO()
                {
                    Key = y.Id,
                    Value = y.Tag
                }).Distinct().ToList();
            }

            return View(motorModel);
        }

        // POST: MotorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, MotorModel MotorModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Motor = _mapper.Map<Motor>(MotorModel);
                    _MotorService.Edit(Motor);

                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MotorController/Delete/5
        public ActionResult Delete(int id)
        {
            Motor Motor = _MotorService.Get(id);
            MotorModel MotorModel = _mapper.Map<MotorModel>(Motor);
            return View(MotorModel);
        }

        // POST: MotorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, MotorModel MotorModel)
        {
            try
            {
                _MotorService.Remove(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
