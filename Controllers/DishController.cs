
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyMonAn.Data;
using QuanLyMonAn.Models;

namespace QuanLyMonAn.Controllers
{
    public class DishController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DishController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =====================================
        // 1. DANH SACH MON AN
        // =====================================
        [HttpGet]
        public async Task<IActionResult> Index(
            string? searchName,
            int? categoryId,
            bool? isAvailable,
            decimal? minPrice,
            decimal? maxPrice,
            string? sortBy)
        {
            ViewBag.Categories = new SelectList(
                await _context.DishCategories
                    .OrderBy(c => c.Name)
                    .ToListAsync(),
                "Id", "Name", categoryId);

            ViewBag.SearchName = searchName;
            ViewBag.CategoryId = categoryId;
            ViewBag.IsAvailable = isAvailable;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.SortBy = sortBy;

            if ((minPrice.HasValue && minPrice.Value < 0) ||
                (maxPrice.HasValue && maxPrice.Value < 0) ||
                (minPrice.HasValue && maxPrice.HasValue &&
                 minPrice.Value > maxPrice.Value))
            {
                ViewBag.Error = "Invalid price range.";
                return View(new List<Dish_BIT240154>());
            }

            var query = _context.Dishes
                .Include(d => d.DishCategory)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchName))
            {
                string keyword = searchName.Trim();
                query = query.Where(d => d.Name.Contains(keyword));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(
                    d => d.DishCategoryId == categoryId.Value);
            }

            if (isAvailable.HasValue)
            {
                query = query.Where(
                    d => d.IsAvailable == isAvailable.Value);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(
                    d => d.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(
                    d => d.Price <= maxPrice.Value);
            }

            query = sortBy switch
            {
                "price_asc" => query.OrderBy(d => d.Price),
                "price_desc" => query.OrderByDescending(d => d.Price),
                "prep_asc" => query.OrderBy(d => d.PreparationTime),
                _ => query.OrderBy(d => d.Id)
            };

            var dishes = await query.ToListAsync();

            return View(dishes);
        }

        // =====================================
        // 2. HIEN THI FORM THEM MON
        // =====================================
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadCategoriesAsync();

            return View(new Dish_BIT240154
            {
                IsAvailable = true
            });
        }

        // =====================================
        // 3. XU LY THEM MON
        // =====================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Dish_BIT240154 dish)
        {
            string name = dish.Name?.Trim() ?? string.Empty;
            dish.Name = name;

            bool categoryExists = await _context.DishCategories
                .AnyAsync(c => c.Id == dish.DishCategoryId);

            if (!categoryExists)
            {
                ModelState.AddModelError(
                    nameof(dish.DishCategoryId),
                    "Please select a valid category.");
            }

            if (name.Length > 0)
            {
                bool duplicateName = await _context.Dishes
                    .AnyAsync(d =>
                        d.DishCategoryId == dish.DishCategoryId &&
                        d.Name == name);

                if (duplicateName)
                {
                    ModelState.AddModelError(
                        nameof(dish.Name),
                        "This dish name already exists in this category.");
                }
            }

            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync(dish.DishCategoryId);
                return View(dish);
            }

            _context.Dishes.Add(dish);

            try
            {
                await _context.SaveChangesAsync();
                TempData["Success"] = "Dish added successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Could not save the dish.");

                await LoadCategoriesAsync(dish.DishCategoryId);
                return View(dish);
            }
        }

        // =====================================
        // 4. HIEN THI FORM SUA MON
        // =====================================
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id <= 0)
                return NotFound("Invalid dish ID.");

            var dish = await _context.Dishes
                .FirstOrDefaultAsync(d => d.Id == id.Value);

            if (dish == null)
                return NotFound("Dish not found.");

            await LoadCategoriesAsync(dish.DishCategoryId);

            return View(dish);
        }

        // =====================================
        // 5. XU LY SUA MON
        // =====================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Dish_BIT240154 dish)
        {
            if (id <= 0 || id != dish.Id)
                return BadRequest("Invalid dish ID.");

            var existingDish = await _context.Dishes
                .FirstOrDefaultAsync(d => d.Id == id);

            if (existingDish == null)
                return NotFound("Dish not found.");

            string name = dish.Name?.Trim() ?? string.Empty;
            dish.Name = name;

            bool categoryExists = await _context.DishCategories
                .AnyAsync(c => c.Id == dish.DishCategoryId);

            if (!categoryExists)
            {
                ModelState.AddModelError(
                    nameof(dish.DishCategoryId),
                    "Please select a valid category.");
            }

            if (name.Length > 0)
            {
                bool duplicateName = await _context.Dishes
                    .AnyAsync(d =>
                        d.Id != id &&
                        d.DishCategoryId == dish.DishCategoryId &&
                        d.Name == name);

                if (duplicateName)
                {
                    ModelState.AddModelError(
                        nameof(dish.Name),
                        "This dish name already exists in this category.");
                }
            }

            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync(dish.DishCategoryId);
                return View(dish);
            }

            existingDish.Name = dish.Name;
            existingDish.Price = dish.Price;
            existingDish.PreparationTime = dish.PreparationTime;
            existingDish.IsAvailable = dish.IsAvailable;
            existingDish.Description = dish.Description;
            existingDish.DishCategoryId = dish.DishCategoryId;

            try
            {
                await _context.SaveChangesAsync();
                TempData["Success"] = "Dish updated successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Could not update the dish.");

                await LoadCategoriesAsync(dish.DishCategoryId);
                return View(dish);
            }
        }

        // =====================================
        // 6. HIEN THI THONG TIN XAC NHAN XOA
        // =====================================
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id <= 0)
                return NotFound("Invalid dish ID.");

            var dish = await _context.Dishes
                .Include(d => d.DishCategory)
                .FirstOrDefaultAsync(d => d.Id == id.Value);

            if (dish == null)
                return NotFound("Dish not found.");

            return View(dish);
        }

        // =====================================
        // 7. XAC NHAN VA THUC HIEN XOA
        // =====================================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id <= 0)
                return NotFound("Invalid dish ID.");

            var dish = await _context.Dishes
                .FirstOrDefaultAsync(d => d.Id == id);

            if (dish == null)
                return NotFound("Dish not found.");

            try
            {
                _context.Dishes.Remove(dish);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Dish deleted successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return BadRequest(
                    "Could not delete this dish because related data may exist.");
            }
        }

        // =====================================
        // 8. NAP DANH MUC CHO DROPDOWN
        // =====================================
        private async Task LoadCategoriesAsync(
            int? selectedId = null)
        {
            ViewBag.Categories = new SelectList(
                await _context.DishCategories
                    .OrderBy(c => c.Name)
                    .ToListAsync(),
                "Id", "Name", selectedId);
      
              }

        public async Task<IActionResult> Detail(int id)
        {
            var dish = await _context.Dishes
                .Include(d => d.DishCategory)
                .Include(d => d.Images)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (dish == null)
            {
                return NotFound("Dish not found.");
            }

            dish.Images = dish.Images
                .OrderByDescending(i => i.IsThumbnail)
                .ThenBy(i => i.Id)
                .ToList();

            return View(dish);
        }

    }
}
