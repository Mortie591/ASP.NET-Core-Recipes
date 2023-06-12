using Microsoft.AspNetCore.Mvc;
using OurRecipes.Services;

namespace OurRecipes.Controllers
{
    public class CollectionsController:Controller
    {
        private readonly IRecipeService recipeService;
        private readonly ICollectionsService collectionsService;

        public CollectionsController(IRecipeService recipeService, ICollectionsService collectionsService)
        {
            this.recipeService = recipeService;
            this.collectionsService = collectionsService;
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
            var categories = this.collectionsService.DiscoverRecipes();
            return this.View(categories);
        }

        public IActionResult Healthy()
        {
            return this.View();
        }

        public IActionResult Breakfast()
        {
            return this.View();
        }
        public IActionResult Lunch()
        {
            return this.View();
        }
        public IActionResult Dinner()
        {
            return this.View();
        }
        public IActionResult Snacks()
        {
            return this.View();
        }
        public IActionResult Desserts()
        {
            return this.View();
        }
        public IActionResult Drinks()
        {
            return this.View();
        }
        public IActionResult Cuisine()
        {
            return this.View();
        }

        public IActionResult Easy()
        {
            return this.View();
        }

    }
}
