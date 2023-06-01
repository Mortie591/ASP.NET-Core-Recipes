using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OurRecipes.Data.Models
{
    public class Recipe
    {
        public Recipe()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Categories = new HashSet<Category>();
            this.Comments = new HashSet<Comment>();
            this.Sections = new HashSet<Section>();
            this.Tags = new HashSet<Tag>();
            this.Components = new HashSet<Component>();
            this.Nutrients = new HashSet<Nutrient>();
            this.UserFavourites = new HashSet<UserFavourite>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        public string? Description { get; set; }
        [MaxLength(20)]
        public string Servings { get; set; }
        [MaxLength(20)]
        public string? PrepTime { get; set; }
        [MaxLength(20)]
        public string? CookTime { get; set; }
        public ushort Likes { get; set; } = 0;//connect with user? -> My Favourite recipes (all liked ones)
        public string? ImageUrl { get; set; }
        public string? OriginalUrl { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public virtual ICollection<Category> Categories { get; set; }
        public ICollection<Section>? Sections { get; set; }
        public virtual ICollection<Component> Components { get; set; }
        public string Instructions { get; set; }
        public virtual ICollection<Nutrient> Nutrients { get; set; } 
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        
        [ForeignKey(nameof(AppIdentityUser))]
        public string? AuthorId { get; set; }
        public AppIdentityUser? Author { get; set; }
        public ICollection<UserFavourite>? UserFavourites { get; set; }
    }
}
