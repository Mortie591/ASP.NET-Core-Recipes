using Microsoft.AspNetCore.Mvc;
using OurRecipes.Services;

namespace OurRecipes.Controllers
{
    public class CollectionsController:Controller
    {
        private readonly IRecipeService recipeService;

        public CollectionsController(IRecipeService recipeService)
        {
            this.recipeService = recipeService;
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
            return this.View();
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
