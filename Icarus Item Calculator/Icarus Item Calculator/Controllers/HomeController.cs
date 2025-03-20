using Icarus_Item_Calculator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Icarus_Item_Calculator.Services;

namespace Icarus_Item_Calculator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ItemContext _context;
        private readonly ItemServices _itemServices;


        public HomeController(ItemContext context, ItemServices itemServices)
        {
            _context = context;
            _itemServices = itemServices;
        }


        // GET: Home/Index
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["IdSortParm"] = sortOrder == "Id" ? "id_desc" : "Id";
            ViewData["CurrentFilter"] = searchString;

            var items = from i in _context.Items
                        .Include(i => i.Recipes)
                        .ThenInclude(r => r.Ingredients)
                        .ThenInclude(ri => ri.Item)
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
            // Pass the list of items as a strongly-typed collection
            ViewData["AvailableItems"] = _context.Items
                .Select(i => new SelectListItem
                {
                    Value = i.ItemId.ToString(),
                    Text = i.Name
                })
                .ToList();

            return View(new Item());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name, IsBaseItem")] Item item,
                                                        int[] selectedItems, Dictionary<int, double> quantities)
        {
            if (ModelState.IsValid)
            {
                var recipe = new Recipe { Item = item };
                item.Recipes.Add(recipe);

                if (selectedItems != null && selectedItems.Any() && !selectedItems.Contains(0))
                {
                    foreach (var itemId in selectedItems)
                    {
                        var ingredient = await _context.Items.FindAsync(itemId);
                        if (ingredient != null && quantities.TryGetValue(itemId, out double quantity))
                        {
                            recipe.Ingredients.Add(new RecipeItem
                            {
                                ItemId = itemId,
                                Quantity = quantity,
                                Recipe = recipe
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
                .Include(i => i.Recipes)
                .ThenInclude(r => r.Ingredients)
                .ThenInclude(ri => ri.Item)
                .FirstOrDefaultAsync(m => m.ItemId == id);

            if (item == null)
            {
                return NotFound();
            }

            // Pass the list of items as a strongly-typed collection
            ViewData["AvailableItems"] = _context.Items
                .Select(i => new SelectListItem
                {
                    Value = i.ItemId.ToString(),
                    Text = i.Name
                })
                .ToList();

            return View(item);
        }


        // POST: Home/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemId,Name,IsBaseItem")] Item item,
                                      int[] selectedItems, Dictionary<int, double> quantities)
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
                        .Include(i => i.Recipes)
                        .ThenInclude(r => r.Ingredients)
                        .FirstOrDefaultAsync(m => m.ItemId == id);

                    if (existingItem == null)
                    {
                        return NotFound();
                    }

                    existingItem.Name = item.Name;
                    existingItem.IsBaseItem = item.IsBaseItem;

                    var recipe = existingItem.Recipes.FirstOrDefault() ?? new Recipe { ItemId = existingItem.ItemId };
                    if (!existingItem.Recipes.Any())
                    {
                        existingItem.Recipes.Add(recipe);
                    }

                    recipe.Ingredients.Clear();
                    if (selectedItems != null && selectedItems.Any() && !selectedItems.Contains(0))
                    {
                        foreach (var itemId in selectedItems)
                        {
                            var ingredient = await _context.Items.FindAsync(itemId);
                            if (ingredient != null && quantities.TryGetValue(itemId, out double quantity))
                            {
                                recipe.Ingredients.Add(new RecipeItem
                                {
                                    ItemId = itemId,
                                    Quantity = quantity,
                                    RecipeId = recipe.RecipeId
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
        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.ItemId == id);
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
            var item = await _context.Items
                .Include(i => i.Recipes)              // Updated from Recipe to Recipes
                .ThenInclude(r => r.Ingredients)      // Include Ingredients for checking dependencies
                .FirstOrDefaultAsync(i => i.ItemId == id);

            if (item == null)
            {
                return NotFound();
            }

            try
            {
                _context.Items.Remove(item); // This will cascade delete Recipes and their Ingredients
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                // Check if the item is used as an ingredient in any RecipeItem
                if (_context.RecipeItems.Any(ri => ri.ItemId == id))
                {
                    ModelState.AddModelError("", "Cannot delete this item because it is used as an ingredient in other recipes.");
                    return View(item); // Return to the Delete view with the error
                }
                throw; // Re-throw if it's another issue
            }
        }




        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Recipes)              // Updated from Recipe to Recipes
                .ThenInclude(r => r.Ingredients)      // Include the Ingredients collection
                .ThenInclude(ri => ri.Item)           // Include the Item for each RecipeItem
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
                .Include(i => i.Recipes)
                .ThenInclude(r => r.Ingredients)
                .ThenInclude(ri => ri.Item)
                .FirstOrDefaultAsync(i => i.ItemId == id);

            if (item == null)
            {
                return NotFound();
            }

            var model = new ItemWithStepsViewModel
            {
                Item = item,
                RecipeSteps = new List<RecipeStep>(),
                Quantity = quantity
            };

            foreach (var recipe in item.Recipes)
            {
                await _itemServices.LoadNestedRecipesAsync(recipe);
                var (recipeSteps, baseItemsTotal) = _itemServices.CalculateRecipeSteps(recipe, quantity);
                model.RecipeSteps.AddRange(recipeSteps);
                model.BaseItemsTotal = baseItemsTotal; // Note: This overwrites for each recipe; adjust as needed
            }

            return View(model);
        }
        private async Task LoadNestedRecipesAsync(Recipe recipe)
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
    }
}