using OurRecipes.Data.Models;

namespace OurRecipes.Models.Recipes
{
    public class RecipeByUserViewModel
    {
        public string Id { get; set; }
        public string AuthorName { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public ushort Rating { get; set; }
    }
}
