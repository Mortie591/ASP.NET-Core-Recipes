using System.Text.Json.Serialization;

namespace Recipes.DataImport.Models
{
    public class IngredientDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("display_plural")]
        public string NamePlural { get; set; }
    }

}
