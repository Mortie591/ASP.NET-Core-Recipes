using OurRecipes.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace OurRecipes.Models.Recipes
{
    public class RecipeViewModel
    {
        public RecipeViewModel()
        {
            Categories = new List<string>();
            Sections = new List<Section>();
            Components = new List<Component>();
            Nutrients = new List<Nutrient>();
            Instructions = new List<string>();
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Servings { get; set; }
        public string PrepTime { get; set; }
        public string CookTime { get; set; }
        public string? Difficulty { get; set; }
        public string ImageUrl { get; set; }
        public int Rating { get; set; }
        public List<string> Categories { get; set; } 
        public List<Section>? Sections { get; set; }
        public List<Component> Components { get; set; }
        public List<string> Instructions { get; set; }
        public List<Nutrient> Nutrients { get; set; }
        public string Author { get; set; }
    }
}
