using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using OurRecipes.Data;
using OurRecipes.Models;

namespace OurRecipes.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext context;

        public CategoryService(ApplicationDbContext db)
        {
            this.context = db;
        }
        public ICollection<CollectionCardViewModel> DiscoverRecipes()
        {

            var categories = this.context.Categories
                .GroupBy(x => x.Type)
                .Select(x => new CollectionCardViewModel
                {
                    Name = x.Key == "Cuisine" ? "International Cuisine" : x.Key,
                    RecipeCount = x.SelectMany(x => x.Recipes).Count()
                })
                .ToList();
            return categories;
        }

        public ICollection<CollectionCardViewModel> GetCategoriesByType(string typeName)
        {
            var categories = this.context.Categories
                .Where(x => x.Type == typeName)
                .Select(x => new CollectionCardViewModel
                {
                    Name = x.Name,
                    ImageUrl = x.imageUrl,
                    RecipeCount = x.Recipes.Count()
                })
                .ToList();
            return categories;
        }
        
    }
}
