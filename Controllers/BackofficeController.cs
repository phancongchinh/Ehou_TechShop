using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechShop.Data;
using TechShop.Models.Entity;

namespace TechShop.Controllers
{
    [Route("/backoffice")]
    [Authorize(Roles = "Administrator")]
    public class BackofficeController : Controller
    {
        private readonly RepositoryContainer _container = new();

        [HttpGet]
        [Route("/users")]
        public IActionResult Users()
        {
            var purchases = _container.UserRepository.Get(includeProperties: "UserRole")
                .ToList();
            return View(purchases);
        }
        
        [HttpGet]
        [Route("/categories")]
        public IActionResult Categories()
        {
            var purchases = _container.CategoryRepository.Get()
                .ToList();
            return View(purchases);
        }

        [HttpGet]
        [Route("/products")]
        public IActionResult Products()
        {
            var purchases = _container.ProductRepository.Get()
                .ToList();
            return View(purchases);
        }

        [HttpGet]
        [Route("/purchases")]
        public IActionResult Purchases()
        {
            var purchases = _container.PurchaseRepository.Get(includeProperties: "User,PurchaseProducts.Product")
                .ToList();
            return View(purchases);
        }
        
        [HttpPost]
        [Route("/purchase")]
        public IActionResult UpdatePurchaseState(int purchaseId, int state)
        {
            var purchase = _container.PurchaseRepository.GetById(purchaseId);
            if (purchase == null) return NotFound();

            purchase.PurchaseState = (PurchaseState) state;
            _container.Save();
            return RedirectToAction("Purchases");
        }
    }
}