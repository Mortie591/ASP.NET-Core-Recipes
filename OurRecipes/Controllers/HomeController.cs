using Microsoft.AspNetCore.Mvc;
using OurRecipes.Data.Models;
using OurRecipes.Models;
using OurRecipes.Services;
using System.Diagnostics;
using System.Web;

namespace OurRecipes.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRecipeService recipeService;

        public HomeController(ILogger<HomeController> logger, IRecipeService recipeService)
        {
            _logger = logger;
            this.recipeService = recipeService;
        }

        public IActionResult Index()
        {
            //this.dataImportService.ImportRecipes();
            //this.scraperService.PopulateData();
           
            var recipes  = recipeService.GetLatest();
            return View(recipes);
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