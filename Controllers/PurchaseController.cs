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
        private UnitOfWork _unit = new();

        [HttpGet]
        [Authorize]
        [Route("/purchase")]
        public IActionResult Index()
        {
            User user = _unit.UserRepository.Get(x => x.Email == User.Identity.Name).FirstOrDefault();

            if (user == null) return NotFound();

            IEnumerable<Purchase> purchases = _unit.PurchaseRepository
                .Get(x => x.UserId == user.Id, includeProperties: "PurchaseProducts.Product")
                .OrderByDescending(x => x.CreationTime);

            return View(purchases);
        }

        [HttpGet]
        [Authorize]
        [Route("/purchase/{id}")]
        public IActionResult Info(int purchaseId)
        {
            var purchase = _unit.PurchaseRepository.Get(x => x.User.Email == User.Identity.Name,
                includeProperties: "User,PurchaseProducts.Product").FirstOrDefault();

            if (purchase == null) return NotFound();

            return View(purchase);
        }

        [HttpPost]
        [Authorize]
        [Route("/purchase")]
        public IActionResult CreatePurchase(Purchase purchase)
        {
            _unit.PurchaseRepository.Insert(purchase);
            _unit.Save();
            return RedirectToAction("Index");
        }
    }
}