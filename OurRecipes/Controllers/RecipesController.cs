using Microsoft.AspNetCore.Mvc;
using OurRecipes.Models.Recipes;

namespace OurRecipes.Controllers
{
    public class RecipesController:Controller
    {
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
            return this.Redirect("/");
        }
    }
}
