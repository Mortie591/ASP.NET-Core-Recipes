
using System.ComponentModel.DataAnnotations;

namespace OurRecipes.Data.Models
{
    public class Unit
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
