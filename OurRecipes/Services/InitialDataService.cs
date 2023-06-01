using AngleSharp.Dom;
using OurRecipes.Data;
using OurRecipes.Data.Models;
using OurRecipes.Services.Models.ImportDtos;
using OurRecipes.Services.Models.ScraperDtos;
using System.Text;

namespace OurRecipes.Services
{
    public abstract class InitialDataService
    {
        protected readonly ApplicationDbContext context;
        protected readonly List<Recipe> recipes = new List<Recipe>();
        protected readonly List<Unit> units = new List<Unit>();
        protected readonly List<Tag> tags = new List<Tag>();
        protected readonly List<Nutrient> nutrients = new List<Nutrient>();
        protected readonly List<Ingredient> ingredients = new List<Ingredient>();
        protected readonly List<Category> categories = new List<Category>();
        public InitialDataService(ApplicationDbContext db)
        {
            this.context = db; 
        }

        protected virtual ICollection<Component> GetOrCreateComponents(ICollection<Section> sections)
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
        protected virtual ICollection<Component> GetOrCreateComponents(ICollection<Models.ScraperDtos.ComponentDto> componentDtos)
        {
            var recipeComponents = new HashSet<Component>();

            foreach (Models.ScraperDtos.ComponentDto comp in componentDtos)
            {
                Component component = new Component
                {
                    Text = comp.Text,
                    Unit = GetOrCreateUnit(comp.Unit),
                    Quantity = comp.Quantity,
                    Ingredient = GetOrCreateIngredient(comp.IngredientName)
                };
                recipeComponents.Add(component);
            }
            return recipeComponents;
        }
        //Check for existing records before adding them to DB
        protected virtual Unit GetOrCreateUnit(string unitName)
        {
            Unit unit = this.context.Units.FirstOrDefault(x => string.Equals(x.Name, unitName));
            if (unit != null)
            {
                return unit;
            }
            unit = this.units.FirstOrDefault(x => string.Equals(x.Name, unitName));
            if (unit == null && !String.IsNullOrEmpty(unitName))
            {
                unit = new Unit { Name = unitName };
                this.units.Add(unit);
            }
            return unit;
        }
        protected virtual Tag GetOrCreateTag(string tagName, string type)
        {
            Tag tag = this.context.Tags.FirstOrDefault(x => string.Equals(x.Name, tagName));
            if (tag != null)
            {
                return tag;
            }
            tag = this.tags.FirstOrDefault(x => string.Equals(x.Name, tagName)
            && string.Equals(x.Type, type));
            if (tag == null)
            {
                tag = new Tag { Name = tagName, Type = type };
                this.tags.Add(tag);
            }
            return tag;
        }
        protected virtual Tag GetOrCreateTag(string tagName)
        {
            var tag = this.context.Tags.FirstOrDefault(x => string.Equals(x.Name, tagName));
            if(tag != null)
            {
                return tag;
            }
            tag = this.tags.FirstOrDefault(x => string.Equals(x.Name, tagName));
            
            if (tag == null)
            {
                tag = new Tag { Name = tagName, Type = tagName };
                this.tags.Add(tag);
            }
            return tag;
        }
        protected virtual Nutrient GetOrCreateNutrient(string nutrientName, string quantity)
        {
            Nutrient nutrient = this.context.Nutrients.FirstOrDefault(x => string.Equals(x.Name, nutrientName));
            if (nutrient != null)
            {
                return nutrient;
            }
            nutrient = this.nutrients.FirstOrDefault(x => string.Equals(x.Name, nutrientName) && x.Quantity == quantity);
            if (nutrient == null)
            {
                nutrient = new Nutrient { Name = nutrientName, Quantity = quantity };
                this.nutrients.Add(nutrient);
            }
            return nutrient;
        }
        protected virtual Nutrient GetOrCreateNutrient(string nutrientName, string quantity, string unitName)
        {
            Nutrient nutrient = this.context.Nutrients.FirstOrDefault(x => string.Equals(x.Name, nutrientName));
            if (nutrient != null)
            {
                return nutrient;
            }
            nutrient = this.nutrients.FirstOrDefault(x => string.Equals(x.Name, nutrientName) && x.Quantity == quantity);
            if (nutrient == null)
            {
                nutrient = new Nutrient { Name = nutrientName, Quantity = quantity, Unit = GetOrCreateUnit(unitName) };
                this.nutrients.Add(nutrient);
            }
            return nutrient;
        }
        protected virtual Ingredient GetOrCreateIngredient(string ingredientName, string pluralName)
        {
            Ingredient ingredient = this.context.Ingredients.FirstOrDefault(x => string.Equals(x.Name, ingredientName));
            if (ingredient != null)
            {
                return ingredient;
            }
            ingredient = this.ingredients.FirstOrDefault(x => string.Equals(x.Name, ingredientName));
            if (ingredient == null)
            {
                ingredient = new Ingredient { Name = ingredientName, NamePlural = pluralName };
                this.ingredients.Add(ingredient);
            }
            return ingredient;
        }
        protected virtual Ingredient GetOrCreateIngredient(string ingredientName)
        {
            Ingredient ingredient = this.context.Ingredients.FirstOrDefault(x => string.Equals(x.Name, ingredientName));
            if (ingredient != null)
            {
                return ingredient;
            }
            ingredient = this.ingredients.FirstOrDefault(x => string.Equals(x.Name, ingredientName));
            if (ingredient == null)
            {
                ingredient = new Ingredient { Name = ingredientName, NamePlural = null };
                this.ingredients.Add(ingredient);
            }
            return ingredient;
        }
        protected virtual Category GetOrCreateCategory(string categoryName)
        {
            Category category = this.context.Categories.FirstOrDefault(x => string.Equals(x.Name, categoryName));
            if (category != null)
            {
                return category;
            }
            category = this.categories.FirstOrDefault(x => string.Equals(x.Name, categoryName));
            if (category == null)
            {
                category = new Category { Name = categoryName };
                this.categories.Add(category);
            }
            return category;
        }
    }
}
 