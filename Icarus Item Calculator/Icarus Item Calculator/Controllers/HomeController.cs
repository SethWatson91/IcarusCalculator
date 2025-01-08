using Icarus_Item_Calculator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Icarus_Item_Calculator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ItemContext _context;

        public HomeController(ItemContext context)
        {
            _context = context;
        }

        // GET: Home/Index
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["IdSortParm"] = sortOrder == "Id" ? "id_desc" : "Id";
            ViewData["CurrentFilter"] = searchString;

            var items = from i in _context.Items
                        .Include(i => i.Recipe)
                        .ThenInclude(r => r.Item)
                        select i;

            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(i => i.Name);
                    break;
                case "Id":
                    items = items.OrderBy(i => i.ItemId);
                    break;
                case "id_desc":
                    items = items.OrderByDescending(i => i.ItemId);
                    break;
                default:
                    items = items.OrderBy(i => i.Name);
                    break;
            }

            return View(await items.AsNoTracking().ToListAsync());
        }

        // GET: Home/Create
        public IActionResult Create()
        {
            ViewBag.Items = new SelectList(_context.Items, "ItemId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IsCraftable,IsCraftingInterface")] Item item, int[] selectedItems, Dictionary<double, double> quantities)
        {
            if (ModelState.IsValid)
            {
                if (selectedItems.Contains(0))
                {
                    item.Recipe = new List<RecipeItem>();
                }
                else
                {
                    item.Recipe = new List<RecipeItem>();
                    foreach (var itemId in selectedItems)
                    {
                        var recipeItem = await _context.Items.FindAsync(itemId);
                        if (recipeItem != null && quantities.TryGetValue(itemId, out double quantity))
                        {
                            item.Recipe.Add(new RecipeItem
                            {
                                ItemId = itemId,
                                Quantity = quantity,
                                ParentItemId = item.ItemId
                            });
                        }
                    }
                }

                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Items = new SelectList(_context.Items, "ItemId", "Name");
            return View(item);
        }

        // GET: Home/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Recipe)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            ViewBag.Items = new SelectList(_context.Items, "ItemId", "Name");
            return View(item);
        }

        // POST: Home/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemId,Name,IsCraftable,IsCraftingInterface")] Item item, int[] selectedItems, Dictionary<double, double> quantities)
        {
            if (id != item.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingItem = await _context.Items
                        .Include(i => i.Recipe)
                        .FirstOrDefaultAsync(m => m.ItemId == id);

                    if (existingItem == null)
                    {
                        return NotFound();
                    }

                    existingItem.Name = item.Name;

                    if (selectedItems.Contains(0))
                    {
                        existingItem.Recipe.Clear();
                    }
                    else
                    {
                        existingItem.Recipe.Clear();
                        foreach (var itemId in selectedItems)
                        {
                            var recipeItem = await _context.Items.FindAsync(itemId);
                            if (recipeItem != null && quantities.TryGetValue(itemId, out double quantity))
                            {
                                existingItem.Recipe.Add(new RecipeItem
                                {
                                    ItemId = itemId,
                                    Quantity = quantity,
                                    ParentItemId = existingItem.ItemId
                                });
                            }
                        }
                    }

                    _context.Update(existingItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ItemId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Items = new SelectList(_context.Items, "ItemId", "Name");
            return View(item);
        }

        // GET: Home/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Items.FindAsync(id);
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.ItemId == id);
        }

        private async Task LoadNestedRecipes(Item item)
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

                    await LoadNestedRecipes(recipeItem.Item);
                }
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Recipe)
                    .ThenInclude(r => r.Item)
                .FirstOrDefaultAsync(m => m.ItemId == id);

            if (item == null)
            {
                return NotFound();
            }
            var model = new ItemWithStepsViewModel
            {
                Item = item,
                RecipeSteps = new List<RecipeStep>(),
                Quantity = 1
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(int id, double quantity)
        {
            var item = await _context.Items
                .Include(i => i.Recipe)
                    .ThenInclude(r => r.Item)
                .FirstOrDefaultAsync(i => i.ItemId == id);

            if (item == null)
            {
                return NotFound();
            }

            await LoadNestedRecipes(item);

            var (recipeSteps, baseItemsTotal) = CalculateRecipeSteps(item, quantity);

            var model = new ItemWithStepsViewModel
            {
                Item = item,
                RecipeSteps = recipeSteps,
                Quantity = quantity,
                BaseItemsTotal = baseItemsTotal  // Add this to your view model
            };

            return View(model);
        }

        private (List<RecipeStep>, Dictionary<string, double>) CalculateRecipeSteps(Item item, double quantity)
        {
            List<RecipeStep> steps = new List<RecipeStep>();
            Dictionary<string, double> baseItemsTotal = new Dictionary<string, double>();
            // Recursive function to break down the recipe
            CalculateStepsRecursive(item, quantity, steps, new Dictionary<int, Dictionary<string, double>>(), baseItemsTotal);
            return (steps, baseItemsTotal);
        }
        private void CalculateStepsRecursive(Item item, double quantity, List<RecipeStep> steps, Dictionary<int, Dictionary<string, double>> accumulatedIngredients, Dictionary<string, double> baseItemsTotal)
        {
            if (item == null) return;
            // We only initialize for the current item because we want to track for this specific run
            accumulatedIngredients[item.ItemId] = new Dictionary<string, double>();

            steps.Add(new RecipeStep
            {
                ItemName = item.Name,
                Quantity = quantity,
                Ingredients = item.Recipe.Select(r =>
                {
                    // calculate total quantity needed for ingredient
                    double totalQuantity = r.Quantity * quantity;   
                    
                    accumulatedIngredients[item.ItemId][r.Item.Name] = totalQuantity;

                    // If it's a base item, add or update in baseItemsTotal
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