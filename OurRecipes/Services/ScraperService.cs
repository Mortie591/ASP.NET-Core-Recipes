using AngleSharp;
using OurRecipes.Services.Models;
using IConfiguration = AngleSharp.IConfiguration;

namespace OurRecipes.Services
{
    public class ScraperService 
    {
        //private readonly IConfiguration config;
        //private readonly IBrowsingContext context;
        //private readonly string sourcePath = "../../../SourceData/"; 

        
        public ScraperService()
        {
            //this.config = Configuration.Default.WithDefaultLoader();
            //this.context = BrowsingContext.New(config);
        }

        //public RecipeDto GetRecipesFromFile(string fileName)
        //{
        //    string path = "../../../SourceData";
        //    DirectoryInfo dir = new DirectoryInfo(path);
        //    FileInfo[] filesInSourceDir = dir.GetFiles();
        //    foreach (var file in filesInSourceDir)
        //    {
        //        Console.WriteLine(file.Name);
        //    }
        //    var recipe = new RecipeDto();
        //    return recipe;
        //}

        public void PopulateDbWithRecipes()
        {
            throw new NotImplementedException();
        }
         
    //    // IScraperService.ScrapeRecipes()
    //    {
    //        //"https://www.allrecipes.com/recipes/16679/salad/coleslaw/vinegar-coleslaw/"
    //        //Recipe name
    //        //Category
    //        //Description
    //        //CookTime
    //        //PrepTime
    //        //ServingsCount
    //        //Ingredients
    //        //Instructions
    //        //Nutrients
    //        //Tags

    //        throw new NotImplementedException();
    //}


    //private void GetRecipe(int id, string name)
    //    {
            
    //        throw new NotImplementedException();
    //    }
    }
}
