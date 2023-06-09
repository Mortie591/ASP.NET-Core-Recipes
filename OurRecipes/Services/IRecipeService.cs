using OurRecipes.Data.Models;
using OurRecipes.Models.Recipes;

namespace OurRecipes.Services
{
    public interface IRecipeService
    {
        public RecipeViewModel GetRecipeById(string id);
        public RecipeViewModel GetRecipeByName(string name);
        public ICollection<RecipeCardViewModel> GetRandomRecipes();
        public ICollection<RecipeCardViewModel> GetRecipesByIngredient(string ingredientName);
        public ICollection<RecipeCardViewModel> GetRecipesByCategory(string categoryName);
        public ICollection<RecipeCardViewModel> GetLatest();
        public ICollection<RecipeCardViewModel> GetTrending();
        public void Add(CreateRecipeInputModel recipeDto);
        public void Remove(string id);
       
    }
}
