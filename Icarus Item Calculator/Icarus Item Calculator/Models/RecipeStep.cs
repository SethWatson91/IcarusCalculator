namespace Icarus_Item_Calculator.Models
{
    public class RecipeStep
    {
        public string ItemName { get; set; }
        public double Quantity { get; set; }
        public List<IngredientStep> Ingredients { get; set; }
    }
}
