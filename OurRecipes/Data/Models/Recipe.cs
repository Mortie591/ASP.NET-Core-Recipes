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
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        public byte? Servings { get; set; }
        public ushort? PrepTime { get; set; }
        public ushort? CookTime { get; set; }
        public ushort? TotalTime => (ushort?)(CookTime + PrepTime);
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
