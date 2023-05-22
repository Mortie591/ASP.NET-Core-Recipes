namespace OurRecipes.Data.Models
{
    public class Recipe
    {
        public Recipe()
        {
            this.Categories = new HashSet<Category>();
            this.Comments = new HashSet<Comment>();
            this.Tags = new HashSet<Tag>();
            this.Ingredients = new Dictionary<Ingredient,float>();
            this.Instructions = new HashSet<Instruction>();
            this.Nutrients = new HashSet<Nutrient>();
        }
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public byte Servings { get; set; }
        public virtual Time Time { get; set; }
        public ushort Votes { get; set; } //connect with user? -> My Favourite recipes (all liked ones)
        public string ImageUrl { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual IDictionary<Ingredient,float> Ingredients { get; set; }
        public virtual ICollection<Instruction> Instructions { get; set; }
        public virtual ICollection<Nutrient> Nutrients { get; set; } //nutrient example - protein:"2.4g"
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
