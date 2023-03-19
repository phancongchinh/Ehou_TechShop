﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechShop.Models.Entity
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        public string Path { get; set; }
        
        public Product Product { get; set; }
    }
}