using System.Text.Json.Serialization;

namespace Recipes.DataImport.Models
{
    public class ComponentDto
    {
        [JsonPropertyName("raw_text")]
        public string Text { get; set; }
        [JsonPropertyName("ingredient")]
        public IngredientDto Ingredient { get; set; }

        public ICollection<MeasurementDto> measurements { get; set; }
    }

}
