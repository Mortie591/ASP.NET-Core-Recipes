using System.Text.Json.Serialization;

namespace OurRecipes.Services.Models.ImportDtos
{
    public class SectionDto
    {
        public SectionDto()
        {
            Components = new HashSet<ComponentDto>();
        }
        [JsonPropertyName("components")]
        public ICollection<ComponentDto> Components { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

}
