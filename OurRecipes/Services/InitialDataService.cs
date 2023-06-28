using OurRecipes.Data;
using OurRecipes.Data.Models;

namespace OurRecipes.Services
{
    public abstract class InitialDataService
    {
        protected readonly ApplicationDbContext context;
        protected readonly List<Recipe> recipes = new List<Recipe>();
        protected readonly List<Unit> units = new List<Unit>();
        protected readonly List<Nutrient> nutrients = new List<Nutrient>();
        protected readonly List<Ingredient> ingredients = new List<Ingredient>();
        protected readonly List<Category> categories = new List<Category>();

        public InitialDataService(ApplicationDbContext db)
        {
            this.context = db;
        }
        //Check for existing records before adding them to DB
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
                this.context.Units.Add(unit);
            }
            return unit;
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
                this.context.Nutrients.Add(nutrient);
            }
            return nutrient;
        }
        protected virtual Nutrient GetOrCreateNutrient(string nutrientName, string quantity, string unitName)
        {
            Nutrient nutrient = this.context.Nutrients
                .FirstOrDefault(x => string.Equals(x.Name, nutrientName) && string.Equals(x.Unit.Name, unitName) && x.Quantity == quantity);
            if (nutrient != null)
            {
                return nutrient;
            }
            nutrient = this.nutrients
                .FirstOrDefault(x => string.Equals(x.Name, nutrientName) && string.Equals(x.Unit.Name, unitName) && x.Quantity == quantity);
            if (nutrient == null)
            {
                nutrient = new Nutrient { Name = nutrientName, Quantity = quantity, Unit = GetOrCreateUnit(unitName) };
                this.nutrients.Add(nutrient);
                this.context.Nutrients.Add(nutrient);
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
                this.context.Ingredients.Add(ingredient);
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
                this.context.Ingredients.Add(ingredient);
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
                this.context.Categories.Add(category);

            }
            return category;
        }
        protected virtual Category GetOrCreateCategory(string categoryName, string categoryType)
        {
            Category category = this.context.Categories.FirstOrDefault(x => string.Equals(x.Name, categoryName));
            if (category != null)
            {
                return category;
            }
            category = this.categories.FirstOrDefault(x => string.Equals(x.Name, categoryName));
            if (category == null)
            {
                category = new Category { Name = categoryName,Type = categoryType };
                this.categories.Add(category);
                this.context.Categories.Add(category);
            }
            return category;
        }

        protected virtual Component GetOrCreateComponent(string ingredientName, string quantity, string unitName)
        {
            var dbComponent = this.context.Components.FirstOrDefault(x=>x.Ingredient.Name== ingredientName 
            && x.Quantity==quantity && x.Unit.Name==unitName);
            if(dbComponent != null)
            {
                return dbComponent;
            }
            else
            {
                dbComponent = new Component
                {
                    Ingredient = GetOrCreateIngredient(ingredientName),
                    Quantity = quantity,
                    Unit =  !String.IsNullOrEmpty(unitName) ? GetOrCreateUnit(unitName) : null,
                    Text = $"{quantity} {unitName} {ingredientName}"
                };
                return dbComponent;
            }
        }
        protected virtual Section GetOrCreateSection(string name, string recipeId, ICollection<ComponentInputModel> formComponents)
        {
            var dbSection = this.context.Sections.FirstOrDefault(x => x.Name == name && x.Recipes.Any(r => r.Id == recipeId));
            var editComponents = new HashSet<Component>();
            if(dbSection != null)
            {
                foreach(var component in formComponents)
                {
                    var editComponent = GetOrCreateComponent(component.IngredientName,component.Quantity,component.Unit);
                    if (!editComponents.Any(x => x.Id == editComponent.Id))
                    {
                        editComponents.Add(editComponent);
                    }
                }

                dbSection.Components=editComponents;

                if (dbSection.Components.Count() > 0 && editComponents.Count < dbSection.Components.Count())
                {
                    foreach (var component in dbSection.Components)
                    {
                        if (!editComponents.Any(x => x.Id == component.Id))
                        {
                            dbSection.Components.Remove(component);
                            this.context.SaveChanges();
                        }
                    }
                }
                return dbSection;
            }
            else
            {
                var newSection = new Section
                {
                    Name = name,
                    Components = (ICollection<Data.Models.Component>)formComponents.Select(
                        c => GetOrCreateComponent(c.IngredientName, c.Quantity, c.Unit))
                };
                return newSection;
            }
        }
    }
}
 