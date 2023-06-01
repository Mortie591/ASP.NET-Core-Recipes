using OurRecipes.Data;
using OurRecipes.Data.Models;

namespace OurRecipes.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly ApplicationDbContext context;
        public RecipeService(ApplicationDbContext db)
        {
            this.context = db;
        }
        public void Add()
        {
            throw new NotImplementedException();
        }

        public ICollection<Recipe> GetLatest(string categoryName)
        {
            throw new NotImplementedException();
        }

        public Recipe GetRandomRecipe()
        {
            var recipe = this.context.Recipes.OrderBy(x => Guid.NewGuid()).First();
            return recipe;
        }

        public Recipe GetRecipeById(string id)
        {
            throw new NotImplementedException();
        }

        public Recipe GetRecipeByName(string name)
        {
            throw new NotImplementedException();
        }

        public ICollection<Recipe> GetRecipesByCategory(string categoryName)
        {
            throw new NotImplementedException();
        }

        public ICollection<Recipe> GetRecipesByIngredient(string ingredientName)
        {
            throw new NotImplementedException();
        }

        public ICollection<Recipe> GetRecipesByTag(string tagType)
        {
            throw new NotImplementedException();
        }

        public ICollection<Recipe> GetTrending(string categoryName)
        {
            throw new NotImplementedException();
        }

        public void Remove()
        {
            throw new NotImplementedException();
        }
    }
}
