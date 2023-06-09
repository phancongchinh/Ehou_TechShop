﻿using System;
using System.Collections.Generic;

namespace TechShop.Models.Entity
{
    public class Purchase
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string State { get; set; }
        public DateTime CreationTime { get; set; }

        public User User { get; set; }
        public IEnumerable<PurchaseProduct> PurchaseProducts { get; set; }
    }
}