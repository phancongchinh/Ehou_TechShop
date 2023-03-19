using System.Collections.Generic;
using TechShop.Models.Entity;

namespace TechShop.Models.Dto
{
    public class UserPurchaseVM
    {
        public ICollection<Purchase> Purchases { get; set; }
    }
}