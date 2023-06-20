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
        private readonly SignInManager<AppIdentityUser> signInManager;

        public RecipesController(IRecipeService recipeService, UserManager<AppIdentityUser> userManager, SignInManager<AppIdentityUser>signInManager)
        {
            this.recipeService = recipeService;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public IActionResult Details(string id)
        {
            var recipe = recipeService.GetRecipeViewModel(id);
            return this.View(recipe);
        }
        public IActionResult Delete(string id)
        {
            recipeService.Delete(id);
            return RedirectToAction("MyRecipes", "Recipes");
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
        [Authorize]
        public async Task<IActionResult> ByUser(string userName)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user != null)
            {
                var recipes = await recipeService.GetRecipesByUserAsync(user.Id);
                return this.View(recipes);
            }
            else
            {
                return NotFound();
            }
            
        }
        [Authorize]
        public IActionResult MyRecipes()
        {
            string userId = userManager.GetUserId(User);
            var recipes = recipeService.GetMyRecipes(userId);
            return this.View(recipes);
        }
        [Authorize]
        public IActionResult MyFavouriteRecipes()
        {
            string userId = userManager.GetUserId(User);
            if (userId != null)
            {
                var recipes = recipeService.GetFavouriteRecipes(userId);
                return this.View(recipes);
            }
            else
            {
                return NotFound();
            }
           
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
            string authorId = userManager.GetUserId(User);
            if (!ModelState.IsValid)
            {
                return this.View(input);
            }
            this.recipeService.Add(input, authorId);
            return RedirectToAction("Details","Recipes",new {name = input.Title});
        }
        [Authorize]
        public IActionResult Edit(string id)
        {
            var recipe = this.recipeService.GetEditData(id);
            var currentUserId = userManager.GetUserId(User);
            var recipeAuthorId = this.recipeService.GetRecipeById(recipe.Id)?.AuthorId;

            if (currentUserId== recipeAuthorId)
            {
                return this.View(recipe);
            }
            else
            {
                return Forbid();
            }
            
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditRecipeViewModel input)
        {
            var recipeAuthorId = this.recipeService.GetRecipeById(input.Id)?.AuthorId;
            if (!ModelState.IsValid || recipeAuthorId!=userManager.GetUserId(User))
            {
                return this.View(input);
            }

            this.recipeService.Edit(input);
            return RedirectToAction("Details", "Recipes", new  { name=input.Title });
        }
        [Authorize]
        public async Task<IActionResult> Like() //Add to favourites
        {
            return this.View();
        }
        [Authorize] 
        public IActionResult Unlike() //remove from favourites
        {
            return View();
        }

    }
}
