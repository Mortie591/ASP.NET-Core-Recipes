using System.ComponentModel.DataAnnotations;

public class NutrientInputModel
{
    [Required, MinLength(3), MaxLength(30)]
    public string Name { get; set; }
    [Required, MinLength(3), MaxLength(30)]
    [RegularExpression("^[\\d]+[\\w]*$")]
    public string Quantity { get; set; }
}