
using System.ComponentModel.DataAnnotations;

namespace OurRecipes.Data.Models
{
    public class Unit
    {
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string Name { get; set; }
    }
}
