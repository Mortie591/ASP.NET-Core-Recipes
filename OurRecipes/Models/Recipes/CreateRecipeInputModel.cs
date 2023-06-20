using OurRecipes.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace OurRecipes.Models.Recipes
{
    public class CreateRecipeInputModel
    {
        public CreateRecipeInputModel()
        {
            Categories = new List<string>();
            Sections = new List<SectionInputModel>();
            Components = new List<ComponentInputModel>();
            Nutrients = new List<NutrientInputModel>();
        }
        [Required, MinLength(4),MaxLength(50)]
        public string Title { get; set; }
        [Required, MinLength(4), MaxLength(250)]
        public string Description { get; set; }
        [Required, Range(1,12)]
        public int Servings { get; set; }
        [Range(0,24*60)]
        public int? PrepTime { get; set; }
        [Range(0, 24 * 60)]
        public int? CookTime { get; set; }
        [Required]
        //[RegularExpression(@"/^(?:(?<scheme>[^:\\/?#]+):)?(?:\\/\\/(?<authority>[^\\/?#]*))?(?<path>[^?#]*\\/)?(?<file>[^?#]*\\.(?<extension>[Jj][Pp][Ee]?[Gg]|[Pp][Nn][Gg]|[Gg][Ii][Ff]))(?:\\?(?<query>[^#]*))?(?:#(?<fragment>.*))?$/gm")]
        public string ImageUrl { get; set; }
        public  List<string> Categories { get; set; } //Type <-> Name
        public string? Cuisine { get; set; } 
        public string? Season { get; set; }
        public string? Difficulty { get; set; }
        public string? CookingTechnique { get; set; }
        public List<SectionInputModel>? Sections { get; set; }
        public List<ComponentInputModel> Components { get; set; }

        [Required,MinLength(10)]
        public string Instructions { get; set; }
        public List<NutrientInputModel> Nutrients { get; set; }
    }
}
