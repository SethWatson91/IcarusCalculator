using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Icarus_Item_Calculator.Models
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public bool IsBaseItem { get; set; } = false;

        public List<Recipe> Recipes { get; set; } = new List<Recipe>();

    }
}
