using Microsoft.AspNetCore.Mvc;
using OurRecipes.Models.Recipes;

namespace OurRecipes.Controllers
{
    public class RecipesController:Controller
    {
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateRecipeInputModel input)
        {
            if(!this.ModelState.IsValid)
            {
                return this.View();
            }
            //TODO: Redirect to Recipe info page
            return this.Redirect("/");
        }
    }
}
