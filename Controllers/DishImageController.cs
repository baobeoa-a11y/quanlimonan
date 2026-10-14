
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyMonAn.Data;
using QuanLyMonAn.Models;

namespace QuanLyMonAn.Controllers
{
    public class DishImageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DishImageController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Xem danh sach anh cua mot mon an
        public async Task<IActionResult> Index(int dishId)
        {
            var dish = await _context.Dishes
                .Include(d => d.Images)
                .FirstOrDefaultAsync(d => d.Id == dishId);

            if (dish == null)
            {
                return NotFound("Dish not found.");
            }

            ViewBag.DishId = dish.Id;
            ViewBag.DishName = dish.Name;

            return View(dish.Images.OrderByDescending(i => i.IsThumbnail).ToList());
        }

        // Them anh - GET
        public async Task<IActionResult> Create(int dishId)
        {
            var dishExists = await _context.Dishes
                .AnyAsync(d => d.Id == dishId);

            if (!dishExists)
            {
                return NotFound("Dish not found.");
            }

            ViewBag.DishId = dishId;
            return View();
        }

        // Them anh - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            int dishId,
            string imageUrl,
            bool isThumbnail)
        {
            var dishExists = await _context.Dishes
                .AnyAsync(d => d.Id == dishId);

            if (!dishExists)
            {
                return NotFound("Dish not found.");
            }

            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                ModelState.AddModelError("imageUrl", "Image URL is required.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.DishId = dishId;
                return View();
            }

            if (isThumbnail)
            {
                var oldThumbnails = await _context.DishImages
                    .Where(i => i.DishId == dishId && i.IsThumbnail)
                    .ToListAsync();

                foreach (var image in oldThumbnails)
                {
                    image.IsThumbnail = false;
                }
            }

            var newImage = new DishImage_BIT240154
            {
                DishId = dishId,
                ImageUrl = imageUrl.Trim(),
                IsThumbnail = isThumbnail
            };

            _context.DishImages.Add(newImage);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { dishId });
        }

        // Chon mot anh lam anh dai dien
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetThumbnail(int id)
        {
            var selectedImage = await _context.DishImages
                .FirstOrDefaultAsync(i => i.Id == id);

            if (selectedImage == null)
            {
                return NotFound("Image not found.");
            }

            var images = await _context.DishImages
                .Where(i => i.DishId == selectedImage.DishId)
                .ToListAsync();

            foreach (var image in images)
            {
                image.IsThumbnail = image.Id == selectedImage.Id;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index),
                new { dishId = selectedImage.DishId });
        }

        // Xoa anh
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var image = await _context.DishImages
                .FirstOrDefaultAsync(i => i.Id == id);

            if (image == null)
            {
                return NotFound("Image not found.");
            }

            int dishId = image.DishId;

            _context.DishImages.Remove(image);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { dishId });
        }
    }
}
