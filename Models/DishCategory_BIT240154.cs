
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyMonAn.Models
{
    [Table("BIT240154_DishCategories")]
    public class DishCategory_BIT240154
    {
        [Key]
        [Column("BIT240154_Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100)]
        [Column("BIT240154_Name")]
        public string Name { get; set; } = string.Empty;

        [Column("BIT240154_Description")]
        public string? Description { get; set; }

        public ICollection<Dish_BIT240154> Dishes { get; set; }
            = new List<Dish_BIT240154>();
    }
}
