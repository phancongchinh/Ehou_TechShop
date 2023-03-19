using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechShop.Models.Entity
{
    public class Purchase
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreationTime { get; set; }
        
        public User User { get; set; }
        public ICollection<PurchaseProduct> PurchaseProducts { get; set; }
    }
}