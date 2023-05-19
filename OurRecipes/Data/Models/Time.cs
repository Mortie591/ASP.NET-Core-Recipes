namespace OurRecipes.Data.Models
{
    public class Time
    {
        /*
        repration_time:"10 mins "
        cooking_time:"20 mins "
        additional_time:""
        total:"30 mins "
         */
        public int Id { get; set; }
        public TimeSpan PreparationTime { get; set; }
        public TimeSpan CookingTime { get; set; }
        public TimeSpan AdditionalTime { get; set; }
        public TimeSpan TotalTime =>PreparationTime + CookingTime + AdditionalTime;
        public virtual Recipe Recipe { get; set; }
        public string RecipeId { get; set; }
    }
}