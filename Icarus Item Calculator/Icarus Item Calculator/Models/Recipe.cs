using System.ComponentModel.DataAnnotations;

namespace Icarus_Item_Calculator.Models
{
    public class Recipe
    {
        [Key]
        public int RecipeId { get; set; }

        [Required]
        public int ItemId { get; set; } // Foreign key to the crafted Item
        public Item Item { get; set; }   // Navigation property

        public List<RecipeItem> Ingredients { get; set; } = new List<RecipeItem>(); // Ingredients for this recipe
    }
}
