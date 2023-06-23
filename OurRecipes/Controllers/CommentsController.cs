using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OurRecipes.Data.Models;
using OurRecipes.Services;

namespace OurRecipes.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly IRecipeService recipeService;
        private readonly UserManager<AppIdentityUser> userManager;

        public CommentsController(IRecipeService recipeService, UserManager<AppIdentityUser> userManager) 
        {
            this.recipeService = recipeService;
            this.userManager = userManager;
        }
        
        public IActionResult Comment()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddComment()
        {
            return View();
        }
        public IActionResult RemoveComment()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reply()
        {
            return View();
        }
        
        public IActionResult RemoveReply()
        {
            return View();
        }
    }
}
