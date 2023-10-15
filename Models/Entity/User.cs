using System.ComponentModel.DataAnnotations.Schema;

namespace TechShop.Models.Entity
{
    [Table("user")]
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public int RoleId { get; set; }

        public bool IsDisabled { get; set; }

        public UserRole UserRole { get; set; }
        
        public IEnumerable<ShoppingCartItem> ShoppingCartItems { get; set; }
        
        public IEnumerable<Purchase> Purchases { get; set; }
    }
}