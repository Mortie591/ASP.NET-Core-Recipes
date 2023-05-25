namespace OurRecipes.Data.Models
{
    public class Ingredient
    {
        public Ingredient()
        {
            
        }
        
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual Component Component { get; set; }
        public  int ComponentId { get; set; }

    }
}