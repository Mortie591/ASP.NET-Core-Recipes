using OurRecipes.Data.Models;
using OurRecipes.Models;

namespace OurRecipes.Services
{
    public interface IRecipeService
    {
        public Recipe GetRecipeById(string id);
        public Recipe GetRecipeByName(string name);
        public ICollection<RecipeCardViewModel> GetRandomRecipes();
        public ICollection<Recipe> GetRecipesByIngredient(string ingredientName);
        public ICollection<Recipe> GetRecipesByTag(string tagType);
        public ICollection<Recipe> GetRecipesByCategory(string categoryName);
        public ICollection<Recipe> GetLatest();
        public ICollection<Recipe> GetTrending(string categoryName);
        public void Add();
        public void Remove();
       
    }
}
