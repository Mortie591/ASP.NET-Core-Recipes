namespace OurRecipes.Data.Models
{
    public class Recipe
    {
        public Recipe()
        {
            this.Comments = new HashSet<Comment>();
            this.Tags = new HashSet<Tag>();
            this.Ingredients = new HashSet<Ingredient>();
            this.Instructions = new HashSet<Instruction>();
            this.Nutrients = new HashSet<Nutrient>();
        }
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public byte Servings { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public virtual Time Time { get; set; }
        public ushort Votes { get; set; } //connect with user? -> My Favourite recipes (all liked ones)
        public virtual ICollection<Ingredient> Ingredients { get; set; }
        public virtual ICollection<Instruction> Instructions { get; set; }
        public virtual ICollection<Nutrient> Nutrients { get; set; } //nutrient example - protein:"2.4g"
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
