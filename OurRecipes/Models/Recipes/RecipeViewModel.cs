using OurRecipes.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace OurRecipes.Models.Recipes
{
    public class RecipeViewModel
    {
        public RecipeViewModel()
        {
            Categories = new List<Category>();
            Sections = new List<Section>();
            Components = new List<Component>();
            Nutrients = new List<Nutrient>();
        }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Servings { get; set; }
        public int? PrepTime { get; set; }
        public int? CookTime { get; set; }
        public string ImageUrl { get; set; }
        public List<Category> Categories { get; set; } 
        public List<Section>? Sections { get; set; }
        public List<Component> Components { get; set; }
        public string Instructions { get; set; }
        public List<Nutrient> Nutrients { get; set; }
    }
}
