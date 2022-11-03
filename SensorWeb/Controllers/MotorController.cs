using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SensorWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SensorWeb.Controllers
{
    public class MotorController : BaseController
    {
        IMotorService _MotorService;
        IMapper _mapper;
        private readonly IStringLocalizer<Resources.CommonResources> _localizer;

        private readonly ILogger<MotorController> _logger;
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="MotorService"></param>
        /// <param name="mapper"></param>
        /// <param name="localizer"></param>
        public MotorController(IMotorService MotorService,
                                  IMapper mapper,
                                  IStringLocalizer<Resources.CommonResources> localizer,
                                  ILogger<MotorController> logger)
        {
            _MotorService = MotorService;
            _mapper = mapper;
            _localizer = localizer;
            _logger = logger;
        }

        // GET: MotorController
        public ActionResult Index()
        {
            try
            {
                var listaUsuarios = _MotorService.GetAll();
                _logger.LogInformation($"Busca OK");
                var listaMotorModel = _mapper.Map<List<MotorModel>>(listaUsuarios);
                _logger.LogInformation($"Busca OK1");

                //foreach (var MotorModel in listaMotorModel)
                //{
                //    MotorModel.Contact = _mapper.Map<List<ContactModel>>(_MotorService.GetAll())
                //}

                //Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
                //  ViewData["Title"] = _localizer["MotorTittle"];

                return View(listaMotorModel.OrderBy(x => x.Id));
            }
            catch(Exception ex)
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
            //MotorModel motor = new MotorModel()
            //{
            //    Id = _MotorService.GetlastCode()
            //};

            return View();
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
            catch(Exception e)
            {
                return View();
            }
        }

        // GET: MotorController/Edit/5
        public ActionResult Edit(int id)
        {
            Motor Motor = _MotorService.Get(id);
            MotorModel MotorModel = _mapper.Map<MotorModel>(Motor);
            return View(MotorModel);
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
