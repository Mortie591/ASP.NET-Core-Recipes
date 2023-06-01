
using System.ComponentModel.DataAnnotations;

namespace OurRecipes.Data.Models
{
    public class Unit
    {
        public int Id { get; set; }

        [Required, MaxLength(40)]
        public string Name { get; set; }
    }
}
