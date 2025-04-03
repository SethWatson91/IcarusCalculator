using Icarus_Item_Calculator.Models;
using Microsoft.EntityFrameworkCore;

namespace Icarus_Item_Calculator.Services
{
    public class ItemServices
    {
        private readonly ItemContext _context;

        public ItemServices(ItemContext context)
        {
            _context = context;
        }

        public async Task LoadNestedRecipesAsync(Recipe recipe)
        {
            foreach (var recipeItem in recipe.Ingredients)
            {
                if (recipeItem.Item != null)
                {
                    await _context.Entry(recipeItem.Item)
                        .Collection(i => i.Recipes)
                        .Query()
                        .Include(r => r.Ingredients)
                        .ThenInclude(ri => ri.Item)
                        .LoadAsync();

                    foreach (var nestedRecipe in recipeItem.Item.Recipes)
                    {
                        await LoadNestedRecipesAsync(nestedRecipe);
                    }
                }
            }
        }

        public (List<RecipeStep>, Dictionary<string, double>) CalculateRecipeSteps(Recipe recipe, double quantity)
        {
            List<RecipeStep> steps = new List<RecipeStep>();
            Dictionary<string, double> baseItemsTotal = new Dictionary<string, double>();
            CalculateStepsRecursive(recipe, quantity, steps, new Dictionary<int, Dictionary<string, double>>(), baseItemsTotal);
            return (steps, baseItemsTotal);
        }

        private void CalculateStepsRecursive(Recipe recipe, double quantity, List<RecipeStep> steps,
            Dictionary<int, Dictionary<string, double>> accumulatedIngredients, Dictionary<string, double> baseItemsTotal)
        {
            if (recipe == null || recipe.Item == null) return;

            accumulatedIngredients[recipe.RecipeId] = new Dictionary<string, double>();

            steps.Add(new RecipeStep
            {
                ItemName = recipe.Item.Name,
                Quantity = quantity,
                Ingredients = recipe.Ingredients.Select(r =>
                {
                    double totalQuantity = r.Quantity * quantity;

                    accumulatedIngredients[recipe.RecipeId][r.Item.Name] = totalQuantity;

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
                        Quantity = totalQuantity,
                        IsBase = r.Item.IsBaseItem
                    };
                }).ToList()
            });

            foreach (var recipeItem in recipe.Ingredients.Where(r => !r.Item.IsBaseItem))
            {
                var firstRecipe = recipeItem.Item.Recipes.FirstOrDefault(); // Use the first recipe for simplicity
                if (firstRecipe != null)
                {
                    CalculateStepsRecursive(firstRecipe, recipeItem.Quantity * quantity, steps, accumulatedIngredients, baseItemsTotal);
                }
            }
        }
    }
}
