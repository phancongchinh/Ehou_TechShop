namespace TechShop.Models.Dto
{
    public class UserInfoVM
    {
        public string Name { get; set; }

        public string Phone { get; set; }
        
        public string Email { get; set; }
        
        public bool IsModerator { get; set; }
        
        public bool IsAdmin { get; set; }
    }
}