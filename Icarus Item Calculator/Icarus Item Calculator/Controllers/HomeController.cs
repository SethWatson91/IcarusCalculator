using Icarus_Item_Calculator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Icarus_Item_Calculator.Services;
using Microsoft.AspNetCore.Authorization;

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
        [AllowAnonymous]
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
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            // Pass the list of items for ingredient selection
            ViewData["AvailableItems"] = _context.Items
                .Select(i => new SelectListItem
                {
                    Value = i.ItemId.ToString(),
                    Text = i.Name
                })
                .ToList();

            // Set up AvailableRecipes with only "Create New Recipe" for a new item
            ViewData["AvailableRecipes"] = new List<SelectListItem>
    {
        new SelectListItem
        {
            Value = "new",
            Text = "Create New Recipe"
        }
    };

            return View(new Item());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Name,IsBaseItem")] Item item,
                                                string selectedRecipe, int[] selectedItems, Dictionary<int, double> quantities)
        {
            if (ModelState.IsValid)
            {
                var newItem = new Item
                {
                    Name = item.Name,
                    IsBaseItem = item.IsBaseItem,
                    Recipes = new List<Recipe>()
                };

                if (selectedRecipe == "new" && selectedItems != null && selectedItems.Any() && !selectedItems.Contains(0))
                {
                    var recipe = new Recipe { Item = newItem };
                    newItem.Recipes.Add(recipe);

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

                _context.Add(newItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Repopulate ViewData if validation fails
            ViewData["AvailableItems"] = _context.Items
                .Select(i => new SelectListItem
                {
                    Value = i.ItemId.ToString(),
                    Text = i.Name
                })
                .ToList();
            ViewData["AvailableRecipes"] = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "new",
                    Text = "Create New Recipe"
                }
            };

            return View(item);
        }


        [Authorize(Roles = "Admin")]
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

            // Pass the list of items for ingredient selection
            ViewData["AvailableItems"] = _context.Items
                .Select(i => new SelectListItem
                {
                    Value = i.ItemId.ToString(),
                    Text = i.Name
                })
                .ToList();

            // Build the list of recipes and add the "Create New Recipe" option
            var availableRecipes = item.Recipes
                .Select(r => new SelectListItem
                {
                    Value = r.RecipeId.ToString(),
                    Text = $"Recipe #{r.RecipeId}"
                })
                .ToList();
            availableRecipes.Add(new SelectListItem
            {
                Value = "new",
                Text = "Create New Recipe"
            });
            ViewData["AvailableRecipes"] = availableRecipes;

            return View(item);
        }


        // POST: Home/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ItemId,Name,IsBaseItem")] Item item,
                                      string selectedRecipe, int[] selectedItems, Dictionary<int, double> quantities)
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

                    Recipe recipe;
                    if (selectedRecipe == "new")
                    {
                        recipe = new Recipe { ItemId = existingItem.ItemId };
                        existingItem.Recipes.Add(recipe);
                    }
                    else
                    {
                        int recipeId = int.Parse(selectedRecipe);
                        recipe = existingItem.Recipes.FirstOrDefault(r => r.RecipeId == recipeId);
                        if (recipe == null)
                        {
                            return NotFound();
                        }
                    }

                    // Clear and update ingredients
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

            // Repopulate ViewData if validation fails
            ViewData["AvailableItems"] = _context.Items
                .Select(i => new SelectListItem
                {
                    Value = i.ItemId.ToString(),
                    Text = i.Name
                })
                .ToList();

            var availableRecipes = _context.Items
                .Include(i => i.Recipes)
                .FirstOrDefault(i => i.ItemId == id)?.Recipes
                .Select(r => new SelectListItem
                {
                    Value = r.RecipeId.ToString(),
                    Text = $"Recipe #{r.RecipeId}"
                })
                .ToList() ?? new List<SelectListItem>();
            availableRecipes.Add(new SelectListItem
            {
                Value = "new",
                Text = "Create New Recipe"
            });
            ViewData["AvailableRecipes"] = availableRecipes;

            return View(item);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRecipeIngredients(int recipeId)
        {
            var recipe = await _context.Recipes
                .Include(r => r.Ingredients)
                .ThenInclude(ri => ri.Item)
                .FirstOrDefaultAsync(r => r.RecipeId == recipeId);

            if (recipe == null)
            {
                return NotFound();
            }

            var ingredients = recipe.Ingredients.Select(ri => new
            {
                itemId = ri.ItemId,
                quantity = ri.Quantity
            });

            return Json(ingredients);
        }


        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.ItemId == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRecipe(int itemId, int recipeId)
        {
            var item = await _context.Items
                .Include(i => i.Recipes)
                .FirstOrDefaultAsync(i => i.ItemId == itemId);

            if (item == null)
            {
                return NotFound();
            }

            var recipe = item.Recipes.FirstOrDefault(r => r.RecipeId == recipeId);
            if (recipe == null)
            {
                return NotFound();
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Edit), new { id = itemId });
        }





        // GET: Home/Delete/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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



        [AllowAnonymous]
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
                Quantity = 1,
                AvailableRecipes = item.Recipes.Select(r => new SelectListItem
                {
                    Value = r.RecipeId.ToString(),
                    Text = $"Recipe #{r.RecipeId}" // Customize this as needed
                }).ToList()
            };

            // Optionally pre-select the first recipe and calculate its steps
            if (item.Recipes.Any())
            {
                model.SelectedRecipeId = item.Recipes.First().RecipeId;
                var recipe = item.Recipes.First();
                await _itemServices.LoadNestedRecipesAsync(recipe);
                var (recipeSteps, baseItemsTotal) = _itemServices.CalculateRecipeSteps(recipe, model.Quantity);
                model.RecipeSteps = recipeSteps;
                model.BaseItemsTotal = baseItemsTotal;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id, double quantity, int? selectedRecipeId)
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
                Quantity = quantity,
                AvailableRecipes = item.Recipes.Select(r => new SelectListItem
                {
                    Value = r.RecipeId.ToString(),
                    Text = $"Recipe #{r.RecipeId}"
                }).ToList(),
                SelectedRecipeId = selectedRecipeId
            };

            if (selectedRecipeId.HasValue)
            {
                var selectedRecipe = item.Recipes.FirstOrDefault(r => r.RecipeId == selectedRecipeId.Value);
                if (selectedRecipe != null)
                {
                    await _itemServices.LoadNestedRecipesAsync(selectedRecipe);
                    var (recipeSteps, baseItemsTotal) = _itemServices.CalculateRecipeSteps(selectedRecipe, quantity);
                    model.RecipeSteps = recipeSteps;
                    model.BaseItemsTotal = baseItemsTotal;
                }
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