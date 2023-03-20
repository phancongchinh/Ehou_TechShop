using TechShop.Models.Entity;

namespace TechShop.Models.Dto
{
    public class PurchaseVM
    {
        public Purchase Purchase { get; set; }
        public decimal TotalPrice { get; set; }
    }
}