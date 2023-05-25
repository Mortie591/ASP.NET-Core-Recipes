namespace OurRecipes.Data.Models
{
    public class Section
    {
        public Section()
        {
            this.Components = new HashSet<Component>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Component> Components { get; set; }
    }
}