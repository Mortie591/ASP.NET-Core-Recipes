namespace DataSeeder.Models.ScraperDtos
{
    public class RecipeDto
    {
        public RecipeDto()
        {
            this.Categories = new HashSet<string>();
            this.Components = new HashSet<ComponentDto>();
            this.Nutrients = new HashSet<NutrientDto>();
            this.Tags = new HashSet<string>();
        }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Servings { get; set; }
        public string? PrepTime { get; set; }
        public string? CookTime { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public ICollection<string> Categories { get; set; }
        public ICollection<ComponentDto> Components { get; set; }
        public string Instructions { get; set; }
        public ICollection<NutrientDto> Nutrients { get; set; }

        public ICollection<string> Tags { get; set; }
        public string OriginalUrl { get; set; }
    }
}
