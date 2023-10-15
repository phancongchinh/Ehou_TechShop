using Microsoft.AspNetCore.Mvc.Rendering;

namespace TechShop.Models.ViewModels
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string Producer { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public SelectList Categories { get; set; }
        public string ImageName { get; set; }
        public int Quantity { get; set; }
        public IFormFile Image{ get; set; }
    }
}
