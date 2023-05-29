using OurRecipes.Data;
using OurRecipes.Data.Models;
using OurRecipes.Services.Models.ImportDtos;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Component = OurRecipes.Data.Models.Component;

namespace OurRecipes.Services
{
    public class DataImportService : IDataImportService
    {
        private readonly ApplicationDbContext context;
        public DataImportService(ApplicationDbContext db)
        {
            this.context = db;
        }
        public void ImportRecipes()
        {
            //this.context.Database.EnsureDeleted();
            //this.context.Database.EnsureCreated();

            this.context.SaveChanges();
            string path = "Services/SourceData";
            ICollection<RecipeDto> recipesDto = DeserializeDataFromJSON(path);
            var recipes = new List<Recipe>();
            foreach(RecipeDto recipeDto in recipesDto)
            {
                DateTime createdOn;
                if (!string.IsNullOrEmpty(recipeDto.CreatedOnDate))
                {
                    createdOn = DateTimeOffset.FromUnixTimeSeconds(long.Parse(recipeDto.CreatedOnDate)).DateTime;
                }
                else
                {
                    createdOn = DateTime.Now;
                }
                
                var recipe = new Recipe
                {
                    Title = recipeDto.Title,
                    Description = recipeDto.Description,
                    Servings = (byte)recipeDto.Servings,
                    PrepTime = recipeDto.PrepTime,
                    CookTime = recipeDto.CookTime,
                    ImageUrl = recipeDto.ImageUrl,
                    CreatedOnDate = createdOn,
                    Categories = recipeDto.Categories.Select(x => new Category
                    {
                        Name = x.name
                    }).ToList(),
                    Sections = recipeDto.Sections.Count > 0 ? recipeDto.Sections.Select(x => new Section
                    {
                        Name = x.Name,
                        Components = x.Components.Select(c => new Component
                        {
                            Text = c.Text,
                            Ingredient = new Ingredient
                            {
                                Name = c.Ingredient.Name,
                                NamePlural = c.Ingredient.NamePlural
                            },
                            
                            Quantity = c.measurements
                            .Select(x => x.quantity).FirstOrDefault(),
                            Unit = c.measurements
                            .Select(x => x.unit.abbreviation).FirstOrDefault()
                        }).ToList()
                    }).ToList():null,
                    Instructions = String.Join("|",recipeDto.Instructions),
                    Nutrients = recipeDto.Nutritients.Select(x => new Nutrient
                    {
                        Name = x.name,
                        Quantity = Double.TryParse(x.quantity,out double result)?result:0
                    }).ToList(),
                    Tags = recipeDto.Tags.Select(x=>new Tag
                    {
                        Type = x.Type,
                        Name = x.Name
                    }).ToList()
                };

                recipes.Add(recipe);
            }
            
            foreach (var recipe in recipes)
            {
                if (recipe.Sections.Any(s => s.Components.Any(c => c.Quantity == null)))
                {
                    Console.WriteLine($"{recipe.Title} is invalid");
                    continue;
                }
                else
                {
                    if (!this.context.Recipes.Select(x => x.Title).Contains(recipe.Title))
                    {
                        this.context.Recipes.Add(recipe);
                    }
                }
            }
            
            this.context.SaveChanges();
        }
        private static ICollection<RecipeDto> DeserializeDataFromJSON(string path)
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
                recipe.Sections.Select(x => x.Components.Select(c => new ComponentDto
                {
                    Ingredient = new IngredientDto
                    {
                        Name = c.Ingredient.Name,
                        NamePlural = c.Ingredient.NamePlural
                    },
                    Unit = c.measurements
                            .Select(x => x.unit.abbreviation).FirstOrDefault(),
                    Quantity = c.measurements
                            .Select(x => x.quantity).FirstOrDefault(),

                }));
                    
                    recipesDto.Add(recipe);
                }
            }
            return recipesDto;
        }
    }
}
