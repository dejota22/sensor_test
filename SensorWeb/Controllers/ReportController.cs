using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace SensorWeb.Controllers
{
    [Authorize]
    public class ReportController : BaseController
    {
        IMapper _mapper;
        private readonly IStringLocalizer<Resources.CommonResources> _localizer;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="localizer"></param>
        public ReportController(IMapper mapper,
                                  IStringLocalizer<Resources.CommonResources> localizer)
        {
            _mapper = mapper;
            _localizer = localizer;
        }

        // GET: ReportController
        public ActionResult Index()
        {
            return View();
        }       
    }
}
