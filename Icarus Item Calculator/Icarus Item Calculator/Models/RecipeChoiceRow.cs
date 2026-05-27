using Microsoft.AspNetCore.Mvc.Rendering;

namespace Icarus_Item_Calculator.Models
{
    public class RecipeChoiceRow
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = "";
        public int? SelectedRecipeId { get; set; }  // user choice
        public List<SelectListItem> RecipeOptions { get; set; } = [];

    }
}
