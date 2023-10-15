using System.ComponentModel.DataAnnotations.Schema;

namespace TechShop.Models.Entity
{
    [Table("product")]
    public class Product
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string Producer { get; set; }

        public string Model { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public int ImageId { get; set; }

        public bool IsDeleted { get; set; }


        public Image Image { get; set; }
        public Category Category { get; set; }
        public IEnumerable<ShoppingCartItem> ShoppingCartItems { get; set; }
        public IEnumerable<PurchaseProduct> PurchaseProducts { get; set; }

        public string Name => Producer + " " + Model;
    }
}