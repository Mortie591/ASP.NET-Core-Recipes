using OurRecipes.Data.Models;
using OurRecipes.Models.Recipes;

namespace OurRecipes.Services
{
    public interface IRecipeService
    {
        public Recipe GetRecipeById(string id);
        public Recipe GetRecipeByName(string name);
        public RecipeViewModel GetRecipeViewModel(string id);
        public ICollection<RecipeCardViewModel> GetRandomRecipes();
        public ICollection<RecipeCardViewModel> GetRecipesByIngredients(params string[] ingredients);
        public ICollection<RecipeCardViewModel> GetRecipesByCategory(string categoryName);
        public ICollection<RecipeCardViewModel> GetLatest();
        public ICollection<RecipeCardViewModel> GetTrending();
        public void Add(CreateRecipeInputModel recipeDto, string authorId);
        public EditRecipeViewModel GetEditData(string id);
        public void Edit(EditRecipeViewModel recipeData);
        public ICollection<RecipeCardViewModel> GetMyRecipes(string userId);
        public Task<ICollection<RecipeByUserViewModel>> GetRecipesByUserAsync(string userId);
        public ICollection<RecipeByUserViewModel> GetFavouriteRecipes(string userId);
        public void Delete(string id);
        public Task LikeRecipe(string id, string userId);
        public Task UnlikeRecipe(string id, string userId);

    }
}
