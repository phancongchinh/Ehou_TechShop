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
        private readonly UnitOfWork _unit = new();

        [HttpGet]
        [Route("/users")]
        public IActionResult Users()
        {
            var purchases = _unit.UserRepository.Get(includeProperties: "UserRole")
                .ToList();
            return View(purchases);
        }
        
        [HttpGet]
        [Route("/categories")]
        public IActionResult Categories()
        {
            var purchases = _unit.CategoryRepository.Get()
                .ToList();
            return View(purchases);
        }

        [HttpGet]
        [Route("/products")]
        public IActionResult Products()
        {
            var purchases = _unit.ProductRepository.Get()
                .ToList();
            return View(purchases);
        }

        [HttpGet]
        [Route("/purchases")]
        public IActionResult Purchases()
        {
            var purchases = _unit.PurchaseRepository.Get(includeProperties: "User,PurchaseProducts.Product")
                .ToList();
            return View(purchases);
        }
        
        [HttpPost]
        [Route("/purchase")]
        public IActionResult UpdatePurchaseState(int purchaseId, int state)
        {
            var purchase = _unit.PurchaseRepository.GetById(purchaseId);
            if (purchase == null) return NotFound();

            purchase.PurchaseState = (PurchaseState) state;
            _unit.Save();
            return RedirectToAction("Purchases");
        }
    }
}