using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OurRecipes.Data;
using OurRecipes.Data.Models;
using OurRecipes.Models.Recipes;
using OurRecipes.Services;

namespace OurRecipes.Controllers
{
    public class RecipesController:Controller
    {
        private readonly IRecipeService recipeService;
        private readonly UserManager<AppIdentityUser> userManager;

        public RecipesController(IRecipeService recipeService, UserManager<AppIdentityUser> userManager)
        {
            this.recipeService = recipeService;
            this.userManager = userManager;
        }
        public IActionResult Details(string name)
        {
            var recipe = recipeService.GetRecipeByName(name);
            return this.View(recipe);
        }
        public IActionResult ByCategory(string categoryName)
        {
            var recipes = this.recipeService.GetRecipesByCategory(categoryName);
            return this.View(recipes);
        }
        
        public IActionResult ByIngredients(string ingredientsInput)
        {
            string[] ingredients = ingredientsInput.Split(',',StringSplitOptions.RemoveEmptyEntries);
            if(ingredients.Length != 0)
            {
                var recipes = this.recipeService.GetRecipesByIngredients(ingredients);
                return this.View(recipes);
            }
            else
            {
                return Redirect("/");
            } 

        }
        public IActionResult ByUser(string userName)
        {
            return this.View(userName);
        }
        public IActionResult MyRecipes()
        {
            string userId = userManager.GetUserId(User);
            var recipes = recipeService.GetMyRecipes(userId);
            return this.View(recipes);
        }
        [Authorize]
        public IActionResult Create()
        {
            var inputModel = new CreateRecipeInputModel();
            
            return this.View(inputModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateRecipeInputModel input)
        {
            if (ModelState.IsValid)
            {
                return this.View(input);
            }
            input.Author = userManager.GetUserId(User);
            this.recipeService.Add(input);
            return RedirectToAction("Details","Recipes",new {input.Title});
        }

        public async Task<IActionResult> Like() //Add to favourites
        {
            return this.View();
        }

        public IActionResult Unlike() //remove from favourites
        {
            return View();
        }


    }
}
