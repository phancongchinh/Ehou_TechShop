using System.ComponentModel.DataAnnotations;

namespace TechShop.Models.Entity
{
    public class ShoppingCartItem
    {
        [Key]
        public int UserId { get; set; }
        [Key]
        public int ProductId { get; set; }

        public User User { get; set; }
        public Product Product { get; set; }
        public int Count { get; set; }
    }
}