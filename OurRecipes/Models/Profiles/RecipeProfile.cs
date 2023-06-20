using AutoMapper;
using OurRecipes.Data.Models;
using OurRecipes.Models.Recipes;
using System.Web;

namespace OurRecipes.Models.Profiles
{
    public class RecipeProfile:Profile
    {
        public RecipeProfile()
        {
            CreateMap<Recipe, RecipeViewModel>()
                .ForMember(d => d.Title, s => s.MapFrom(s => HttpUtility.HtmlDecode(s.Title)))
                .ForMember(d => d.PrepTime, s => s.MapFrom(s => s.PrepTime))
                .ForMember(d => d.CookTime, s => s.MapFrom(s => s.CookTime))
                .ForMember(d => d.Rating, s => s.MapFrom(s => s.Likes))
                .ForMember(d => d.Difficulty, s => s.MapFrom(s => s.Categories.FirstOrDefault(x => x.Type == "Difficulty").Name))
                .ForMember(d => d.Servings, opt=>opt.ConvertUsing(new IntConverter()))
                .ForMember(d => d.Description, s => s.MapFrom(s => HttpUtility.HtmlDecode(s.Description)))
                .ForMember(d=>d.Instructions,s=>s.MapFrom(s=> s.Instructions.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList()))
                .ForMember(d => d.AuthorName, s => s.MapFrom(s => s.Author.UserName))
                .ForMember(d => d.ImageUrl, s => s.MapFrom(s => s.ImageUrl))
                .ForMember(d => d.Nutrients, s => s.MapFrom(s => s.Nutrients))
                .ForMember(d => d.Categories, s => s.MapFrom(s => s.Categories.Where(x => x.Type != "Difficulty").Select(x => x.Name)))
                .ForMember(d => d.Components, s => s.MapFrom(s => s.Components))
                .ForMember(d => d.Sections, s => s.MapFrom(s => s.Sections));



            //Not in use, just in case
            /*
            CreateMap<Recipe, EditRecipeViewModel>()
                .ForMember(d => d.Title, s => s.MapFrom(s => HttpUtility.HtmlDecode(s.Title)))
                .ForMember(d => d.PrepTime, s => s.MapFrom(s => s.PrepTime))
                .ForMember(d => d.CookTime, s => s.MapFrom(s => s.CookTime))
                .ForMember(d => d.Difficulty, s => s.MapFrom(s => s.Categories.FirstOrDefault(x => x.Type == "Difficulty").Name))
                .ForMember(d => d.Cuisine, s => s.MapFrom(s => s.Categories.FirstOrDefault(x => x.Type == "Cuisine").Name))
                .ForMember(d => d.CookingTechnique, s => s.MapFrom(s => s.Categories.FirstOrDefault(x => x.Type == "Cooking Technique").Name))
                .ForMember(d => d.Season, s => s.MapFrom(s => s.Categories.FirstOrDefault(x => x.Type == "Seasonal").Name))
                .ForMember(d => d.Categories, s => s.MapFrom(s => s.Categories.Where(x => x.Type != "Difficulty" && x.Type != "Cuisine" && x.Type != "seasonal" && x.Type != "cookingtechnique").Select(x => x.Name)))
                .ForMember(d => d.Servings, opt => opt.ConvertUsing(new IntConverter()))
                .ForMember(d => d.Description, s => s.MapFrom(s => HttpUtility.HtmlDecode(s.Description)))
                .ForMember(d => d.Instructions, s => s.MapFrom(s => s.Instructions.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList()))
                .ForMember(d => d.ImageUrl, s => s.MapFrom(s => s.ImageUrl))
                .ForMember(d => d.Nutrients, s => s.MapFrom(s => s.Nutrients.Select(x => new NutrientInputModel
                {
                    Name = x.Name,
                    Quantity = x.Quantity,
                    UnitName = x.Unit.Name
                }).ToList()))
                .ForMember(d => d.Components, s => s.MapFrom(s => s.Components.Select(c => new ComponentInputModel
                {
                    IngredientName = c.Ingredient.Name,
                    Quantity = c.Quantity,
                    Unit = c.Unit == null ? null : c.Unit.Name,
                    Text = c.Text
                }).ToList()))
                .ForMember(d => d.Sections, opt => opt.MapFrom(s => s.Sections.Select(x => new SectionInputModel
                {
                    SectionName = x.Name,
                    Components = x.Components.Select(c => new ComponentInputModel
                    {
                        IngredientName = c.Ingredient.Name,
                        Quantity = c.Quantity,
                        Unit = c.Unit == null ? null : c.Unit.Name,
                        Text = c.Text
                    }).ToList()
                }))); 
             
            CreateMap<Recipe, RecipeCardViewModel>()
                 .ForMember(d => d.Title, s => s.MapFrom(s => HttpUtility.HtmlDecode(s.Title)))
                 .ForMember(d=>d.CreatedOnDate,s=>s.MapFrom(s=>s.CreatedOnDate))
                 .ForMember(d => d.ImageUrl, s => s.MapFrom(s => s.ImageUrl))
                 .ForMember(d => d.Rating, s => s.MapFrom(s => s.Likes))
                 .ForMember(d => d.Categories, s => s.MapFrom(s => s.Categories));

            CreateMap<Recipe, RecipeByUserViewModel>()
                .ForMember(d => d.Title, s => s.MapFrom(s => HttpUtility.HtmlDecode(s.Title)))
                 .ForMember(d => d.ImageUrl, s => s.MapFrom(s => s.ImageUrl))
                 .ForMember(d => d.Rating, s => s.MapFrom(s => s.Likes))
                 .ForMember(d => d.AuthorName, s => s.MapFrom(s => s.Author.UserName)); 
                */
        }
    }
}
