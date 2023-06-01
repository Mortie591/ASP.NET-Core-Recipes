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
        [Required, MaxLength(250)]
        public string Text { get; set; }

        [ForeignKey(nameof(Ingredient))]
        public int IngredientId { get; set; }
        [Required]
        public virtual Ingredient Ingredient { get; set; }
        public int? UnitId { get; set; }
        public virtual Unit? Unit { get; set; }
        public string Quantity { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }

    }
}