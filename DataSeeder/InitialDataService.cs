﻿using OurRecipes.Data;
using OurRecipes.Data.Models;
using DataSeeder.Models.ScraperDtos;
using DataSeeder.Models;
using System.Web;
using System.Text.Json;

namespace DataSeeder
{
    public abstract class InitialDataService
    {
        protected readonly ApplicationDbContext context;
        protected readonly List<Recipe> recipes = new List<Recipe>();
        protected readonly List<Unit> units = new List<Unit>();
        protected readonly List<Nutrient> nutrients = new List<Nutrient>();
        protected readonly List<Ingredient> ingredients = new List<Ingredient>();
        protected readonly List<Category> categories = new List<Category>();
        private readonly HttpClient _httpClient = new HttpClient();
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
            Nutrient nutrient = this.context.Nutrients.FirstOrDefault(x => x.Name==nutrientName && x.Quantity == quantity);
            if (nutrient != null)
            {
                return nutrient;
            }
            nutrient = this.nutrients.FirstOrDefault(x => x.Name == nutrientName && x.Quantity == quantity);
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
            Nutrient nutrient = this.context.Nutrients.FirstOrDefault(x => x.Name == nutrientName && x.Quantity == quantity && x.Unit.Name==unitName);
            if (nutrient != null)
            {
                return nutrient;
            }
            nutrient = this.nutrients.FirstOrDefault(x => x.Name == nutrientName && x.Quantity == quantity && x.Unit.Name == unitName);
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
            var categoryImg = GetImageUrl(this._httpClient, categoryName).Result;
            Category category = this.context.Categories.FirstOrDefault(x => string.Equals(x.Name, categoryName));
            if (category != null)
            {
                if (category.imageUrl == null)
                {
                    category.imageUrl = categoryImg;
                }
                return category;
            }
            category = this.categories.FirstOrDefault(x => string.Equals(x.Name, categoryName));
            if (category == null)
            {
                category = new Category { Name = categoryName, Type=categoryName,imageUrl= categoryImg };
                this.categories.Add(category);
                this.context.Categories.Add(category);

            }
            return category;
        }
        protected virtual Category GetOrCreateCategory(string categoryName, string categoryType)
        {
            var categoryImg = GetImageUrl(this._httpClient, categoryName).Result;
            Category category = this.context.Categories.FirstOrDefault(x => string.Equals(x.Name, categoryName));
            if (category != null)
            {
                if (category.imageUrl == null)
                {
                    category.imageUrl = categoryImg;
                }
                return category;
            }
            category = this.categories.FirstOrDefault(x => string.Equals(x.Name, categoryName));
            if (category == null)
            {
                category = new Category { Name = categoryName,Type = categoryType,imageUrl=categoryImg };
                this.categories.Add(category);
                this.context.Categories.Add(category);
            }
            return category;
        }

        protected virtual async Task<string> GetImageUrl(HttpClient client, string title)
        {
            title = HttpUtility.UrlEncode(title);
            string apiKey = "36940976-984b944bf6bcd10a97e6750ab";
            var url = $"https://pixabay.com/api/?key={apiKey}&q={title}";
            string imageUrl = string.Empty;
            var json = await client.GetStringAsync(url);
            if (json != null)
            {
                var result = JsonSerializer.Deserialize<pixabayDto>(json);
                var images = result?.hits.ToList();
                
                imageUrl = images?.FirstOrDefault()?.imageURL?? images?.FirstOrDefault()?.largeImageURL;
            }
            Thread.Sleep(100);
            return imageUrl;
        }
    }
}
 