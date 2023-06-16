using OurRecipes.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace OurRecipes.Models.Recipes
{
    public class EditRecipeViewModel:CreateRecipeInputModel
    {
        public EditRecipeViewModel()
        {
        }

        [Required]
        public string Id { get; set; }
    }
}
