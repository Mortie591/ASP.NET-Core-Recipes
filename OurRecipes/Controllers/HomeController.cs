using Microsoft.AspNetCore.Mvc;
using OurRecipes.Models;
using OurRecipes.Services;
using System.Diagnostics;

namespace OurRecipes.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDataImportService dataImportService;
        private readonly IScraperService scraperService;

        public HomeController(ILogger<HomeController> logger, IDataImportService dataImportService, IScraperService scraperService)
        {
            _logger = logger;
            this.dataImportService = dataImportService;
            this.scraperService = scraperService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}