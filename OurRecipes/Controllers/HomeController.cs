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
            var recipes = new List<RecipeCardViewModel>();
            for (int i = 0; i < 6; i++)
            {
                var recipe = recipeService.GetRandomRecipe();
                RecipeCardViewModel viewModel = new RecipeCardViewModel
                {
                    Title = HttpUtility.HtmlDecode(recipe.Title),
                    Rating = recipe.Likes,
                    imageUrl = recipe.ImageUrl
                };
                recipes.Add(viewModel);

            }
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