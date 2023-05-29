using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OurRecipes.Data.Models
{
    public class Component
    {
        public Component()
        {
            this.Recipes = new HashSet<Recipe>();
        }
        public int Id { get; set; }
        public string Text { get; set; }

        [ForeignKey(nameof(Ingredient))]
        public string IngredientName { get; set; }
        [Required]
        public virtual Ingredient Ingredient { get; set; }
        public string? Unit { get; set; }
        public string Quantity { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }

    }
}