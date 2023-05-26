using OurRecipes.Services.Models;

namespace OurRecipes.Services
{
    public interface IScraperService
    {
        //TODO: Add Logic for recipes scraping using AngleSharp
        //public RecipeDto ScrapeRecipes();
        public RecipeDto GetRecipesFromFile(string filePath);
        public void PopulateDbWithRecipes();
    }
}
