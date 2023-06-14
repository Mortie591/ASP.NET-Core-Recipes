using OurRecipes.Models;

namespace OurRecipes.Services
{
    public interface ICategoryService
    {
        
        
        public ICollection<CollectionCardViewModel> DiscoverRecipes();
        public ICollection<CollectionCardViewModel> GetCategoriesByType(string typeName);
        
        
    }
}
