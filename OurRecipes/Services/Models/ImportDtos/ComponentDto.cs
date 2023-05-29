using System.Text.Json.Serialization;

namespace OurRecipes.Services.Models.ImportDtos
{
    public class ComponentDto
    {
        [JsonPropertyName("raw_text")]
        public string Text { get; set; }
        [JsonPropertyName("ingredient")]
        public IngredientDto Ingredient { get; set; }

        public ICollection<MeasurementDto> measurements { get; set; }
        public string Quantity { get; set; } 
        public string? Unit { get; set; }
    }

}
