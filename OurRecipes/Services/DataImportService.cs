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
                DateTime createdOn = !string.IsNullOrEmpty(recipeDto.CreatedOnDate)
               ? DateTimeOffset.FromUnixTimeSeconds(long.Parse(recipeDto.CreatedOnDate)).DateTime
               : DateTime.Now;

                var recipe = new Recipe
                {
                    Title = recipeDto.Title,
                    Description = recipeDto.Description,
                    Servings = (byte)recipeDto.Servings,
                    PrepTime = recipeDto.PrepTime,
                    CookTime = recipeDto.CookTime,
                    ImageUrl = recipeDto.ImageUrl,
                    CreatedOnDate = createdOn,
                    Categories = recipeDto.Categories.Select(x => GetOrCreateCategory(x.name)).ToList(),
                    Sections = recipeDto.Sections.Count > 0 ? recipeDto.Sections.Select(x => new Section
                    {
                        Name = x.Name,
                        Components = x.Components.Select(c => new Component
                        {
                            Text = c.Text,
                            Ingredient = GetOrCreateIngredient(c.Ingredient.Name, c.Ingredient.NamePlural),
                            Quantity = c.measurements
                            .Select(x => x.quantity).FirstOrDefault(),
                            Unit = c.measurements
                            .Select(x => GetOrCreateUnit(x.unit.abbreviation)).FirstOrDefault()
                        }).ToList()
                    }).ToList():null,
                    Instructions = String.Join("|",recipeDto.Instructions),
                    Nutrients = recipeDto.Nutritients.Select(x => GetOrCreateNutrient(x.name,x.quantity)).ToList(),
                    Tags = recipeDto.Tags.Select(x=> GetOrCreateTag(x.Name,x.Type)).ToList()
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

        //Check for existing records before adding them to DB
        private Unit GetOrCreateUnit(string unitName)
        {
            var unit = this.context.Units.FirstOrDefault(x => string.Equals(x.Name,unitName));
            if (unit == null && !String.IsNullOrEmpty(unitName))
            {
                unit = new Unit { Name = unitName };
                this.context.Units.Add(unit);
                this.context.SaveChanges();
            }
            return unit;
        }
        private Tag GetOrCreateTag(string tagName, string type)
        {
            var tag = this.context.Tags.FirstOrDefault(x => string.Equals(x.Name, tagName) 
            && string.Equals(x.Type, type));
            if (tag == null)
            {
                tag = new Tag { Name = tagName, Type = type };
                this.context.Tags.Add(tag);
                this.context.SaveChanges();
            }
            return tag;
        }
        private Nutrient GetOrCreateNutrient(string nutrientName, string quantity)
        {
            var nutrient = this.context.Nutrients.FirstOrDefault(x => string.Equals(x.Name, nutrientName) && x.Quantity==quantity);
            if (nutrient == null)
            {
                nutrient = new Nutrient { Name = nutrientName, Quantity = quantity };
                this.context.Nutrients.Add(nutrient);
                this.context.SaveChanges();
            }
            return nutrient;
        }
        private Nutrient GetOrCreateNutrient(string nutrientName, string quantity, string unitName)
        {
            var nutrient = this.context.Nutrients.FirstOrDefault(x => string.Equals(x.Name, nutrientName) && x.Quantity == quantity);
            if (nutrient == null)
            {
                nutrient = new Nutrient { Name = nutrientName, Quantity = quantity, Unit = GetOrCreateUnit(unitName) };
                this.context.Nutrients.Add(nutrient);
                this.context.SaveChanges();
            }
            return nutrient;
        }
        private Ingredient GetOrCreateIngredient(string ingredientName, string pluralName)
        {
            var ingredient = this.context.Ingredients.FirstOrDefault(x => string.Equals(x.Name, ingredientName));
            if (ingredient == null)
            {
                ingredient = new Ingredient { Name = ingredientName, NamePlural = pluralName };
                this.context.Ingredients.Add(ingredient);
                this.context.SaveChanges();
            }
            return ingredient;
        }
        private Category GetOrCreateCategory(string categoryName)
        {
            var category = this.context.Categories.FirstOrDefault(x=>string.Equals(x.Name, categoryName));
            if(category == null)
            {
                category = new Category { Name = categoryName };
                this.context.Categories.Add(category);
                this.context.SaveChanges();
            }
            return category;
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
