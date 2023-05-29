using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OurRecipes.Data.Models
{
    public class Ingredient
    {
        [Key]
        public string Name { get; set; }

        public string? NamePlural { get; set; }

    }
}