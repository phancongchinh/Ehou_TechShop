using System.ComponentModel.DataAnnotations;

namespace TechShop.Models.Dto
{
    public class UserRegisterVM
    {
        [Required] 
        public string Name { get; set; }

        [Required] 
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        public string ConfirmPassword { get; set; }
    }
}