using System.ComponentModel.DataAnnotations;

public class ComponentInputModel
{
    [Required,MinLength(3),MaxLength(255)]
    public string IngredientName { get; set; }
    [Required, MinLength(1), MaxLength(255)]
    public string Quantity { get; set; }
    public string? Unit { get; set; }
    public string? Text { get; set; }
}
