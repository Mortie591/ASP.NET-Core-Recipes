using System.Text.Json.Serialization;

namespace Recipes.DataImport.Models
{
    public class SectionDto
    {
        public SectionDto()
        {
            this.Components = new HashSet<ComponentDto>();
        }
        [JsonPropertyName("components")]
        public ICollection<ComponentDto> Components { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

}
