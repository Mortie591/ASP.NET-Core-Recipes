﻿using System.Text.Json.Serialization;

namespace OurRecipes.Services.Models.ImportDtos
{
    public class IngredientDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("display_plural")]
        public string NamePlural { get; set; }
    }

}
