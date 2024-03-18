using Autofac.Core;
using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SensorService;
using SensorWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace SensorWeb.Controllers
{
    [Authorize]
    public class ConfigController : BaseController
    {
        IConfigService _configService;
        IUserService _userService;
        IMapper _mapper;

        public ConfigController(IConfigService configService, IUserService userService, IMapper mapper)
        {
            _configService = configService;
            _userService = userService;
            _mapper = mapper;
        }

        // GET: Config
        public ActionResult Index()
        {
            var listConfigs = _configService.GetAllParam();

            return View(listConfigs);
        }

        // GET: Config/UpdateParam/5
        public ActionResult UpdateParam(int id, string value)
        {
            ConfigParams config = _configService.GetParam(id);
            var oldValue = config.Value;

            config.Value = value;
            _configService.EditParam(config);

            var userId = LoggedUserId;
            var user = _userService.Get(Convert.ToInt32(userId));

            ConfigLog log = new ConfigLog()
            {
                Name = "ParamChange",
                PrimaryId = id,
                PrimaryName = config.Name,
                UserName = $"{userId} - {user.Email}",
                Description = $"De {oldValue} para {value}"
            };

            _configService.InsertLog(log);

            return RedirectToAction(nameof(Index));
        }

        
    }
}
