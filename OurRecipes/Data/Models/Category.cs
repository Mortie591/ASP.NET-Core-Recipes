namespace OurRecipes.Data.Models
{
    public class Category
    {
        public Category()
        {
            this.Recipes = new HashSet<Recipe>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        ICollection<Recipe> Recipes { get; set; }
    }
}
