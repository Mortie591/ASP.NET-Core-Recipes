using System.Text.Json.Serialization;

namespace OurRecipes.Services.Models.ImportDtos
{
    /*
     *  public string Title { get; set; }
        public string? Description { get; set; }
        public byte? Servings { get; set; }
        public ushort? PrepTime { get; set; }
        public ushort? CookTime { get; set; }
        public ushort? TotalTime => (ushort?)(CookTime + PrepTime);
        public ushort Likes { get; set; } //connect with user? -> My Favourite recipes (all liked ones)
        public string? ImageUrl { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public bool? IsDeleted { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public ICollection<Section> Sections { get; set; }
        public virtual ICollection<Component> Components { get; set; }
        public string Instructions { get; set; }
        public virtual ICollection<Nutrient> Nutrients { get; set; } 
        public virtual ICollection<Tag> Tags { get; set; }
    */
    public class RecipeDto
    {
        [JsonPropertyName("name")]
        public string Title { get; set; }

        [JsonPropertyName("cook_time_minutes")]
        public ushort? CookTime { get; set; }

        [JsonPropertyName("prep_time_minutes")]
        public ushort? PrepTime { get; set; }

        [JsonPropertyName("sections")]
        public ICollection<SectionDto> Sections { get; set; }
        [JsonPropertyName("tags")]

        public ICollection<TagDto> Tags { get; set; }

        [JsonPropertyName("thumbnail_url")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("num_servings")]
        public int Servings { get; set; }
        [JsonPropertyName("nutrition")]
        public ICollection<NutritionDto> Nutritients { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("instructions")]
        public ICollection<InstructionDto> Instructions { get; set; }
        [JsonPropertyName("topics")]
        public ICollection<TopicDto> Categories { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("created_at")]
        public string CreatedOnDate { get; set; }

        

    }

}
