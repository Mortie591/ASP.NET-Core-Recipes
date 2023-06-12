using Microsoft.AspNetCore.Mvc;
using OurRecipes.Services;

namespace OurRecipes.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IRecipeService recipeService;
        private readonly ICategoryService categoryService;

        public CategoriesController(IRecipeService recipeService, ICategoryService categoryService)
        {
            this.recipeService = recipeService;
            this.categoryService = categoryService;
        }

        public IActionResult MyRecipes()
        {
            return this.View();
        }

        public IActionResult FavouriteRecipes()
        {
            return this.View();
        }

        public IActionResult Discover()
        {
            var categories = this.categoryService.DiscoverRecipes();
            return this.View(categories);
        }
        public IActionResult ByType(string categoryType)
        {
            var categories = this.categoryService.GetCategoriesByType(categoryType);
            return this.View(categories);
        }

    }
}
