using System.ComponentModel.DataAnnotations.Schema;

namespace TechShop.Models.Entity
{
    [Table("purchase_product")]
    public class PurchaseProduct
    {
        public int PurchaseId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        
        public Purchase Purchase { get; set; }
        public Product Product { get; set; }
        
    }
}