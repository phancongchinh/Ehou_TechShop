using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechShop.Data;
using TechShop.Models.Entity;

namespace TechShop.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class BackofficeController : Controller
    {
        private readonly UnitOfWork _unit = new();

        [HttpGet]
        [Route("/backoffice/users")]
        public IActionResult Users()
        {
            var purchases = _unit.UserRepository.Get(includeProperties: "UserRole")
                .ToList();
            return View(purchases);
        }
        
        [HttpGet]
        [Route("/backoffice/categories")]
        public IActionResult Categories()
        {
            var purchases = _unit.CategoryRepository.Get()
                .ToList();
            return View(purchases);
        }

        [HttpGet]
        [Route("/backoffice/products")]
        public IActionResult Products()
        {
            var purchases = _unit.ProductRepository.Get()
                .ToList();
            return View(purchases);
        }

        [HttpGet]
        [Route("/backoffice/purchases")]
        public IActionResult Purchases()
        {
            var purchases = _unit.PurchaseRepository.Get(includeProperties: "User,PurchaseProducts.Product")
                .ToList();
            return View(purchases);
        }
        
        [HttpPost]
        public IActionResult UpdatePurchaseState(int purchaseId, string state)
        {
            var purchase = _unit.PurchaseRepository.GetById(purchaseId);
            if (purchase == null) return NotFound();

            purchase.State = state;
            _unit.Save();
            return RedirectToAction("Purchases");
        }
    }
}