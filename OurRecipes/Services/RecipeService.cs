﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using OurRecipes.Data;
using OurRecipes.Data.Models;
using OurRecipes.Models.Recipes;
using System.Text.RegularExpressions;
using System.Web;
using static System.Net.Mime.MediaTypeNames;

namespace OurRecipes.Services
{
    public class RecipeService : InitialDataService,IRecipeService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<AppIdentityUser> userManager;

        public RecipeService(ApplicationDbContext db, IMapper mapper, UserManager<AppIdentityUser> userManager) 
            : base(db)
        {
            this.context = db;
            this.mapper = mapper;
            this.userManager = userManager;
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
                AuthorId = recipeDto.Author
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
                        Unit = c.Unit!="---"?GetOrCreateUnit(c.Unit):null,
                        Text = $"{c.Quantity} {c.Unit} {c.IngredientName}"
                    };
                    Recipe.Components.Add(recipeComponent);
                }
                
            }
            
            this.context.Recipes.Add(Recipe);
            this.context.SaveChanges();
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
        public RecipeViewModel GetRecipeById(string id)
        {
            Recipe recipe = context.Recipes.FirstOrDefault(x => x.Id.Equals(id));
            if (recipe != null)
            {
                RecipeViewModel recipeViewModel = mapper.Map<Recipe, RecipeViewModel>(recipe);
                return recipeViewModel;
            }
            return null;
        }
        public RecipeViewModel GetRecipeByName(string name)
        {
            name = HttpUtility.HtmlEncode(name);
            //TODO: split query
            Recipe recipe = context.Recipes
                .Include(x => x.Nutrients)
                .Include(x => x.Components)
                .Include(x => x.Categories)
                .FirstOrDefault(x => x.Title.Equals(name));
            if (recipe != null)
            {
                Regex regex = new Regex(@"([/\d]+[.])|([/\d]+[/\s]+[-])/gm");
                var author = userManager.FindByIdAsync(recipe.AuthorId).Result;
                RecipeViewModel recipeViewModel = new RecipeViewModel
                {
                    Name = HttpUtility.HtmlDecode(recipe.Title),
                    Description = HttpUtility.HtmlDecode(recipe.Description),
                    PrepTime = recipe.PrepTime,
                    CookTime = recipe.CookTime,
                    Difficulty = recipe.Categories.FirstOrDefault(x => x.Type == "difficulty") != null ? recipe.Categories.FirstOrDefault(x => x.Type == "difficulty").Name : null,
                    Servings = int.TryParse(recipe.Servings, out int servings) is true ? servings : 0,
                    Nutrients = recipe.Nutrients.Where(x => x.Name != "updated_at").ToList(),
                    ImageUrl = recipe.ImageUrl,
                    Categories = recipe.Categories.Where(x => x.Type != "difficulty").Select(x => x.Name).ToList(),
                    Instructions = recipe.Instructions.Split(Environment.NewLine,StringSplitOptions.RemoveEmptyEntries).ToList(),
                    Sections = recipe.Sections.ToList(),
                    Components = recipe.Components.ToList(),
                    Author = author != null ? author.UserName : null
                };
                //RecipeViewModel recipeViewModel = mapper.Map<Recipe, RecipeViewModel>(recipe);
                return recipeViewModel;
            }
            return null;
        }
        public ICollection<RecipeCardViewModel> GetRandomRecipes()
        {
            var recipes = this.context.Recipes.Select(x=>new RecipeCardViewModel
            {
                Title = HttpUtility.HtmlDecode(x.Title),
                Rating = x.Likes,
                imageUrl = x.ImageUrl
            }).OrderBy(x => Guid.NewGuid()).Take(6).ToList();
            
            return recipes;
        }
        public ICollection<RecipeCardViewModel> GetLatest()
        {
            try
            {
                var recipes = this.context.Recipes.Select(x => new RecipeCardViewModel
                {
                    Title = HttpUtility.HtmlDecode(x.Title),
                    Rating = x.Likes,
                    imageUrl = x.ImageUrl,
                    Categories = x.Categories,
                    CreatedOnDate = x.CreatedOnDate,
                }).OrderByDescending(x => x.CreatedOnDate).Take(10).ToList();

                return recipes;
            }
            catch (Exception)
            {
                throw new Exception();
            }
            
        }

        public ICollection<RecipeCardViewModel> GetRecipesByCategory(string categoryName)
        {
            var recipes = this.context.Recipes.Select(x => new RecipeCardViewModel
            {
                Title = HttpUtility.HtmlDecode(x.Title),
                Rating = x.Likes,
                imageUrl = x.ImageUrl,
                Categories = x.Categories,
                CreatedOnDate = x.CreatedOnDate,
            }).Where(x => x.Categories.Any(c => c.Name.Equals(categoryName))).ToList();

            return recipes;
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
        
        public ICollection<RecipeCardViewModel> GetTrending()
        {
            var recipes = this.context.Recipes.Select(x => new RecipeCardViewModel
            {
                Title = HttpUtility.HtmlDecode(x.Title),
                Rating = x.Likes,
                imageUrl = x.ImageUrl,
                Categories = x.Categories
            }).OrderByDescending(x=>x.Rating).ThenByDescending(x=>x.CreatedOnDate).ToList();

            return recipes;
        }

        public ICollection<RecipeCardViewModel> GetMyRecipes(string userId)
        {
            try
            {
                var recipes = this.context.Recipes.Where(x => x.AuthorId == userId).Select(x => new RecipeCardViewModel
                {
                    Title = HttpUtility.HtmlDecode(x.Title),
                    Rating = x.Likes,
                    imageUrl = x.ImageUrl,
                    Categories = x.Categories
                }).ToList();

                return recipes;
            }
            catch (Exception)
            {

                throw new Exception();
            }
            
        }

        public ICollection<RecipeByUserViewModel> GetFavouriteRecipes(string userId)
        {
            try
            {
                var recipes = this.context.UserFavourites.Where(x => x.UserId == userId)
                .Select(x => new RecipeByUserViewModel
                {
                    Id = x.RecipeId,
                    AuthorName = x.User.UserName,
                    Title = x.Recipe.Title,
                    Rating = x.Recipe.Likes,
                    imageUrl = x.Recipe.ImageUrl
                })
                .ToList();

                return recipes;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task LikeRecipe(string id, string userId)
        {
            var user = this.context.Users.FirstOrDefault(x => x.Id == userId);
            var recipe = this.context.Recipes.FirstOrDefault(x => x.Id == id);
            if (user != null && recipe!=null)
            {
                user.UserFavourites.Add(new UserFavourite
                {
                    UserId = userId,
                    RecipeId = recipe.Id,
                });
                this.context.SaveChanges();
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        public async Task UnlikeRecipe(string id, string userId)
        {
            var user = this.context.Users.FirstOrDefault(x => x.Id == userId);
            var recipe = this.context.Recipes.FirstOrDefault(x => x.Id == id);
            try
            {
                var userFavouriteItem = user.UserFavourites.FirstOrDefault(x => x.Recipe == recipe);
                if(userFavouriteItem != null)
                {
                    user.UserFavourites.Remove(userFavouriteItem);
                    this.context.SaveChanges();
                }
                else
                {
                    throw new Exception("No such user favourite item");
                }
                
            }
            catch(NullReferenceException)
            {
                throw new NullReferenceException();
            }
        }
        public async Task<ICollection<RecipeByUserViewModel>> GetRecipesByUserAsync(string userId)
        {
            var author = await userManager.FindByIdAsync(userId);
            try
            {
            var recipes = this.context.Recipes.Where(x => x.AuthorId == userId).Select(x => new RecipeByUserViewModel
            {
                Title = HttpUtility.HtmlDecode(x.Title),
                AuthorName = author.UserName,
                Rating = x.Likes,
                imageUrl = x.ImageUrl,
            }).ToList();
               
                return recipes;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        //Private methods
        private List<Category> AddCategories(CreateRecipeInputModel recipeDto)
        {
            var initalCategoryList = recipeDto.Categories;
            initalCategoryList.Add(recipeDto.Cuisine);
            initalCategoryList.Add(recipeDto.Difficulty);
            initalCategoryList.Add(recipeDto.CookingTechnique);
            initalCategoryList.Add(recipeDto.Season);

            var categories = new List<Category>();

            foreach (var category in initalCategoryList)
            {
                if (category == "---") continue;
                string[] categoryLine = category.Split('-', 2, StringSplitOptions.RemoveEmptyEntries);
                var key = categoryLine[0];
                var value = categoryLine[1];

                var dbCategory = this.context.Categories.FirstOrDefault(x => x.Type == key && x.Name == value);

                if (dbCategory == null)
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
        private List<string> CompareIngredients(ICollection<string> source, ICollection<string> destination)
        {
            var result = new List<string>();
            var isMatch = true;
            foreach (var sourceItem in source)
            {
                foreach (var destinationItem in destination)
                {
                    if (destinationItem.Contains(sourceItem) || sourceItem.Contains(destinationItem))
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
        
    }
}
