namespace OurRecipes.Data.Models
{
    public class Instruction
    {
        /*"Heat butter and olive oil in a large saucepan over medium-low heat and 
         * cook onion and garlic until onion is soft and translucent, about 5 minutes. 
         * Add tomatoes, water, sugar, salt, pepper, red pepper flakes, celery seed, and oregano. 
         * Bring to a boil. Reduce heat, cover, and simmer for 15 minutes."
         * */
        public int Id { get; set; }
        public byte Step { get; set; }
        public string Content { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}