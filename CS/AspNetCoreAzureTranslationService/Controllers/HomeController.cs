using AspNetCoreAzureTranslationService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AspNetCoreAzureTranslationService.Controllers {
    public class HomeController : Controller {
        private readonly IAzureTranslationService _translationService;
        public HomeController(IAzureTranslationService translationService) {
            _translationService = translationService;
        }

        public IActionResult Index() {
            return View();
        }

        public IActionResult Designer() {
            return View();
        }

        public IActionResult Viewer() {
            return View();
        }

        public async Task<ActionResult> GetAzureTranslationService([FromBody] AzureTranslationData data) {            
            return Content(await _translationService.TranslationTextRequest(data), "application/json");
        }
    }
}
