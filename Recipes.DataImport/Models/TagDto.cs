using System.Text.Json.Serialization;

namespace Recipes.DataImport.Models
{
    public class TagDto
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

}
