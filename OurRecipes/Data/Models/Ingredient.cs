﻿namespace OurRecipes.Data.Models
{
    public class Ingredient
    {
        public Ingredient()
        {
            this.Recipes = new HashSet<Recipe>();
        }
        //"<Quantity> <unit> unsalted butter or margarine"
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}