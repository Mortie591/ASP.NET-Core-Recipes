﻿using System.Text.Json.Serialization;

namespace DataSeeder.Models.ImportDtos
{
    public class TagDto
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

}
