using ODataSeeder.Models.ImportDtos;
using System.Text.Json.Serialization;

namespace DataSeeder.Models.ImportDtos
{
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
