using System.ComponentModel.DataAnnotations;

namespace Icarus_Item_Calculator.Models
{
    public class RecipeItem
    {
        [Key]
        public int RecipeItemId { get; set; }

        [Required]
        public int ItemId { get; set; }
        public Item? Item { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        public double Quantity { get; set; }

        [Required]
        public int RecipeId { get; set; }
        public Recipe? Recipe { get; set; }
    }
}
