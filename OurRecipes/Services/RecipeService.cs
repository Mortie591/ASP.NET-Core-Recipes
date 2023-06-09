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

        public RecipeService(ApplicationDbContext db) 
            : base(db)
        {
            this.context = db;
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
