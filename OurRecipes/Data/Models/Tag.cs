using System.ComponentModel.DataAnnotations;

namespace OurRecipes.Data.Models
{
    public class Tag
    {
        /*
         * id:64444
         * name:"north_american"
         * display_name:"North American"
         * type:"cuisine"
         * root_tag_type:"cuisine"

        */
        public int Id { get; set; }
        [Required,MaxLength(50)]
        public string Name { get; set; }
        [Required, MaxLength(50)]
        public string Type { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }

    }
}