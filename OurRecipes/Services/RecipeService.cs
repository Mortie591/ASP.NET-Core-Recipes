﻿using OurRecipes.Data;
using OurRecipes.Data.Models;
using OurRecipes.Models;
using System.Web;

namespace OurRecipes.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly ApplicationDbContext context;
        public RecipeService(ApplicationDbContext db)
        {
            this.context = db;
        }
        public void Add()
        {
            throw new NotImplementedException();
        }

        public ICollection<Recipe> GetLatest()
        {
            throw new NotImplementedException();
        }

        public ICollection<RecipeCardViewModel> GetRandomRecipes()
        {
            var recipes = this.context.Recipes.Select(x=>new
            {
                x.Id,
                x.Title,
                x.Likes,
                x.ImageUrl
            }).OrderBy(x => Guid.NewGuid()).Take(6).ToList();
            
            var recipeCards = new List<RecipeCardViewModel>();
            foreach (var recipe in recipes)
            {
                RecipeCardViewModel viewModel = new RecipeCardViewModel
                {

                    Title = HttpUtility.HtmlDecode(recipe.Title),
                    Rating = recipe.Likes,
                    imageUrl = recipe.ImageUrl
                };
                recipeCards.Add(viewModel);

            }
            return recipeCards;
        }

        public Recipe GetRecipeById(string id)
        {
            throw new NotImplementedException();
        }

        public Recipe GetRecipeByName(string name)
        {
            throw new NotImplementedException();
        }

        public ICollection<Recipe> GetRecipesByCategory(string categoryName)
        {
            throw new NotImplementedException();
        }

        public ICollection<Recipe> GetRecipesByIngredient(string ingredientName)
        {
            throw new NotImplementedException();
        }

        public ICollection<Recipe> GetRecipesByTag(string tagType)
        {
            throw new NotImplementedException();
        }

        public ICollection<Recipe> GetTrending(string categoryName)
        {
            throw new NotImplementedException();
        }

        public void Remove()
        {
            throw new NotImplementedException();
        }
    }
}