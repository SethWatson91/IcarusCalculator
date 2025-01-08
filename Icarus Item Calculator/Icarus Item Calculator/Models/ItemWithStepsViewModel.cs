namespace Icarus_Item_Calculator.Models
{
    public class ItemWithStepsViewModel
    {
        public Item Item { get; set; }
        public List<RecipeStep> RecipeSteps { get; set; }
        public double Quantity { get; set; }
        public Dictionary<string, double> BaseItemsTotal { get; set; }
    }
}
