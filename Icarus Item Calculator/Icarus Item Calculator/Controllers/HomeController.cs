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
            Item item = new Item();

            if (item == null)
            {
                return NotFound();
            }

            ViewBag.Items = new SelectList(_context.Items, "ItemId", "Name");
            return View(item);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Item item,
                                                        int[] selectedItems, Dictionary<int, double> quantities)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Items = new SelectList(_context.Items, "ItemId", "Name");
                return View(item);
            }

            if (selectedItems == null || !selectedItems.Any())
            {
                item.Recipe = new List<RecipeItem>();
            }
            else if (selectedItems.Contains(0))
            {
                item.Recipe = new List<RecipeItem>();
            }
            else
            {
                item.Recipe = new List<RecipeItem>();
                foreach (var itemId in selectedItems)
                {
                    var recipeItem = await _context.Items.FindAsync(itemId);
                    if (recipeItem != null && quantities.TryGetValue(itemId, out double quantity) && quantity > 0)
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

            try
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while saving the item.");
                ViewBag.Items = new SelectList(_context.Items, "ItemId", "Name");
                return View(item);
            }
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
        public async Task<IActionResult> Edit(int id, [Bind("ItemId,Name,IsCraftable,IsCraftingInterface")] Item item,
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
                .Include(i => i.Recipe)
                .FirstOrDefaultAsync(i => i.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            try
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                // Check if the item is used as an ingredient
                if (_context.RecipeItems.Any(ri => ri.ItemId == id))
                {
                    ModelState.AddModelError("", "Cannot delete this item because it is used in other recipes.");
                    return View(item);
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

            await _itemServices.LoadNestedRecipesAsync(item);
            var (recipeSteps, baseItemsTotal) = _itemServices.CalculateRecipeSteps(item, quantity);

            //if (quantity <= 0)
            //{
            //    ModelState.AddModelError("quantity", "Quantity must be greater than zero.");
            //    return View(model);
            //}

            var model = new ItemWithStepsViewModel
            {
                Item = item,
                RecipeSteps = recipeSteps,
                Quantity = quantity,
                BaseItemsTotal = baseItemsTotal 
            };

            return View(model);
        }
    }
}