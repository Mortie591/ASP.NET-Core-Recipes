using Microsoft.AspNetCore.Mvc;
using OurRecipes.Models;
using OurRecipes.Services;
using System.Diagnostics;
using System.Web;

namespace OurRecipes.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //private readonly IDataImportService dataImportService;
        //private readonly IScraperService scraperService;
        private readonly IRecipeService recipeService;

        public HomeController(ILogger<HomeController> logger, IRecipeService recipeService)
        {
            _logger = logger;
            //this.dataImportService = dataImportService;
            //this.scraperService = scraperService;
            this.recipeService = recipeService;
        }

        public IActionResult Index()
        {
            var recipes  = recipeService.GetRandomRecipes();
            var recipeCards  = new List<RecipeCardViewModel>();
            foreach (var recipe in recipes)
            {
                RecipeCardViewModel viewModel = new RecipeCardViewModel
                {

                    Title = HttpUtility.HtmlDecode(recipe.Title),
                    Rating = recipe.Likes,
                    imageUrl = recipe.ImageUrl
                };
                recipeCards.Add(viewModel);

            }
            return View(recipeCards);
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