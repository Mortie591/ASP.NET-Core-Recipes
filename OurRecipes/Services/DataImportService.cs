using OurRecipes.Data;
using OurRecipes.Data.Models;
using OurRecipes.Services.Models.ImportDtos;
using System.Text;
using System.Text.Json;
using Component = OurRecipes.Data.Models.Component;

namespace OurRecipes.Services
{
    public class DataImportService : IDataImportService
    {
        private readonly ApplicationDbContext context;
        private readonly List<Recipe> recipes = new List<Recipe>();
        private readonly List<Unit> units = new List<Unit>();
        private readonly List<Tag> tags = new List<Tag>();
        private readonly List<Nutrient> nutrients = new List<Nutrient>();
        private readonly List<Ingredient> ingredients = new List<Ingredient>();
        private readonly List<Category> categories = new List<Category>();
        public DataImportService(ApplicationDbContext db)
        {
            this.context = db;
        }
        public void ImportRecipes()
        {
            //this.context.Database.EnsureDeleted();
            //this.context.Database.EnsureCreated();
            //this.context.SaveChanges();

            string path = "Services/SourceData";
            ICollection<RecipeDto> recipesDto = DeserializeDataFromJSON(path);
            

            foreach (RecipeDto recipeDto in recipesDto)
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
                    Sections = CreateSections(recipeDto),
                    Instructions = InstructionsAsString(recipeDto),
                    Nutrients = recipeDto.Nutritients.Select(x => GetOrCreateNutrient(x.name, x.quantity)).ToList(),
                    Tags = recipeDto.Tags.Select(x => GetOrCreateTag(x.Name, x.Type)).ToList()
                };
                recipe.Components = GetOrCreateComponents(recipe.Sections);

                if (recipe.Components.Any(c => c.Quantity == null))
                {
                    Console.WriteLine($"{recipe.Title} is invalid");
                    continue;
                }
                else
                {
                    if (!this.context.Recipes.Select(x => x.Title).Contains(recipe.Title))
                    {
                        this.recipes.Add(recipe);
                    }
                }
            }

            this.context.Units.AddRangeAsync(this.units);
            this.context.Tags.AddRangeAsync(this.tags);
            this.context.Nutrients.AddRangeAsync(this.nutrients);
            this.context.Ingredients.AddRangeAsync(this.ingredients);
            this.context.Categories.AddRangeAsync(this.categories);
            this.context.Recipes.AddRangeAsync(this.recipes);

            this.context.SaveChanges();
        }
        private ICollection<Section> CreateSections(RecipeDto recipe)
        {
            var sections = recipe.Sections;
            var recipeSections = new HashSet<Section>();
            foreach(SectionDto section in sections)
            {
                var sectionDtoComponents = section.Components;
                var sectionComponents = new List<Component>();
                string sectionName = section.Name;
                foreach(var component in sectionDtoComponents)
                {
                    var measurements = component.measurements.Where(x=>!string.IsNullOrEmpty(x.quantity)).FirstOrDefault();
                    var quantity = measurements!=null?measurements.quantity:"0";
                    var unitName = measurements!=null? measurements.unit.abbreviation:"";
                    var ingredient = GetOrCreateIngredient(component.Ingredient.Name, component.Ingredient.NamePlural);
                    var currentComponent = new Component
                    {
                        Text = component.Text,
                        Ingredient = ingredient,
                        Quantity = string.IsNullOrEmpty(quantity)?"0":quantity,
                        Unit = GetOrCreateUnit(unitName)
                    };
                    sectionComponents.Add(currentComponent);
                }
                var recipeSection = new Section
                {
                    Name = sectionName,
                    Components = sectionComponents
                };
                recipeSections.Add(recipeSection);
            }
            return recipeSections;
        }

        private ICollection<Component> GetOrCreateComponents(ICollection<Section> sections)
        {
            var recipeComponents = new HashSet<Component>();

            foreach (Section section in sections)
            {
                var sectionComponents = section.Components;

                foreach (var component in sectionComponents)
                {
                    recipeComponents.Add(component);
                }
            }
            return recipeComponents;
        }

        private string InstructionsAsString(RecipeDto recipe)
        {
            var instructions = recipe.Instructions;
            StringBuilder sb = new StringBuilder();
            foreach(var instruction in instructions)
            {
                sb.AppendLine($"{instruction.position} - {instruction.display_text}");
            }
            return sb.ToString().Trim();
        }

        //Check for existing records before adding them to DB
        private Unit GetOrCreateUnit(string unitName)
        {
            var unit = this.units.FirstOrDefault(x => string.Equals(x.Name, unitName));
            if (unit == null && !String.IsNullOrEmpty(unitName))
            {
                unit = new Unit { Name = unitName };
                this.units.Add(unit);
            }
            return unit;
        }
        private Tag GetOrCreateTag(string tagName, string type)
        {
            var tag = this.tags.FirstOrDefault(x => string.Equals(x.Name, tagName)
            && string.Equals(x.Type, type));
            if (tag == null)
            {
                tag = new Tag { Name = tagName, Type = type };
                this.tags.Add(tag);
            }
            return tag;
        }
        private Nutrient GetOrCreateNutrient(string nutrientName, string quantity)
        {
            var nutrient = this.nutrients.FirstOrDefault(x => string.Equals(x.Name, nutrientName) && x.Quantity == quantity);
            if (nutrient == null)
            {
                nutrient = new Nutrient { Name = nutrientName, Quantity = quantity };
                this.nutrients.Add(nutrient);
            }
            return nutrient;
        }
        private Nutrient GetOrCreateNutrient(string nutrientName, string quantity, string unitName)
        {
            var nutrient = this.nutrients.FirstOrDefault(x => string.Equals(x.Name, nutrientName) && x.Quantity == quantity);
            if (nutrient == null)
            {
                nutrient = new Nutrient { Name = nutrientName, Quantity = quantity, Unit = GetOrCreateUnit(unitName) };
                this.nutrients.Add(nutrient);
            }
            return nutrient;
        }
        private Ingredient GetOrCreateIngredient(string ingredientName, string pluralName)
        {
            var ingredient = this.ingredients.FirstOrDefault(x => string.Equals(x.Name, ingredientName));
            if (ingredient == null)
            {
                ingredient = new Ingredient { Name = ingredientName, NamePlural = pluralName };
                this.ingredients.Add(ingredient);
            }
            return ingredient;
        }
        private Category GetOrCreateCategory(string categoryName)
        {
            var category = this.categories.FirstOrDefault(x => string.Equals(x.Name, categoryName));
            if (category == null)
            {
                category = new Category { Name = categoryName };
                this.categories.Add(category);
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
