using AutoMapper;
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
        public EditRecipeViewModel GetEditData(string id)
        {
            var recipe = GetRecipeById(id)??
                throw new NullReferenceException(nameof(id));
            var difficulty = recipe.Categories.FirstOrDefault(x => x.Type.ToLower() == "difficulty");
            var cuisine = recipe.Categories.FirstOrDefault(x => x.Type.ToLower() == "cuisine");
            var seasonal = recipe.Categories.FirstOrDefault(x => x.Type.ToLower() == "seasonal");
            var cookingTechnique = recipe.Categories.FirstOrDefault(x => x.Type.ToLower() == "cookingtechnique");
            //var recipeDto = this.mapper.Map<Recipe,EditRecipeViewModel>(recipe);
            var recipeDto = new EditRecipeViewModel()
            {
                Title = recipe.Title,
                Description = recipe.Description,
                PrepTime = int.Parse(recipe.PrepTime),
                CookTime = int.Parse(recipe.CookTime),
                Servings = int.Parse(recipe.CookTime),
                Categories = recipe.Categories.Where(x => x.Type.ToLower() != "difficulty" && x.Type.ToLower() != "cuisine"
                && x.Type.ToLower() != "seasonal" && x.Type.ToLower() != "cookingtechnique")
                .Select(x => x.Name).ToList(),
                Difficulty = difficulty != null ? difficulty.Name : null,
                Cuisine = cuisine != null ? cuisine.Name : null,
                Season = seasonal != null ? seasonal.Name : null,
                CookingTechnique = cookingTechnique != null ? cookingTechnique.Name : null,
                Sections = recipe.Sections.Select(x => new SectionInputModel
                {
                    SectionName = x.Name,
                    Components = x.Components.Select(c => new ComponentInputModel
                    {
                        IngredientName = c.Ingredient.Name,
                        Quantity = c.Quantity,
                        Unit = c.Unit == null ? null : c.Unit.Name,
                        Text = c.Text
                    }).ToList()
                }).ToList(),
                Instructions = recipe.Instructions,
                Nutrients = recipe.Nutrients.Select(x => new NutrientInputModel
                {
                    Name = x.Name,
                    Quantity = $"{x.Quantity}{x.Unit}",
                }).ToList(),
                Author = userManager.FindByIdAsync(recipe.AuthorId).Result.UserName
            };

            return recipeDto;
        }
        public void Edit(EditRecipeViewModel recipeData)
        {

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
        public Recipe GetRecipeById(string id)
        {
            Recipe recipe = context.Recipes
                .Include(x => x.Nutrients)
                .Include(x => x.Sections)
                .Include(x => x.Components).ThenInclude(x => x.Ingredient)
                .Include(x => x.Components).ThenInclude(x => x.Unit)
                .Include(x => x.Categories)
                .FirstOrDefault(x => x.Id.Equals(id));
            if (recipe != null)
            {
                return recipe;
            }
            return null;
        }
        public RecipeViewModel GetRecipeByName(string name)
        {
            Recipe recipe = context.Recipes
                .Include(x => x.Nutrients)
                .Include(x=>x.Sections)
                .Include(x => x.Components)
                .Include(x => x.Categories)
                .FirstOrDefault(x => x.Title.Equals(name));
            if (recipe != null)
            {
                RecipeViewModel recipeViewModel = mapper.Map<Recipe, RecipeViewModel>(recipe);
                foreach(var section in recipeViewModel.Sections)
                {
                    foreach (var component in section.Components)
                    {
                        recipeViewModel.Components.Remove(component);
                    }
                }
                return recipeViewModel;
            }
            return null;
        }
        public ICollection<RecipeCardViewModel> GetRandomRecipes()
        {
            try
            {
                var recipes = this.context.Recipes.Select(x => new RecipeCardViewModel
                {
                    Title = HttpUtility.HtmlDecode(x.Title),
                    Rating = x.Likes,
                    ImageUrl = x.ImageUrl,
                    Categories = x.Categories,
                    CreatedOnDate = x.CreatedOnDate,
                }).OrderBy(x => Guid.NewGuid()).Take(6).ToList();

                return recipes;
            }
            catch (Exception)
            {

                throw new Exception();
            }
           
        }
        public ICollection<RecipeCardViewModel> GetLatest()
        {
            try
            {
                var recipes = this.context.Recipes.Select(x => new RecipeCardViewModel
                {
                    Title = HttpUtility.HtmlDecode(x.Title),
                    Rating = x.Likes,
                    ImageUrl = x.ImageUrl,
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
            try
            {
                var recipes = this.context.Recipes.Select(x => new RecipeCardViewModel
                {
                    Title = HttpUtility.HtmlDecode(x.Title),
                    Rating = x.Likes,
                    ImageUrl = x.ImageUrl,
                    Categories = x.Categories,
                    CreatedOnDate = x.CreatedOnDate,
                }).Where(x => x.Categories.Any(c => c.Name.Equals(categoryName))).ToList();

                return recipes;
            }
            catch (Exception)
            {

                throw;
            }
            
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
                        ImageUrl = recipe.ImageUrl,
                    };
                    recipeCards.Add(viewModel);
                }
            }
            
            return recipeCards;
        }
        
        public ICollection<RecipeCardViewModel> GetTrending()
        {
            try
            {
                var recipes = this.context.Recipes.Select(x => new RecipeCardViewModel
                {
                    Title = HttpUtility.HtmlDecode(x.Title),
                    Rating = x.Likes,
                    ImageUrl = x.ImageUrl,
                    Categories = x.Categories
                }).OrderByDescending(x => x.Rating).ThenByDescending(x => x.CreatedOnDate).ToList();

                return recipes;
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        public ICollection<RecipeCardViewModel> GetMyRecipes(string userId)
        {
            try
            {
                var recipes = this.context.Recipes.Where(x => x.AuthorId == userId).Select(x => new RecipeCardViewModel
                {
                    Id = x.Id,
                    Title = HttpUtility.HtmlDecode(x.Title),
                    Rating = x.Likes,
                    ImageUrl = x.ImageUrl,
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
                    ImageUrl = x.Recipe.ImageUrl
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
                ImageUrl = x.ImageUrl,
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
