
using QuanLyMonAn.Models;

namespace QuanLyMonAn.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Nếu đã có danh mục thì không tạo lại dữ liệu mẫu.
            if (context.DishCategories.Any())
                return;

            var categories = new List<DishCategory_BIT240154>
            {
                new DishCategory_BIT240154
                {
                    Name = "Main Dishes",
                    Description = "Main meals"
                },
                new DishCategory_BIT240154
                {
                    Name = "Drinks",
                    Description = "Beverages"
                },
                new DishCategory_BIT240154
                {
                    Name = "Desserts",
                    Description = "Sweet dishes"
                }
            };

            context.DishCategories.AddRange(categories);
            context.SaveChanges();

            var mainDishes = categories[0];
            var drinks = categories[1];
            var desserts = categories[2];

            var dishes = new List<Dish_BIT240154>
            {
                new Dish_BIT240154
                {
                    Name = "Fried Rice",
                    Price = 45000,
                    PreparationTime = 12,
                    IsAvailable = true,
                    Description = "Fried rice with egg",
                    DishCategoryId = mainDishes.Id
                },
                new Dish_BIT240154
                {
                    Name = "Beef Noodles",
                    Price = 55000,
                    PreparationTime = 20,
                    IsAvailable = true,
                    Description = "Noodles with beef",
                    DishCategoryId = mainDishes.Id
                },
                new Dish_BIT240154
                {
                    Name = "Chicken Rice",
                    Price = 50000,
                    PreparationTime = 18,
                    IsAvailable = true,
                    Description = "Rice with chicken",
                    DishCategoryId = mainDishes.Id
                },
                new Dish_BIT240154
                {
                    Name = "Orange Juice",
                    Price = 25000,
                    PreparationTime = 5,
                    IsAvailable = true,
                    Description = "Fresh orange juice",
                    DishCategoryId = drinks.Id
                },
                new Dish_BIT240154
                {
                    Name = "Flan",
                    Price = 15000,
                    PreparationTime = 10,
                    IsAvailable = false,
                    Description = "Caramel custard",
                    DishCategoryId = desserts.Id
                }
            };

            context.Dishes.AddRange(dishes);
            context.SaveChanges();
        }
    }
}
