using System.ComponentModel.DataAnnotations.Schema;

namespace TechShop.Models.Entity
{
    [Table("user_role")]
    public class UserRole
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<User> Users { get; set; }
    }
}