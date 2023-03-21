using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechShop.Data;
using TechShop.Models.Entity;

namespace TechShop.Controllers
{
    public class UserController : Controller
    {
        private UnitOfWork _unit = new();

        [HttpGet]
        [Authorize]
        public IActionResult Info()
        {
            User user = _unit.UserRepository.Get(x => x.Email == User.Identity.Name, includeProperties: "UserRole")
                .FirstOrDefault();

            if (user == null) return NotFound();

            return View(user);
        }
    }
}