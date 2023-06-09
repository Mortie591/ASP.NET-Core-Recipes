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
            //TODO: Redirect to Recipe info page
            //this.recipeService.Add(input);
            return this.Redirect("/");
        }
    }
}
