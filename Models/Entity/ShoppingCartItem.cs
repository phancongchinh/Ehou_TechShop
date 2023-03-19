using System.ComponentModel.DataAnnotations.Schema;

namespace TechShop.Models.Entity
{
    [Table("shopping_cart_item")]
    public class ShoppingCartItem
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        
        public int Count { get; set; }

        public User User { get; set; }
        public Product Product { get; set; }
        
    }
}