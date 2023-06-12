using OurRecipes.Data.Models;

namespace OurRecipes.Models.Recipes
{
    public class RecipeCardViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string imageUrl { get; set; }
        public ushort Rating { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}
