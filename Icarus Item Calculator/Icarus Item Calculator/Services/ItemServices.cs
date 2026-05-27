using Icarus_Item_Calculator.Models;
using Microsoft.EntityFrameworkCore;

namespace Icarus_Item_Calculator.Services
{
    public class ItemServices(ItemContext context)
    {
        private readonly ItemContext _context = context;

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

        public (List<RecipeStep>, Dictionary<string, double>) CalculateRecipeSteps(
            Recipe recipe,
            double quantity,
            Dictionary<int, int>? recipeChoiceByItemId)
        {
            List<RecipeStep> steps = [];
            Dictionary<string, double> baseItemsTotal = [];
            CalculateStepsRecursive(recipe, quantity, steps, [], baseItemsTotal, recipeChoiceByItemId);
            return (steps, baseItemsTotal);
        }

        private static void CalculateStepsRecursive(Recipe recipe, double quantity, List<RecipeStep> steps,
            Dictionary<int, Dictionary<string, double>> accumulatedIngredients, Dictionary<string, double> baseItemsTotal, Dictionary<int, int>? recipeChoiceByItemId)
        {
            accumulatedIngredients[recipe.RecipeId] = [];

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

            foreach (var recipeItem in recipe.Ingredients.Where(r => r.Item != null && !r.Item.IsBaseItem))
            {
                var craftedItem = recipeItem.Item;
                
                Recipe? chosenRecipe = null;

                if (recipeChoiceByItemId != null && recipeChoiceByItemId.TryGetValue(craftedItem.ItemId, out var chosenRecipeId))
                {
                    chosenRecipe = craftedItem.Recipes?.FirstOrDefault(r => r.RecipeId == chosenRecipeId);
                }

                chosenRecipe ??= craftedItem.Recipes?.FirstOrDefault();

                if ( chosenRecipe == null)
                {
                    // This is 1.5 (missing recipe) — for now just skip or throw
                    continue;
                }
                CalculateStepsRecursive(
                    chosenRecipe,
                    recipeItem.Quantity * quantity,
                    steps,
                    accumulatedIngredients,
                    baseItemsTotal,
                    recipeChoiceByItemId);
            }
        }
        public List<Item> CollectCraftedItemsNeedingChoice(Recipe rootRecipe)
        {
            var result = new Dictionary<int, Item>();
            var visitedRecipeIds = new HashSet<int>();

            void WalkRecipe(Recipe recipe)
            {
                if (!visitedRecipeIds.Add(recipe.RecipeId)) return; // Avoid cycles

                foreach (var ri in recipe.Ingredients)
                {
                    if (ri.Item == null) continue;
                    if (ri.Item.IsBaseItem) continue;

                    // This ingredient is crafted. If it has >1 recipe, user should pick.
                    if (ri.Item.Recipes != null && ri.Item.Recipes.Count > 1)
                        result[ri.Item.ItemId] = ri.Item;

                    // Walk into all recipes so we can discover deeper crafted items too.
                    if (ri.Item.Recipes != null)
                    {
                        foreach (var childRecipe in ri.Item.Recipes)
                        {
                            WalkRecipe(childRecipe);
                        }
                    }
                }

            }
            WalkRecipe(rootRecipe);
            return result.Values.OrderBy(i => i.Name).ToList();
        }
    }
}
