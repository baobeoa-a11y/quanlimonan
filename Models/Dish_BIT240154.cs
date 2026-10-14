
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyMonAn.Models
{
    [Table("BIT240154_Dishes")]
    public class Dish_BIT240154
    {
        [Key]
        [Column("BIT240154_Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Dish name is required.")]
        [StringLength(150)]
        [Column("BIT240154_Name")]
        public string Name { get; set; } = string.Empty;

        [Range(typeof(decimal), "0.01", "999999999",
            ErrorMessage = "Price must be greater than 0.")]
        [Column("BIT240154_Price", TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Range(1, 1440,
            ErrorMessage = "Preparation time must be greater than 0.")]
        [Column("BIT240154_PreparationTime")]
        public int PreparationTime { get; set; }

        [Column("BIT240154_IsAvailable")]
        public bool IsAvailable { get; set; } = true;

        [Column("BIT240154_Description")]
        public string? Description { get; set; }

        [Column("BIT240154_DishCategoryId")]
        public int DishCategoryId { get; set; }

        public DishCategory_BIT240154? DishCategory { get; set; }

        public ICollection<DishImage_BIT240154> Images { get; set; }
            = new List<DishImage_BIT240154>();

        // Tinh phan loai khi hien thi, khong tao cot SQL.
        [NotMapped]
        public string PreparationType =>
            PreparationTime <= 15 ? "Quick / Nhanh" : "Regular / Thong thuong";
    }
}
