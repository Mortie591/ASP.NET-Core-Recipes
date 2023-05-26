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
        [ForeignKey(nameof(Ingredient))]
        public int IngredientId { get; set; }
        [Required]
        public Ingredient Ingredient { get; set; }
        public Unit? Unit { get; set; }
        public double Quantity { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }

    }
}