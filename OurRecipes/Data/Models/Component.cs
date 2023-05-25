namespace OurRecipes.Data.Models
{
    public class Component
    {
        public Component()
        {
            this.Recipes = new HashSet<Recipe>();
        }
        public int Id { get; set; }
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
        public Unit? Unit { get; set; }
        public double Quantity { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }

    }
}