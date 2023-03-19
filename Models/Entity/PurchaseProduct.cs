using System.ComponentModel.DataAnnotations;

namespace TechShop.Models.Entity
{
    public class PurchaseProduct
    {
        [Key]
        public int PurchaseId { get; set; }
        [Key]
        public int ProductId { get; set; }
        public int Count { get; set; }
        
        public Purchase Purchase { get; set; }
        public Product Product { get; set; }
        
    }
}