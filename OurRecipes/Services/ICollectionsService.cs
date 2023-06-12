using OurRecipes.Models;

namespace OurRecipes.Services
{
    public interface ICollectionsService
    {
        public void GetMyRecipes();
        public void GetFavouriteRecipes();
        public void GetMealCourses();
        public ICollection<CollectionCardViewModel> DiscoverRecipes();
        
    }
}
