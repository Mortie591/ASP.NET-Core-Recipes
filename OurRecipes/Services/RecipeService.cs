﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using OurRecipes.Data;
using OurRecipes.Data.Models;
using OurRecipes.Models.Recipes;
using System.Web;
using static System.Net.Mime.MediaTypeNames;

namespace OurRecipes.Services
{
    public class RecipeService : InitialDataService,IRecipeService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public RecipeService(ApplicationDbContext db, IMapper mapper) 
            : base(db)
        {
            this.context = db;
            this.mapper = mapper;
        }

        public void Add(CreateRecipeInputModel recipeDto)
        {
            //var sections = recipeDto.Sections;
            //var componens = recipeDto.Components;

            var Recipe = new Recipe()
            {
                Title = recipeDto.Title,
                Description = recipeDto.Description,
                PrepTime = recipeDto.PrepTime.ToString(),
                CookTime = recipeDto.CookTime.ToString(),
                Servings = recipeDto.CookTime.ToString(),
                CreatedOnDate = DateTime.Now,
                Categories = AddCategories(recipeDto),
                Sections = recipeDto.Sections.Select(x=>new Section
                {
                    Name = x.SectionName,
                    Components = x.Components.Select(c=>new Component
                    {
                        Ingredient = GetOrCreateIngredient(c.IngredientName),
                        Quantity = c.Quantity,
                        Unit = GetOrCreateUnit(c.Unit),
                        Text = $"{c.Quantity} {c.Unit} {c.IngredientName}"

                    }).ToList()
                }).ToList(),
                Instructions = recipeDto.Instructions,
                Nutrients = recipeDto.Nutrients.Select(x=>GetOrCreateNutrient(x.Name,x.Quantity)).ToList(),
                //Author
            };
            if(Recipe.Sections.Any())
            {
                Recipe.Components = GetOrCreateComponents(Recipe.Sections);
            }
            if(recipeDto.Components.Any())
            {
                foreach(var c in recipeDto.Components)
                {
                    Component recipeComponent = new Component
                    {
                        Ingredient = GetOrCreateIngredient(c.IngredientName),
                        Quantity = c.Quantity,
                        Unit = GetOrCreateUnit(c.Unit),
                        Text = $"{c.Quantity} {c.Unit} {c.IngredientName}"
                    };
                    Recipe.Components.Add(recipeComponent);
                }
                
            }
            
            this.context.Recipes.Add(Recipe);
            this.context.SaveChanges();
        }

        public ICollection<RecipeCardViewModel> GetRandomRecipes()
        {
            var recipes = this.context.Recipes.Select(x=>new
            {
                x.Id,
                x.Title,
                x.Categories,
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
        public ICollection<RecipeCardViewModel> GetLatest()
        {
            var recipes = this.context.Recipes.Select(x => new
            {
                x.Id,
                x.Title,
                x.Categories,
                x.Likes,
                x.ImageUrl,
                x.CreatedOnDate
            }).OrderByDescending(x => x.CreatedOnDate).Take(10).ToList();

            var recipeCards = new List<RecipeCardViewModel>();
            foreach (var recipe in recipes)
            {
                RecipeCardViewModel viewModel = new RecipeCardViewModel
                {

                    Title = HttpUtility.HtmlDecode(recipe.Title),
                    Rating = recipe.Likes,
                    imageUrl = recipe.ImageUrl,
                    Categories = recipe.Categories,
                };
                recipeCards.Add(viewModel);
            }
            return recipeCards;
        }
        

        public RecipeViewModel GetRecipeById(string id)
        {
            Recipe recipe = context.Recipes.FirstOrDefault(x=>x.Id.Equals(id));
            if (recipe != null)
            {
                RecipeViewModel recipeViewModel = mapper.Map<Recipe, RecipeViewModel>(recipe);
                return recipeViewModel;
            }
            return null;
        }

        public RecipeViewModel GetRecipeByName(string name)
        {
            Recipe recipe = context.Recipes.FirstOrDefault(x => x.Title.Equals(name));
            if (recipe != null)
            {
                RecipeViewModel recipeViewModel = mapper.Map<Recipe, RecipeViewModel>(recipe);
                return recipeViewModel;
            }
            return null;
        }

        public ICollection<RecipeCardViewModel> GetRecipesByCategory(string categoryName)
        {
            var recipes = this.context.Recipes.Select(x => new
            {
                x.Id,
                x.Title,
                x.Categories,
                x.Likes,
                x.ImageUrl
            }).Where(x => x.Categories.Any(c => c.Name.ToLower().Equals(categoryName.ToLower())));

            var recipeCards = new List<RecipeCardViewModel>();
            foreach (var recipe in recipes)
            {
                //RecipeCardViewModel recipeViewModel = mapper.Map<Recipe, RecipeCardViewModel>(recipe);
                RecipeCardViewModel viewModel = new RecipeCardViewModel
                {
                    Title = HttpUtility.HtmlDecode(recipe.Title),
                    Rating = recipe.Likes,
                    imageUrl = recipe.ImageUrl,
                };
                recipeCards.Add(viewModel);
            }
            return recipeCards;
        }

        public ICollection<RecipeCardViewModel> GetRecipesByIngredients(params string[] ingredients)
        {
            var recipeCards = new List<RecipeCardViewModel>();
            var recipes = this.context.Recipes
                .Include(x=>x.Components).ThenInclude(x=>x.Ingredient)
                .Select(x => new 
            {
                x.Id,
                x.Title,
                x.Components,
                x.Likes,
                x.ImageUrl
            });
            
            foreach (var recipe in recipes)
            {
                var components = recipe.Components.Where(c=>c.Ingredient!=null).Select(c => c.Ingredient.Name).ToList();
                //var result = ingredients.Intersect(components);
                var result = CompareIngredients(ingredients, components);
                if (result.Count() == ingredients.Length)
                {
                    RecipeCardViewModel viewModel = new RecipeCardViewModel
                    {
                        Title = HttpUtility.HtmlDecode(recipe.Title),
                        Rating = recipe.Likes,
                        imageUrl = recipe.ImageUrl,
                    };
                    recipeCards.Add(viewModel);
                }
            }
            
            return recipeCards;
        }

        private List<string> CompareIngredients(ICollection<string> source, ICollection<string> destination)
        {
            var result = new List<string>();
            var isMatch = true;
            foreach (var sourceItem in source)
            {
                foreach (var destinationItem in destination)
                {
                    if(destinationItem.Contains(sourceItem) || sourceItem.Contains(destinationItem)) 
                    {
                        result.Add(sourceItem);
                    }
                    else
                    {
                        isMatch = false;
                        continue;
                    }
                }
            }
            return result;
        }
        public ICollection<RecipeCardViewModel> GetTrending()
        {
            var recipes = this.context.Recipes.Select(x => new
            {
                x.Id,
                x.Title,
                x.Likes,
                x.Categories,
                x.ImageUrl,
                x.CreatedOnDate
            }).OrderByDescending(x=>x.Likes).ThenByDescending(x=>x.CreatedOnDate).ToList();

            var recipeCards = new List<RecipeCardViewModel>();
            foreach (var recipe in recipes)
            {
                RecipeCardViewModel viewModel = new RecipeCardViewModel
                {
                    Title = HttpUtility.HtmlDecode(recipe.Title),
                    Rating = recipe.Likes,
                    imageUrl = recipe.ImageUrl,
                    Categories = recipe.Categories
                };
                recipeCards.Add(viewModel);
            }
            return recipeCards;
        }

        public void Remove(string id)
        {
            Recipe recipe = this.context.Recipes.FirstOrDefault(x => x.Id == id);
            if (recipe != null)
            {
                this.context.Recipes.Remove(recipe);
                this.context.SaveChanges();
                Console.WriteLine($"Successfully deleted recipe with id: {id}");
            }
            else
            {
                Console.WriteLine($"No recipe with id: {id} found");
            }
        }

        private List<Category> AddCategories(CreateRecipeInputModel recipeDto)
        {
            var initalCategoryList = recipeDto.Categories;
            initalCategoryList.Add(recipeDto.Cuisine);
            initalCategoryList.Add(recipeDto.Difficulty);
            initalCategoryList.Add(recipeDto.CookingTechnique);
            initalCategoryList.Add(recipeDto.Season);
           
            var categories = new List<Category>();
            
            foreach(var category in initalCategoryList)
            {
                if (category == "---") continue;
                string[] categoryLine = category.Split('-',2,StringSplitOptions.RemoveEmptyEntries);
                var key = categoryLine[0];
                var value = categoryLine[1];
                
                var dbCategory=this.context.Categories.FirstOrDefault(x => x.Type == key && x.Name == value);

                if (dbCategory==null)
                {
                    categories.Add(new Category
                    {
                        Type = key,
                        Name = value,
                    });
                }
                else
                {
                    categories.Add(dbCategory);
                }
                
            }

            return categories;
        }
    }
}
