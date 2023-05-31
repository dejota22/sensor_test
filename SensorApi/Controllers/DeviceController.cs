using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using Core;
using Core.ApiModel.Request;
using Core.DTO;
using SensorApi.Models;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SensorApi.Interfaces;
using Newtonsoft.Json;
using Core.ApiModel.Response;
using SensorService;

namespace SensorApi.Controllers
{
    [Authorize]
    [Route("api/device")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceConfigurationService _DeviceConfigurationService;
        private readonly IDeviceMeasureService _DeviceMeasureService;
        private readonly IMotorService _MotorService;
        private readonly IDeviceService _DeviceService;
        private readonly IJwtAuth _jwtAuth;
        private readonly IMapper _mapper;


        private readonly IMachineService _MachineService;
        private readonly IFixationTypeService _FixationService;
        private readonly ICouplingTypeService _CouplingService;
        private readonly ICardanShaftTypeService _CardanShaftTypeService;
        private readonly IPumpTypeService _PumpTypeService;
        private readonly ICompressorTypeService _CompressorTypeService;
        //private readonly ILogger<DeviceController> _logger;

        /// <summary>
        /// Construtor
        /// </summary>        
        public DeviceController(IDeviceConfigurationService DeviceConfigurationService, IDeviceMeasureService DeviceMeasureService, IMotorService MotorService, IDeviceService DeviceService, IJwtAuth jwtAuth, IMapper mapper,
            IMachineService MachineService, IFixationTypeService FixationService, ICouplingTypeService CouplingService,
            ICardanShaftTypeService CardanShaftTypeService, IPumpTypeService PumpTypeService, ICompressorTypeService CompressorTypeService)
        {
            _DeviceConfigurationService = DeviceConfigurationService;
            _DeviceMeasureService = DeviceMeasureService;
            _MotorService = MotorService;
            _DeviceService = DeviceService;
            _jwtAuth = jwtAuth;
            _mapper = mapper;
            _MachineService = MachineService;
            _FixationService = FixationService;
            _CouplingService = CouplingService;
            _CardanShaftTypeService = CardanShaftTypeService;
            _PumpTypeService = PumpTypeService;
            _CompressorTypeService = CompressorTypeService;
            //  _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        [Produces("application/json")]
        public ContentResult Get()
        {

            //_logger.LogInformation($"Acesso ok");

            string respJson = "{\"Status\":\"Active\",\"Connection\":\"Ok\"}";
            ContentResult contRes1 = Content(Newtonsoft.Json.Linq.JObject.Parse(respJson).ToString(), "application/json");
            return contRes1;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("deviceMeasure")]
        public ContentResult AddDeviceMeasure([FromBody] DeviceRequest devicemeasure)
        {
            string respJson = string.Empty;

            if (devicemeasure == null)
            {
                respJson = "{\"Erro Interno\":\"Erro na leitura dos dados recebidos - Estrutura vazia\"}";
                ContentResult contRes1 = Content(Newtonsoft.Json.Linq.JObject.Parse(respJson).ToString(), "application/json");
                contRes1.StatusCode = (int)HttpStatusCode.InternalServerError;
                return contRes1;
            }

            ////Faz busca do Motor para salvar informaões no Measure
            ////var getDevice = _MotorService.GetBymotorTag(devicemeasure.T);

            //var getDevice = _DeviceService.GetByDeviceTag(devicemeasure.T);
            //if (getDevice == null)
            //{
            //    respJson = "{\"Erro\":\"Tag do Sensor não encontrado no cadastro\"}";
            //    ContentResult contRes1 = Content(Newtonsoft.Json.Linq.JObject.Parse(respJson).ToString(), "application/json");
            //    contRes1.StatusCode = (int)HttpStatusCode.InternalServerError;
            //    return contRes1;
            //}

            //DeviceMeasure deviMes = new DeviceMeasure
            //{
            //    //MotorId = getDevice.Id,
            //    DeviceId = getDevice.Id,
            //    Temperature = devicemeasure.E, //Temperature
            //    ReadDataType = devicemeasure.R, //Tipo de Leitura 
            //    CreatedAt = DateTime.Now,
            //    YAxle = string.Empty,
            //    ZAxle = string.Empty,
            //    XAxle = string.Empty,
            //};

            //try
            //{
            //    deviMes.XAxle = devicemeasure.D.Where(x => !String.IsNullOrEmpty(x.x)).FirstOrDefault().x; //Eixo x
            //}
            //catch { }
            //try
            //{
            //    deviMes.YAxle = devicemeasure.D.Where(y => !String.IsNullOrEmpty(y.y)).FirstOrDefault().y; //Eixo y
            //}
            //catch { }
            //try
            //{
            //    deviMes.ZAxle = devicemeasure.D.Where(z => !String.IsNullOrEmpty(z.z)).FirstOrDefault().z; //Eixo z
            //}
            //catch { }

            //_DeviceMeasureService.Insert(deviMes);

            respJson = "{\"T\":\"13b579\",\"R\":\"S6302$\",\"S\":\"14400\"}";
            ContentResult contRes = Content(Newtonsoft.Json.Linq.JObject.Parse(respJson).ToString(), "application/json");
            contRes.ContentType = "application/json";
            return contRes;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("deviceGlobal")]
        public ContentResult AddDeviceGlobal([FromBody] DeviceGlobalRequest deviceGlobal)
        {
            string respJson;
            if (deviceGlobal == null)
            {
                respJson = "{\"Erro Interno\":\"Erro na leitura dos dados recebidos - Estrutura vazia\"}";
                ContentResult contRes1 = Content(Newtonsoft.Json.Linq.JObject.Parse(respJson).ToString(), "application/json");
                contRes1.StatusCode = (int)HttpStatusCode.InternalServerError;
                return contRes1;
            }

            var path = "C:\\Log\\global.txt";
            var text = String.Format("{0}: {1}", DateTime.Now, JsonConvert.SerializeObject(deviceGlobal));

            if (!System.IO.File.Exists(path))
            {
                var createFile = System.IO.File.Create(path);
                createFile.Close();
            }

            System.IO.File.AppendAllText(path, text + Environment.NewLine);

            var deviceConfigEnviados = new List<int>();
            var response = new DeviceGlobalResponse() { };

            var deviceConfigList = _DeviceConfigurationService.GetAll()
                .Where(s => s.config == true)
                .GroupBy(s => s.DeviceId);

            response.Sensor = new List<Sensor>();
            response.Horarios = new List<Horario>();
            response.Gatilhos = new List<Gatilho>();
            response.Lora = new List<Lora>();

            foreach (var deviceConfig in deviceConfigList)
            {
                var deviceConfigId = 0;
                var sensor = new Sensor();
                sensor.SetupAcc = new List<Setup>();
                sensor.SetupSpd = new List<Setup>();
                sensor.SetupUsr = new List<Setup>();

                var horario = new Horario();
                horario.HorariosEnviosCard = new List<HorariosEnviosCard>();

                var gatilho = new Gatilho();
                var lora = new Lora();

                foreach (var setup in deviceConfig)
                {
                    deviceConfigId = setup.Id;

                    sensor.Config = Convert.ToInt32(setup.config);

                    sensor.SetupAcc.Add(new Setup()
                    {
                        Amostras = setup.acc_amostras.Value,
                        Eixo = setup.acc_eixo.Value,
                        Filtro = setup.acc_filtro.Value,
                        FreqCut = setup.acc_freq_cut.Value,
                        Fs = setup.acc_fs.Value,
                        Odr = setup.acc_odr.Value
                    });

                    sensor.SetupSpd.Add(new Setup()
                    {
                        Amostras = setup.spd_amostras.Value,
                        Eixo = setup.spd_eixo.Value,
                        Filtro = setup.spd_filtro.Value,
                        FreqCut = setup.spd_freq_cut.Value,
                        Fs = setup.spd_fs.Value,
                        Odr = setup.spd_odr.Value
                    });

                    var usrSetupData = _DeviceConfigurationService.GetUsrSetup(setup.MotorId.Value, setup.DeviceId.Value);
                    if (usrSetupData != null)
                    {
                        sensor.SetupUsr.Add(new Setup()
                        {
                            Amostras = usrSetupData.usr_amostras.Value,
                            Eixo = usrSetupData.usr_eixo.Value,
                            Filtro = usrSetupData.usr_filtro.Value,
                            FreqCut = usrSetupData.usr_freq_cut.Value,
                            Fs = usrSetupData.usr_fs.Value,
                            Odr = usrSetupData.usr_odr.Value
                        });
                    }

                    horario.Config = Convert.ToInt32(setup.config);
                    horario.QuantHorariosCards = setup.quant_horarios_cards.Value;
                    horario.ModoHora = setup.modo_hora.Value;
                    horario.IntervaloAnalise = setup.intervalo_analise.Value;
                    horario.IntervaloAnaliseAlarme = setup.intervalo_analise_alarme.Value;
                    horario.ContaEnvios = setup.conta_envios.Value;
                    horario.DiaEnvioRelat = setup.dia_envio_relat;
                    horario.HoraEnvioRelat = setup.hora_envio_relat;
                    horario.DiasRun = setup.dias_run;
                    horario.FimTurno = setup.fim_turno;
                    horario.InicioTurno = setup.inicio_turno;

                    horario.HorariosEnviosCard = new List<HorariosEnviosCard>();
                    setup.DeviceConfigurationHorariosEnviosCard = _DeviceConfigurationService.GetHoras(setup.Id);
                    foreach (var hora in setup.DeviceConfigurationHorariosEnviosCard)
                    {
                        horario.HorariosEnviosCard.Add(new HorariosEnviosCard()
                        {
                            Hora = hora.Hora != null ? hora.Hora.Substring(0, 5) : ""
                        });
                    }

                    gatilho.Config = Convert.ToInt32(setup.config);
                    gatilho.MaxRmsRed = setup.max_rms_red.HasValue ? 
                        Decimal.ToDouble(setup.max_rms_red.Value) : 0;
                    gatilho.MaxRmsYel = setup.max_rms_yel.HasValue ? 
                        Decimal.ToDouble(setup.max_rms_yel.Value) : 0;
                    gatilho.MinRms = setup.min_rms.HasValue ? 
                        Decimal.ToDouble(setup.min_rms.Value) : 0;
                    gatilho.MaxPercent = setup.max_percent.HasValue ? setup.max_percent.Value : 0;

                    var loraSetupData = _DeviceConfigurationService.GetLast(0,0);
                    lora.Canal = loraSetupData.canal.Value;
                    lora.End = loraSetupData.end.Value;
                    lora.Syn = loraSetupData.syn.Value;
                    lora.Sf = loraSetupData.sf.Value;
                    lora.Bw = loraSetupData.bw.Value;
                    lora.Gtw = loraSetupData.gtw.Value;
                }

                var device = _DeviceService.Get(deviceConfig.Key.Value);
                if (device != null && deviceGlobal.Id == device.Code)
                {
                    response.Sensor.Add(sensor);
                    response.Horarios.Add(horario);
                    response.Gatilhos.Add(gatilho);
                    response.Lora.Add(lora);
                    response.Versao = "1.3.1";
                    response.Sn = device.Code;

                    deviceConfigEnviados.Add(deviceConfigId);
                }
            }

            respJson = JsonConvert.SerializeObject(response);

            ContentResult contRes = Content(Newtonsoft.Json.Linq.JObject.Parse(respJson).ToString(), "application/json");
            contRes.StatusCode = (int)HttpStatusCode.OK;

            foreach (var dcId in deviceConfigEnviados)
            {
                var dc = _DeviceConfigurationService.Get(dcId);
                if (dc != null)
                {
                    dc.config = false;
                    _DeviceConfigurationService.Edit(dc);
                }
            }

            return contRes;
        }

        [HttpPost]
        [Route("deviceData")]
        public ContentResult AddDeviceData([FromBody] DeviceDataRequest deviceData)
        {
            string respJson;
            if (deviceData == null)
            {
                respJson = "{\"Erro Interno\":\"Erro na leitura dos dados recebidos - Estrutura vazia\"}";
                ContentResult contRes1 = Content(Newtonsoft.Json.Linq.JObject.Parse(respJson).ToString(), "application/json");
                contRes1.StatusCode = (int)HttpStatusCode.InternalServerError;
                return contRes1;
            }

            var path = "C:\\Log\\data.txt";
            var text = String.Format("{0}: {1}", DateTime.Now, JsonConvert.SerializeObject(deviceData));

            if (!System.IO.File.Exists(path))
            {
                var createFile = System.IO.File.Create(path);
                createFile.Close();
            }

            System.IO.File.AppendAllText(path, text + Environment.NewLine);

            respJson = "{\"Message\":\"Dados recebidos com sucesso!\"}";
            ContentResult contRes = Content(Newtonsoft.Json.Linq.JObject.Parse(respJson).ToString(), "application/json");
            contRes.StatusCode = (int)HttpStatusCode.OK;
            return contRes;
        }

        #region abc
        [AllowAnonymous]
        [HttpGet("deviceById")]
        public Motor GetDeviceById(int Id)
        {
            return _MotorService.Get(Id);
        }

        [AllowAnonymous]
        [HttpPost("createDevice")]
        public ContentResult AddDevice(MotorModel motor)
        {
            if (motor.CompressorTypeId == 0)
                motor.CompressorTypeId = 1;

            try
            {
                var Motor = _mapper.Map<Motor>(motor);
                Motor.Id = _MotorService.GetlastCode();
                //Motor.Bushing = 
                var idMotor = _MotorService.Insert(Motor);


                var respJson = "{\"Message\":\"Equipamento Cadastrado com sucesso\",\"Id\":\"" + idMotor + "\"}";
                ContentResult contRes1 = Content(Newtonsoft.Json.Linq.JObject.Parse(respJson).ToString(), "application/json");
                contRes1.StatusCode = (int)HttpStatusCode.OK;
                return contRes1;
            }
            catch (Exception e)
            {
                var respJson = "{\"Message\":\"Erro ao adicionar Equipamento\",\"Erro\":\"" + e.Message.ToString() + ":" + e.InnerException.Message.ToString() + "\"}";
                ContentResult contRes1 = Content(Newtonsoft.Json.Linq.JObject.Parse(respJson).ToString(), "application/json");
                contRes1.StatusCode = (int)HttpStatusCode.InternalServerError;
                return contRes1;
            }
        }

        // GET: api/<MembersController>
        [AllowAnonymous]
        [HttpGet("getDetailDevices")]
        public IEnumerable<Motor> GetDetailDevices()
        {
            return _MotorService.GetAll();
        }

        //// GET: api/<MembersController>
        //[AllowAnonymous]
        //[HttpGet("GetMachine")]
        //public List<SelectListItemDTO> GetActuationType()
        //{
        //    return _MachineService.GetQueryDropDownList();
        //}

        // GET: api/<MembersController>
        [AllowAnonymous]
        [HttpGet("getMachine")]
        public List<SelectListItemDTO> GetMachine()
        {
            return _MachineService.GetQueryDropDownList();
        }

        // GET: api/<MembersController>
        [AllowAnonymous]
        [HttpGet("getFixationType")]
        public List<SelectListItemDTO> GetFixationType()
        {
            return _FixationService.GetQueryDropDownList();
        }

        // GET: api/<MembersController>
        [AllowAnonymous]
        [HttpGet("getCoupling")]
        public List<SelectListItemDTO> GetCoupling()
        {
            return _CouplingService.GetQueryDropDownList();
        }

        // GET: api/<MembersController>
        [AllowAnonymous]
        [HttpGet("getCardanShaftType")]
        public List<SelectListItemDTO> GetCardanShaftType()
        {
            return _CardanShaftTypeService.GetQueryDropDownList();
        }


        // GET: api/<MembersController>
        [AllowAnonymous]
        [HttpGet("getPumpType")]
        public List<SelectListItemDTO> GetPumpType()
        {
            return _PumpTypeService.GetQueryDropDownList();
        }


        // GET: api/<MembersController>
        [AllowAnonymous]
        [HttpGet("getCompressorType")]
        public List<SelectListItemDTO> GetCompressorType()
        {
            return _CompressorTypeService.GetQueryDropDownList();
        }

        // GET: api/<MembersController>
        [AllowAnonymous]
        [HttpGet("getDevice")]
        public List<SelectListItemDTO> GetDevice()
        {
            return _DeviceService.GetQueryDropDownList();
        }

        // GET: api/<MembersController>
        [AllowAnonymous]
        [HttpGet("getMotor")]
        public List<SelectListItemDTO> GetMotor()
        {
            List<SelectListItemDTO> listMotor = new List<SelectListItemDTO>
            {
                new SelectListItemDTO(){Key =  1 , Value = "Motor" },
                new SelectListItemDTO() { Key = 2,Value = "Redutores" },
                new SelectListItemDTO() {  Key = 3, Value = "Bomba"},
                new SelectListItemDTO() {  Key = 4,Value = "Ventilador / Exaustor"},
                new SelectListItemDTO(){Key = 5,Value = "Compressor"}
            };

            return listMotor;
        }

        //[AllowAnonymous]
        //// GET: api/<MembersController>
        //[HttpGet]
        //public IEnumerable<Motor> AllMembers()
        //{
        //    ////return lstMember;
        //    return _MotorService.GetAll();
        //}

        //// GET: api/<DeviceController>
        //[AllowAnonymous]
        //[HttpGet]
        //[Produces("application/json")]
        //public ContentResult GetA()
        //{
        //    string respJson = "{\"Status\":\"Active\",\"Connection\":\"Ok\"}";
        //    ContentResult contRes1 = Content(Newtonsoft.Json.Linq.JObject.Parse(respJson).ToString(), "application/json");
        //    return contRes1;
        //}


        //// GET api/<MembersController>/5
        //[HttpGet("{id}")]
        //public Member MemberByid(int id)
        //{
        //    return lstMember.Find(x => x.Id == id);
        //}

        [AllowAnonymous]
        // POST api/<MembersController>
        [HttpPost("authentication")]
        public IActionResult Authentication([FromBody] UserCredential userCredential)
        {
            var token = _jwtAuth.Authentication(userCredential.UserName, userCredential.Password);
            if (token == null)
                return Unauthorized();
            return Ok(token);
        }
        #endregion
    }
}
