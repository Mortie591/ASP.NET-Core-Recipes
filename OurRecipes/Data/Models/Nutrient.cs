using System.ComponentModel.DataAnnotations;

namespace OurRecipes.Data.Models
{
    public class Nutrient
    {
        //protein:"2.4g"
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        public string Quantity { get; set; }
        public int? UnitId { get; set; }
        public virtual Unit? Unit { get; set; }
    }
}