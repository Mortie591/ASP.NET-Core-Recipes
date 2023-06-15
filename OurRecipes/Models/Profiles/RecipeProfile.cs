using AutoMapper;
using OurRecipes.Data.Models;
using OurRecipes.Models.Recipes;
using System.Web;

namespace OurRecipes.Models.Profiles
{
    public class RecipeProfile:Profile
    {
        /*
         Name = HttpUtility.HtmlDecode(recipe.Title),
         Description = HttpUtility.HtmlDecode(recipe.Description),
         PrepTime = recipe.PrepTime,
         CookTime = recipe.CookTime,
         Difficulty = recipe.Categories.FirstOrDefault(x => x.Type == "difficulty") != null ? recipe.Categories.FirstOrDefault(x => x.Type == "difficulty").Name : null,
         Servings = int.TryParse(recipe.Servings, out int servings) is true ? servings : 0,
         ImageUrl = recipe.ImageUrl,
         Author = author != null ? author.UserName : null 
         Nutrients = recipe.Nutrients.Where(x => x.Name != "updated_at").ToList(),
         Categories = recipe.Categories.Where(x => x.Type != "difficulty").Select(x => x.Name).ToList(),
         Instructions = String.Join('\n', regex.Split(recipe.Instructions)),
         Sections = recipe.Sections.ToList(),
         Components = recipe.Components.ToList(),
         */
        public RecipeProfile()
        {
            CreateMap<Recipe, RecipeViewModel>()
                .ForMember(d=>d.Name,s=>s.MapFrom(s=>HttpUtility.HtmlDecode(s.Title)))
                .ForMember(d=>d.PrepTime,s=>s.MapFrom(s=>s.PrepTime))
                .ForMember(d=>d.CookTime,s=>s.MapFrom(s=>s.CookTime))
                .ForMember(d=>d.Difficulty,s=>s.MapFrom(s=> s.Categories.FirstOrDefault(x => x.Type == "difficulty")))
                .ForMember(d=>d.Servings,s=>s.MapFrom(s=>Convert.ToInt32(s.Servings)))
                .ForMember(d=>d.Description,s=>s.MapFrom(s=>s.Description))
                .ForMember(d=>d.Author,s=>s.MapFrom(s=>s.Author.UserName))
                .ForMember(d=>d.ImageUrl,s=>s.MapFrom(s=>s.ImageUrl))
                .ForMember(d=>d.Nutrients,s=>s.MapFrom(s=> s.Nutrients.Where(x => x.Name != "updated_at").ToList()))
                .ForMember(d=>d.Categories,s=>s.MapFrom(s => s.Categories.Where(x => x.Type != "difficulty").Select(x => x.Name).ToList()))

                ;
            CreateMap<Recipe, RecipeCardViewModel>();
            CreateMap<Recipe, RecipeByUserViewModel>();
                
        }
    }
}
