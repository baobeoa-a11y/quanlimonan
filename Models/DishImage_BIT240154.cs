
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyMonAn.Models
{
    [Table("BIT240154_DishImages")]
    public class DishImage_BIT240154
    {
        [Key]
        [Column("BIT240154_Id")]
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        [Column("BIT240154_ImageUrl")]
        public string ImageUrl { get; set; } = string.Empty;

        [Column("BIT240154_IsThumbnail")]
        public bool IsThumbnail { get; set; }

        [Column("BIT240154_DishId")]
        public int DishId { get; set; }

        public Dish_BIT240154? Dish { get; set; }
    }
}
