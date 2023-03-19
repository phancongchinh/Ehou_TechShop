using TechShop.Models.Entity;

namespace TechShop.Models.Dto
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public bool InShopingCart { get; set; }
        public bool InPurchase { get; set; }
        public bool CanEditProduct { get; set; }
    }
}