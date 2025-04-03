using Microsoft.AspNetCore.Mvc.Rendering;

namespace Icarus_Item_Calculator.Models
{
    public class ItemWithStepsViewModel
    {
        public Item Item { get; set; }
        public List<RecipeStep> RecipeSteps { get; set; }
        public double Quantity { get; set; }
        public Dictionary<string, double> BaseItemsTotal { get; set; }
        public int? SelectedRecipeId { get; set; } // Nullable to allow "no selection" initially
        public List<SelectListItem> AvailableRecipes { get; set; } // For the dropdown
    }
}
