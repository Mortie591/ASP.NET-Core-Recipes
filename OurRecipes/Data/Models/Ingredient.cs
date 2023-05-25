using System.ComponentModel.DataAnnotations.Schema;

namespace OurRecipes.Data.Models
{
    public class Ingredient
    {
        public Ingredient()
        {
            
        }
        
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey(nameof(Component))]
        public  int ComponentId { get; set; }
        public virtual Component Component { get; set; }

    }
}