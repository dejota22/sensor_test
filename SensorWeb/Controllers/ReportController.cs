using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using SensorWeb.Models;
using SelectPdf;
using System.Threading.Tasks;

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

        [HttpPost]
        public async Task<IActionResult> Create(ReportModel report)
        {
            var reportView = await this.RenderViewToStringAsync("DownloadPDF", report);

            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();


            converter.Options.CssMediaType = HtmlToPdfCssMediaType.Print;
            //converter.Options.ViewerPreferences.FitWindow = true;
            converter.Options.ViewerPreferences.CenterWindow = true;
            converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

            PdfDocument doc = converter.ConvertHtmlString(reportView);

            byte[] pdf = doc.Save();
            doc.Close();

            FileResult fileResult = new FileContentResult(pdf, "application/pdf")
            {
                FileDownloadName = $"{report.NomeArquivo}.pdf"
            };

            return fileResult;
        }
    }
}
