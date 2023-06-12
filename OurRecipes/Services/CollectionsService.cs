using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using OurRecipes.Data;
using OurRecipes.Models;

namespace OurRecipes.Services
{
    public class CollectionsService : ICollectionsService
    {
        private readonly ApplicationDbContext context;

        public CollectionsService(ApplicationDbContext db)
        {
            this.context = db;
        }
        public ICollection<CollectionCardViewModel> DiscoverRecipes()
        {

            var categories = this.context.Categories
                .Include(x=>x.Recipes)
                .GroupBy(x=>x.Type)
                .Select(x => new CollectionCardViewModel
                {
                    Name = x.Key=="Cuisine"?"International Cuisine":x.Key,
                    RecipeCount =x.SelectMany(x=>x.Recipes).Count()
                })
                .ToList();
            return categories;
        }

        public void GetFavouriteRecipes()
        {
            throw new NotImplementedException();
        }

        public void GetMealCourses()
        {
            throw new NotImplementedException();
        }

        public void GetMyRecipes()
        {
            throw new NotImplementedException();
        }
    }
}
