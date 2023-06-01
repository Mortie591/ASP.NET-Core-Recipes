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

        public HomeController(ILogger<HomeController> logger, IDataImportService dataImportService)
        {
            _logger = logger;
            this.dataImportService = dataImportService;
        }

        public IActionResult Index()
        {
            //this.dataImportService.ImportRecipes();
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