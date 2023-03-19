using System.ComponentModel.DataAnnotations;

namespace TechShop.Models.Entity
{
    public class Image
    {
        public int Id { get; set; }
        public string Path { get; set; }
        
        public Product Product { get; set; }
    }
}