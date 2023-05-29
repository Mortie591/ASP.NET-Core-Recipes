using Recipes.DataImport.Models;
using System.Text.Json;

namespace Recipes.DataImport
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = "../../../SourceData";
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] filesInSourceDir = dir.GetFiles();
            var recipesDto = new List<RecipeDto>();
            foreach (var file in filesInSourceDir)
            {
                var input = File.ReadAllText(file.FullName);
                
                var recipes = JsonSerializer.Deserialize<ICollection<RecipeDto>>(input);
                foreach(var recipe in recipes)
                {
                    recipesDto.Add(recipe);
                }
            }
            var recipeDto = recipesDto.Where(x=>x.Description!=string.Empty).FirstOrDefault();
        }
    }
}