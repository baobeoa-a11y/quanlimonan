
using Microsoft.EntityFrameworkCore;
using QuanLyMonAn.Models;

namespace QuanLyMonAn.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<DishCategory_BIT240154> DishCategories
            => Set<DishCategory_BIT240154>();

        public DbSet<Dish_BIT240154> Dishes
            => Set<Dish_BIT240154>();

        public DbSet<DishImage_BIT240154> DishImages
            => Set<DishImage_BIT240154>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Khong trung ten mon trong cung mot loai.
            modelBuilder.Entity<Dish_BIT240154>()
                .HasIndex(d => new { d.DishCategoryId, d.Name })
                .IsUnique();

            // Moi mon toi da mot anh dai dien.
            modelBuilder.Entity<DishImage_BIT240154>()
                .HasIndex(i => i.DishId)
                .IsUnique()
                .HasFilter("[BIT240154_IsThumbnail] = 1");

            // Khong cho xoa loai mon khi van con mon an.
            modelBuilder.Entity<Dish_BIT240154>()
                .HasOne(d => d.DishCategory)
                .WithMany(c => c.Dishes)
                .HasForeignKey(d => d.DishCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Moi mon co the co nhieu anh.
            modelBuilder.Entity<DishImage_BIT240154>()
                .HasOne(i => i.Dish)
                .WithMany(d => d.Images)
                .HasForeignKey(i => i.DishId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
