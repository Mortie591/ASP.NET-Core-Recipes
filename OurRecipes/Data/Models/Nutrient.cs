using System.ComponentModel.DataAnnotations;

namespace OurRecipes.Data.Models
{
    public class Nutrient
    {
        //protein:"2.4g"
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public float Quantity { get; set; }
    }
}