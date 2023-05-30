namespace OurRecipes.Data.Models
{
    public class Section
    {
        public Section()
        {
            this.Components = new HashSet<Component>();
            this.Recipes = new HashSet<Recipe>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }
        public virtual ICollection<Component> Components { get; set; }
    }
}