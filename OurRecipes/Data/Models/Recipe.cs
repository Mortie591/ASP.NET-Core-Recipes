namespace OurRecipes.Data.Models
{
    public class Recipe
    {
        public Recipe()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Categories = new HashSet<Category>();
            this.Comments = new HashSet<Comment>();
            this.Sections = new Dictionary<string, ICollection<Component>>();
            this.Tags = new HashSet<Tag>();
            this.Components = new HashSet<Component>();
            this.Instructions = new HashSet<Instruction>();
            this.Nutrients = new HashSet<Nutrient>();
        }
        public string Id { get; }
        public string Title { get; set; }
        public string Description { get; set; }
        public byte Servings { get; set; }
        public virtual Time Time { get; set; }
        public ushort Likes { get; set; } //connect with user? -> My Favourite recipes (all liked ones)
        public string ImageUrl { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public IDictionary<string,ICollection<Component>> Sections { get; set; }
        public virtual ICollection<Component> Components { get; set; }
        public virtual ICollection<Instruction> Instructions { get; set; }
        public virtual ICollection<Nutrient> Nutrients { get; set; } 
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public string AuthorId { get; set; }
        public AppIdentityUser Author { get; set; }
    }
}
