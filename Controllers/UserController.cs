using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechShop.Data;
using TechShop.Models;
using TechShop.Models.Dto;

namespace TechShop.Controllers
{
    public class UserController : Controller
    {
        private RepositoryContainer _container = new();

        [HttpGet]
        [Authorize]
        [Route("account/info")]
        public IActionResult Info()
        {
            // if (!User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");

            var user = _container.UserRepository.Get(x => x.Email == User.Identity.Name, includeProperties: "Role")
                .FirstOrDefault();

            if (user == null) return View("~/Views/Error/Error404.cshtml", ErrorVM.Default());

            UserInfoVM model = new()
            {
                Name = user.Name,
                Phone = user.Phone,
                Email = user.Email,
                IsAdmin = user.UserRole.Id == 1,
            };

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult UserPurchases()
        {
            var email = User.Identity.Name;
            var user = _container.UserRepository.Get(x => x.Email == email).FirstOrDefault();

            if (user == null) return View();

            var purchases = _container.PurchaseRepository.Get(x => x.UserId == user.Id,
                includeProperties: "PurchaseProducts.Product");
            // .OrderByDescending(x => x.CreationTime);

            var model = new UserPurchaseVM()
            {
                Purchases = purchases
            };
            return View(model);
        }
    }
}