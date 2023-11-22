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

                var listaMotors = _MotorService.GetAllEquipamento();

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

            ViewBag.MotorDevices = _deviceService.GetAll()
                    .Where(x => x.DeviceMotor.MotorId == id).ToList();

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
                    }).Distinct().ToList()

                //Devices = _deviceService.GetAll()
                //.Where(x => (x.CompanyId == userCompany || companies.Any(y => y.Id == x.CompanyId)) 
                //    && x.DeviceMotorId == null)
                //    .Select(y => new SelectListItemDTO()
                //    {
                //        Key = y.Id,
                //        Value = y.Tag
                //    }).Distinct().ToList()
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

            //motorModel.Devices = _deviceService.GetAll().Where(x => x.CompanyId == userCompany || companies.Any(y => y.Id == x.CompanyId))
            //        .Select(y => new SelectListItemDTO()
            //        {
            //            Key = y.Id,
            //            Value = y.Tag
            //        }).Distinct().ToList();

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

                var allDevices = _deviceService.GetAll();

                motorModel.Devices = allDevices
                    .Where(x => (x.CompanyId == userCompany || companies.Any(y => y.Id == x.CompanyId))
                        && x.DeviceMotorId == null)
                    .Select(y => new SelectListItemDTO()
                    {
                        Key = y.Id,
                        Value = y.Tag
                    }).Distinct().ToList();

                ViewBag.MotorDevices = allDevices
                    .Where(x => x.DeviceMotor?.MotorId == id).ToList();
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

        [HttpPost]
        public JsonResult InsertMotorDevice(int mId, int dId, string planoMedicao, string orientacaoSensor)
        {
            var device = _deviceService.Get(dId);
            var motor = _MotorService.Get(mId);

            if (device != null)
            {
                var motorDevice = new DeviceMotor()
                {
                    MotorId = mId,
                    MeasurementPlan = planoMedicao,
                    SensorOrientation = orientacaoSensor
                };

                motor.MotorDevices.Add(motorDevice);
                _MotorService.Edit(motor);

                device.Company = null;
                device.DeviceMotor = motorDevice;
                _deviceService.Edit(device);
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult DeleteMotorDevice(int mId, int id)
        {
            var motor = _MotorService.Get(mId);

            if (motor != null && motor.MotorDevices.Any(d => d.Id == id))
            {
                var mDevice = motor.MotorDevices.First(d => d.Id == id);
                var device = _deviceService.GetAll().FirstOrDefault(d => d.DeviceMotorId == id);

                if (device != null)
                {
                    device.Company = null;
                    device.DeviceMotorId = null; device.DeviceMotor = null;
                    _deviceService.Edit(device);
                }

                motor.MotorDevices.Remove(mDevice);
                _MotorService.Edit(motor);
            }

            return Json(new { success = true });
        }


        #region Agrupamento

        public ActionResult GroupIndex()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = _userService.Get(Convert.ToInt32(userId));
                var userCompany = user.Contact.CompanyId;
                var companies = _companyService.GetAll().Where(x => x.ParentCompanyId == userCompany).ToList();

                var listaMotors = _MotorService.GetAllAgrupamento();

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

        public ActionResult GroupCreate()
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
                    }).Distinct().ToList()
            };

            return View(motorModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GroupCreate(MotorModel motorModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    motorModel.Id = _MotorService.GetlastCode();
                    var motor = _mapper.Map<Motor>(motorModel);
                    motor.IsGrouping = true;
                    int mId = _MotorService.Insert(motor);

                    return RedirectToAction(nameof(GroupEdit), new { id = mId });
                }
                else
                    return View(motorModel);
            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(GroupCreate));
            }
        }

        public ActionResult GroupEdit(int id)
        {
            var userId = LoggedUserId;
            var user = _userService.Get(Convert.ToInt32(userId));
            var userCompany = user.Contact.CompanyId;
            var companies = _companyService.GetAll().Where(x => x.ParentCompanyId == userCompany).ToList();

            var motor = _MotorService.Get(id);
            var motorModel = _mapper.Map<MotorModel>(motor);

            if (motorModel != null)
            {
                ViewBag.CompanyName = _companyService.Get(motorModel.CompanyId).TradeName;

                var allMotors = _MotorService.GetAllEquipamento();

                motorModel.Equips = allMotors
                    .Where(x => x.GroupId != id && 
                        (x.CompanyId == userCompany || companies.Any(y => y.Id == x.CompanyId)))
                    .Select(y => new SelectListItemDTO()
                    {
                        Key = y.Id,
                        Value = y.Name
                    }).Distinct().ToList();

                ViewBag.Motors = allMotors
                    .Where(x => x.GroupId == id).ToList();
            }

            return View(motorModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GroupEdit(int id, MotorModel motorModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var motor = _mapper.Map<Motor>(motorModel);
                    motor.IsGrouping = true;
                    _MotorService.Edit(motor);

                }

                return RedirectToAction(nameof(GroupEdit), new { id = motorModel.Id }); ;
            }
            catch
            {
                return View();
            }
        }

        public ActionResult GroupDetails(int id)
        {
            var motor = _MotorService.Get(id);
            var motorModel = _mapper.Map<MotorModel>(motor);

            if (motorModel != null)
            {
                ViewBag.CompanyName = _companyService.Get(motorModel.CompanyId).TradeName;

                var allMotors = _MotorService.GetAllEquipamento();

                ViewBag.Motors = allMotors
                    .Where(x => x.GroupId == id).ToList();
            }

            return View(motorModel);
        }

        public ActionResult GroupDelete(int id)
        {
            Motor motor = _MotorService.Get(id);
            MotorModel motorModel = _mapper.Map<MotorModel>(motor);
            return View(motorModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GroupDelete(int id, MotorModel motorModel)
        {
            try
            {
                var group = _MotorService.Get(id);

                if (group.Motors.Any())
                {
                    ViewBag.ErrorMsg = "Não foi possível excluir. Este Agrupamento possui Equipamentos vinculados.";
                    return View(_mapper.Map<MotorModel>(group));
                }

                _MotorService.Remove(id);
                return RedirectToAction(nameof(GroupIndex));
            }
            catch
            {
                return View(motorModel);
            }
        }

        [HttpPost]
        public JsonResult InsertMotorToGroup(int mId, int gId)
        {
            var motor = _MotorService.Get(mId);
            var group = _MotorService.Get(gId);

            if (motor != null)
            {
                group.Motors.Add(motor);
                _MotorService.Edit(group);
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult RemoveMotorFromGroup(int mId, int gId)
        {
            var group = _MotorService.Get(gId);

            if (group != null && group.Motors.Any(m => m.Id == mId))
            {
                var mFromGroup = group.Motors.First(m => m.Id == mId);

                group.Motors.Remove(mFromGroup);
                _MotorService.Edit(group);
            }

            return Json(new { success = true });
        }

        #endregion
    }
}
