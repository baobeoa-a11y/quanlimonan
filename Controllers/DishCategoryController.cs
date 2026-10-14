
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyMonAn.Data;
using QuanLyMonAn.Models;

namespace QuanLyMonAn.Controllers
{
    public class DishCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DishCategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Danh sách loại món ăn
        public async Task<IActionResult> Index()
        {
            var categories = await _context.DishCategories
                .Include(c => c.Dishes)
                .OrderBy(c => c.Name)
                .ToListAsync();

            return View(categories);
        }

        // Xem chi tiết loại món ăn
        public async Task<IActionResult> Detail(int id)
        {
            var category = await _context.DishCategories
                .Include(c => c.Dishes)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound("Category not found.");
            }

            return View(category);
        }

        // Xác nhận trước khi xóa loại món ăn
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.DishCategories
                .Include(c => c.Dishes)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound("Category not found.");
            }

            if (category.Dishes.Any())
            {
                TempData["Error"] =
                    "Cannot delete this category because it is being used by dishes.";

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // Thực hiện xóa loại món ăn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.DishCategories
                .Include(c => c.Dishes)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound("Category not found.");
            }

            if (category.Dishes.Any())
            {
                TempData["Error"] =
                    "Cannot delete this category because it is being used by dishes.";

                return RedirectToAction(nameof(Index));
            }

            _context.DishCategories.Remove(category);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Category deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
