using System.ComponentModel.DataAnnotations;

namespace Icarus_Item_Calculator.Models
{
    public class RecipeItem
    {
        [Key]
        public int RecipeItemId { get; set; }

        [Required]
        public int ItemId { get; set; }
        public Item Item { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Required]
        public int ParentItemId { get; set; }
        public Item ParentItem { get; set; }
    }
}
