using Icarus_Item_Calculator.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Icarus_Item_Calculator.Services
{
    public class ItemServices
    {
        private readonly ItemContext _context;

        public ItemServices(ItemContext context)
        {
            _context = context;
        }
        public async Task LoadNestedRecipesAsync(Item item)
        {
            foreach (var recipeItem in item.Recipe)
            {
                if (recipeItem.Item != null)
                {
                    await _context.Entry(recipeItem.Item)
                        .Collection(i => i.Recipe)
                        .Query()
                        .Include(r => r.Item)
                        .LoadAsync();

                    await LoadNestedRecipesAsync(recipeItem.Item);
                }
            }
        }
        public (List<RecipeStep>, Dictionary<string, double>) CalculateRecipeSteps(Item item, double quantity)
        {
            List<RecipeStep> steps = new List<RecipeStep>();
            Dictionary<string, double> baseItemsTotal = new Dictionary<string, double>();
            CalculateStepsRecursive(item, quantity, steps, new Dictionary<int, Dictionary<string, double>>(), baseItemsTotal);
            return (steps, baseItemsTotal);
        }
        private void CalculateStepsRecursive(Item item, double quantity, List<RecipeStep> steps,
            Dictionary<int, Dictionary<string, double>> accumulatedIngredients, Dictionary<string, double> baseItemsTotal)
        {
            if (item == null) return;

            accumulatedIngredients[item.ItemId] = new Dictionary<string, double>();

            steps.Add(new RecipeStep
            {
                ItemName = item.Name,
                Quantity = quantity,
                Ingredients = item.Recipe.Select(r =>
                {
                    double totalQuantity = r.Quantity * quantity;

                    accumulatedIngredients[item.ItemId][r.Item.Name] = totalQuantity;

                    if (r.Item.IsBaseItem)
                    {
                        if (baseItemsTotal.ContainsKey(r.Item.Name))
                        {
                            baseItemsTotal[r.Item.Name] += totalQuantity;
                        }
                        else
                        {
                            baseItemsTotal[r.Item.Name] = totalQuantity;
                        }
                    }

                    return new IngredientStep
                    {
                        ItemName = r.Item.Name,
                        Quantity = accumulatedIngredients[item.ItemId][r.Item.Name],
                        IsBase = r.Item.IsBaseItem
                    };
                }).ToList()
            });

            foreach (var recipeItem in item.Recipe.Where(r => !r.Item.IsBaseItem))
            {
                CalculateStepsRecursive(recipeItem.Item, recipeItem.Quantity * quantity, steps, accumulatedIngredients, baseItemsTotal);
            }
        }
    }
}
