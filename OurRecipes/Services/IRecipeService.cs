using OurRecipes.Data.Models;

namespace OurRecipes.Services
{
    public interface IRecipeService
    {
        public Recipe GetRecipeById(string id);
        public Recipe GetRecipeByName(string name);
        public Recipe GetRandomRecipe();
        public ICollection<Recipe> GetRecipesByIngredient(string ingredientName);
        public ICollection<Recipe> GetRecipesByTag(string tagType);
        public ICollection<Recipe> GetRecipesByCategory(string categoryName);
        public ICollection<Recipe> GetLatest(string categoryName);
        public ICollection<Recipe> GetTrending(string categoryName);
        public void Add();
        public void Remove();
       
    }
}
