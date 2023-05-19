namespace OurRecipes.Data.Models
{
    public class Ingredient
    {
        //"1 tablespoon unsalted butter or margarine"
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}