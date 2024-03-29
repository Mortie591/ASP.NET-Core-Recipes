﻿using System.ComponentModel.DataAnnotations;

namespace OurRecipes.Data.Models
{
    public class Category
    {
        public Category()
        {
            this.Recipes = new HashSet<Recipe>();
        }
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        public string? Type { get; set; }
        public string? imageUrl { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}
