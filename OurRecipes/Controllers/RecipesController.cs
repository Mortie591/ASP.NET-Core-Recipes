using Microsoft.AspNetCore.Mvc;
using OurRecipes.Data;
using OurRecipes.Models.Recipes;
using OurRecipes.Services;

namespace OurRecipes.Controllers
{
    public class RecipesController:Controller
    {
        private readonly IRecipeService recipeService;

        public RecipesController(IRecipeService recipeService )
        {
            this.recipeService = recipeService;
        }
        [Route("/Recipes/{name}")]
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

        public IActionResult Create()
        {
            var viewModel = new CreateRecipeInputModel();
            return this.View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(CreateRecipeInputModel input)
        {
            if(!this.ModelState.IsValid)
            {
                return this.View(input);
            }
            this.recipeService.Add(input);
            //TODO: Redirect to Recipe info page
            return this.Redirect("/");
        }
    }
}
