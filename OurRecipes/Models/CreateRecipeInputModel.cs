using OurRecipes.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace OurRecipes.Models
{
    public class CreateRecipeInputModel
    {
        public string Title { get; set; }
        public string Description { get; set; }

        [MaxLength(20)]
        public string Servings { get; set; }
        [MaxLength(20)]
        public string? PrepTime { get; set; }
        [MaxLength(20)]
        public string? CookTime { get; set; }
        public string ImageUrl { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public ICollection<Section>? Sections { get; set; }
        public virtual ICollection<Component> Components { get; set; }
        public string Instructions { get; set; }
        public virtual ICollection<Nutrient> Nutrients { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}