using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechShop.Data;
using TechShop.Models.Entity;

namespace TechShop.Controllers
{
    public class PurchaseController : Controller
    {
        private RepositoryContainer _container = new();

        [HttpGet]
        [Authorize]
        [Route("/purchase")]
        public IActionResult Index()
        {
            User user = _container.UserRepository.Get(x => x.Email == User.Identity.Name).FirstOrDefault();

            if (user == null) return NotFound();

            IEnumerable<Purchase> purchases = _container.PurchaseRepository
                .Get(x => x.UserId == user.Id, includeProperties: "PurchaseProducts.Product")
                .OrderByDescending(x => x.CreationTime);

            return View(purchases);
        }
    }
}