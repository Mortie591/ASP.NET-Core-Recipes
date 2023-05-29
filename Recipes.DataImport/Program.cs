using Recipes.DataImport.Models;
using System.Text.Json;

namespace Recipes.DataImport
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ICollection<RecipeDto> recipes =  DeserializeDataFromJSON("../../../SourceData");
        }

        public static ICollection<RecipeDto> DeserializeDataFromJSON(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] filesInSourceDir = dir.GetFiles();
            var recipesDto = new List<RecipeDto>();
            foreach (var file in filesInSourceDir)
            {
                var input = File.ReadAllText(file.FullName);

                var recipes = JsonSerializer.Deserialize<ICollection<RecipeDto>>(input);
                foreach (var recipe in recipes)
                {
                    recipesDto.Add(recipe);
                }
            }
            return recipesDto;
        }
    }
}