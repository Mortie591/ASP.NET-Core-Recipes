using OurRecipes.Models;

namespace OurRecipes.Services
{
    public interface ICategoryService
    {
        public void GetMyRecipes();
        public void GetFavouriteRecipes();
        
        public ICollection<CollectionCardViewModel> DiscoverRecipes();
        public ICollection<CollectionCardViewModel> GetCategoriesByType(string typeName);
        
        
    }
}
