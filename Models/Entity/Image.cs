using System.ComponentModel.DataAnnotations.Schema;

namespace TechShop.Models.Entity
{
    [Table("image")]
    public class Image
    {
        public int Id { get; set; }

        public string Path { get; set; }

        public Product Product { get; set; }
    }
}