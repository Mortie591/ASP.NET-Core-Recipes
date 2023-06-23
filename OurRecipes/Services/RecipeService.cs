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
            :base(db)
        {
            this.context = db;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public void Add(CreateRecipeInputModel recipeDto, string authorId)
        {
            //var sections = recipeDto.Sections;
            //var componens = recipeDto.Components;

            var Recipe = new Recipe()
            {
                Title = recipeDto.Title,
                Description = recipeDto.Description,
                PrepTime = recipeDto.PrepTime.ToString(),
                CookTime = recipeDto.CookTime.ToString(),
                Servings = recipeDto.Servings.ToString(),
                CreatedOnDate = DateTime.Now,
                Categories = EditOrCreateCategories(recipeDto),
                Sections = recipeDto.Sections.Select(x=>new Section
                {
                    Name = x.SectionName,
                    Components = x.Components.Select(c=>new Component
                    {
                        Ingredient = GetOrCreateIngredient(c.IngredientName),
                        Quantity = c.Quantity,
                        Unit = c.Unit!="---"?GetOrCreateUnit(c.Unit):null,
                        Text = $"{c.Quantity} {c.Unit} {c.IngredientName}"

                    }).ToList()
                }).ToList(),
                Instructions = recipeDto.Instructions,
                Nutrients = recipeDto.Nutrients.Select(x=>GetOrCreateNutrient(x.Name,x.Quantity,x.UnitName)).ToList(),
                AuthorId = authorId
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
            var difficulty = recipe.Categories.FirstOrDefault(x => x.Type?.ToLower() == "difficulty");
            var cuisine = recipe.Categories.FirstOrDefault(x => x.Type?.ToLower() == "cuisine");
            var seasonal = recipe.Categories.FirstOrDefault(x => x.Type?.ToLower() == "seasonal");
            var cookingTechnique = recipe.Categories.FirstOrDefault(x => x.Type?.ToLower() == "cooking technique");
            
            var recipeData = new EditRecipeViewModel()
            {
                Id = recipe.Id,
                Title = recipe.Title,
                Description = recipe.Description,
                PrepTime = int.Parse(recipe.PrepTime),
                CookTime = int.Parse(recipe.CookTime),
                Servings = int.Parse(recipe.Servings),
                
                Difficulty = difficulty?.Name,
                Cuisine = cuisine?.Name,
                Season = seasonal?.Name,
                CookingTechnique = cookingTechnique?.Name,
                Sections = recipe.Sections.Select(x => new SectionInputModel
                {
                    Id =x.Id,
                    SectionName = x.Name,
                    Components = x.Components.Any()?x.Components.Select(c => new ComponentInputModel
                    {
                        Id = c.Id,
                        IngredientName = c.Ingredient.Name,
                        Quantity = c.Quantity,
                        Unit =c.Unit?.Name??String.Empty,
                        Text = c.Text
                    }).ToList():new List<ComponentInputModel>()
                }).ToList(),
                
                Instructions = recipe.Instructions,
                Nutrients = recipe.Nutrients.Select(x => new NutrientInputModel
                {
                    Name = x.Name,
                    Quantity = $"{x.Quantity}{x.Unit?.Name}",
                }).ToList(),
        };
            if (recipe.Sections.Any())
            {
                recipeData.Components = new List<ComponentInputModel>();

                foreach (var component in recipe.Components)
                {
                    if (!IsInSection(component,recipe.Sections))
                    {
                        recipeData.Components.Add(new ComponentInputModel
                        {
                            Id = component.Id,
                            IngredientName = component.Ingredient.Name,
                            Quantity = component.Quantity,
                            Unit = component.Unit?.Name ?? String.Empty,
                            Text = component.Text
                        });
                    
                    }
                }
            }
            else
            {
                recipeData.Components = recipe.Components.Select(c => new ComponentInputModel
                {
                    Id = c.Id,
                    IngredientName = c.Ingredient.Name,
                    Quantity = c.Quantity,
                    Unit = c.Unit?.Name ?? String.Empty,
                    Text = c.Text
                }).ToList();
            }

            foreach (var category in recipe.Categories.Where(x => x.Type?.ToLower() != "difficulty" && x.Type?.ToLower() != "cuisine"
                && x.Type?.ToLower() != "seasonal" && x.Type?.ToLower() != "cooking technique"))
            {
               
               recipeData.Categories.Add($"{category.Type}-{category.Name}");
            }

            return recipeData;
        }

        private bool IsInSection(Component component, ICollection<Section> sections)
        {
            foreach(Section section in sections)
            {
                if(section.Components.Any(x=>x.Id==component.Id))
                {
                    return true;
                }
            }
            return false;
        }

        public void Edit(EditRecipeViewModel recipeData)
        {
            var recipe = GetRecipeById(recipeData.Id) ??
                throw new NullReferenceException(nameof(recipeData.Id));

            recipe.Title = recipeData.Title;
            recipe.Description = recipeData.Description;
            recipe.PrepTime = recipeData.PrepTime?.ToString();
            recipe.CookTime = recipeData.CookTime?.ToString();
            recipe.Servings = recipeData.Servings.ToString();
            recipe.Categories = EditOrCreateCategories(recipeData);
            recipe.Sections = EditOrCreateSections(recipeData);
            recipe.Components = EditOrCreateComponents(recipeData);
            recipe.Instructions = recipeData.Instructions;
            recipe.Nutrients = recipeData.Nutrients.Select(x => GetOrCreateNutrient(x.Name, x.Quantity,x.UnitName)).ToList();

            this.context.SaveChanges();
        }
        public void Delete(string id)
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
               throw new NullReferenceException(nameof(id));
            }
        }
        public Recipe GetRecipeById(string id)
        {
            if (context.Recipes.Any(x => x.Id == id))
            {
                Recipe recipe = context.Recipes
                .Include(x => x.Nutrients)
                .Include(x => x.Sections).ThenInclude(x=>x.Components)
                .ThenInclude(x => x.Ingredient)
                .Include(x => x.Sections).ThenInclude(x => x.Components)
                .ThenInclude(x => x.Unit)
                .Include(x => x.Components).ThenInclude(x => x.Ingredient)
                .Include(x => x.Components).ThenInclude(x => x.Unit)
                .Include(x => x.Categories)
                .Include(x => x.Author)
                .FirstOrDefault(x => x.Id.Equals(id));
                return recipe;
            }
            else
            {
                throw new NullReferenceException(nameof(id));
            }
        }
        public Recipe GetRecipeByName(string name)
        {
            Recipe recipe = context.Recipes
                .Include(x => x.Nutrients)
                .Include(x=>x.Sections)
                .Include(x => x.Components)
                .Include(x => x.Categories)
                .FirstOrDefault(x => x.Title.Contains(name))??
                throw new NullReferenceException(nameof(name));
      
            return recipe;
        }
        public RecipeViewModel GetRecipeViewModel(string id)
        {
            var recipe = this.GetRecipeById(id);
            var difficulty = recipe.Categories.FirstOrDefault(x => x.Type?.ToLower() == "difficulty");
            var cuisine = recipe.Categories.FirstOrDefault(x => x.Type?.ToLower() == "cuisine");
            var seasonal = recipe.Categories.FirstOrDefault(x => x.Type?.ToLower() == "seasonal");
            var cookingTechnique = recipe.Categories.FirstOrDefault(x => x.Type?.ToLower() == "cooking technique");

            var recipeData = new RecipeViewModel()
            {
                Id = recipe.Id,
                Title = recipe.Title,
                Description = recipe.Description,
                PrepTime = recipe.PrepTime,
                CookTime = recipe.CookTime,
                Servings = int.Parse(recipe.Servings),
                Difficulty = difficulty?.Name,
                Cuisine = cuisine?.Name,
                Season = seasonal?.Name,
                CookingTechnique = cookingTechnique?.Name,
                AuthorId = recipe.AuthorId,
                Author = recipe.Author,
                Components = recipe.Components.ToList(),
                Sections = recipe.Sections?.ToList(),
                Instructions = recipe.Instructions.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList(),
                Nutrients = recipe.Nutrients.ToList(),
            };

            if (recipe.Sections.Any())
            {
                recipeData.Components = new List<Component>();

                foreach (var component in recipe.Components)
                {
                    if (!IsInSection(component, recipe.Sections))
                    {
                        recipeData.Components.Add(component);
                    }
                }
            }
            else
            {
                recipeData.Components = recipe.Components.ToList();
            }

            foreach (var category in recipe.Categories.Where(x => x.Type?.ToLower() != "difficulty" && x.Type?.ToLower() != "cuisine"
                && x.Type?.ToLower() != "seasonal" && x.Type?.ToLower() != "cooking technique"))
            {
                recipeData.Categories.Add($"{category.Type}-{category.Name}");
            }

            return recipeData;
        }
        public ICollection<RecipeCardViewModel> GetRandomRecipes()
        {
            try
            {
                var recipes = this.context.Recipes.Select(x => new RecipeCardViewModel
                {
                    Id = x.Id,
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
                    Id = x.Id,
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
                    Id= x.Id,
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
                        Id = recipe.Id,
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
                    Id = x.Id,
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
                Id = x.Id,
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
        
        private List<Component> EditOrCreateComponents(EditRecipeViewModel recipeData)
        {
            var editComponents = new HashSet<Component>();
            var components = recipeData.Components;
            foreach (var component in components)
            {
                var editComponent = GetOrCreateComponent(component.IngredientName, component.Quantity, component.Unit);
                if (!editComponents.Any(x=>x.Id==component.Id))
                {
                    editComponents.Add(editComponent);
                }
            }
            var dbComponents = this.context.Components.Where(x => x.Recipes.Any(x => x.Id == recipeData.Id));
            if(dbComponents.Count()>0 && editComponents.Count< dbComponents.Count())
            {
                foreach(var component in dbComponents)
                {
                    if (!editComponents.Any(x=>x.Id==component.Id))
                    {
                        this.context.Recipes.First(x=>x.Id== recipeData.Id).Components.Remove(component);
                        this.context.SaveChanges();
                    }
                }
            }
            return editComponents.ToList();
        }
        private List<Section> EditOrCreateSections(EditRecipeViewModel recipeData)
        {
            var editSections = new List<Section>();
            var sections = recipeData.Sections;
            foreach (var section in sections)
            {
                var editSection = GetOrCreateSection(section.SectionName, recipeData.Id, section.Components);

                editSections.Add(editSection);
            }




            var dbSections= this.context.Sections.Where(x => x.Recipes.Any(x => x.Id == recipeData.Id));
            if (dbSections.Count() > 0 && editSections.Count < dbSections.Count())
            {
                foreach (var section in dbSections)
                {
                    if (!editSections.Any(x => x.Id == section.Id))
                    {
                        this.context.Recipes.First(x => x.Id == recipeData.Id).Sections.Remove(section);
                        this.context.SaveChanges();
                    }
                }
            }

            return editSections;
        }
        private List<Category> EditOrCreateCategories(CreateRecipeInputModel recipeDto)
        {
            var initalCategoryList = new Dictionary<string, List<string>> ();
            if (recipeDto.Categories.Count > 0)
            {
                foreach (var category in recipeDto.Categories)
                {
                    var categoryType = category.Split('-', StringSplitOptions.RemoveEmptyEntries)[0];
                    var categoryName = category.Split('-', StringSplitOptions.RemoveEmptyEntries)[1];
                    if (!initalCategoryList.ContainsKey(categoryType))
                    {
                        initalCategoryList.Add(categoryType, new List<string>());
                    }
                    initalCategoryList[categoryType].Add(categoryName);
                }
            }
            initalCategoryList.Add("Cuisine",new List<string> { recipeDto.Cuisine });
            initalCategoryList.Add("Difficulty",new List<string> { recipeDto.Difficulty });
            initalCategoryList.Add("Seasonal", new List<string> { recipeDto.Season });
            initalCategoryList.Add("Cooking Technique", new List<string> { recipeDto.CookingTechnique });

            var categories = new List<Category>();

            foreach (var category in initalCategoryList)
            {
                foreach(var categoryItem in category.Value)
                {
                    if (!String.IsNullOrEmpty(categoryItem))
                    {
                        categories.Add(GetOrCreateCategory(categoryItem, category.Key));
                    }
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
