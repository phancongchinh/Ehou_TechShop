using System;
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
        private readonly UnitOfWork _unit = new();

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            IEnumerable<Purchase> purchases = _unit.PurchaseRepository
                .Get(x => x.User.Email == User.Identity.Name,
                    includeProperties: "User,PurchaseProducts.Product")
                .OrderByDescending(x => x.CreationTime);

            return View(purchases);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Info(int purchaseId)
        {
            var purchase = _unit.PurchaseRepository
                .Get(x => x.User.Email == User.Identity.Name && x.Id == purchaseId,
                    includeProperties: "User,PurchaseProducts.Product").FirstOrDefault();

            if (purchase == null) return NotFound();

            return View(purchase);
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreatePurchase()
        {
            User user = _unit.UserRepository.Get(x => x.Email == User.Identity.Name).First();

            IEnumerable<ShoppingCartItem> shoppingCartItems = _unit.ShoppingCartItemRepository
                .Get(x => x.User.Email == User.Identity.Name, includeProperties: "Product").ToList();

            Purchase newPurchase = new()
            {
                UserId = user.Id,
                State = "PENDING",
                CreationTime = DateTime.Now,
            };

            _unit.PurchaseRepository.Insert(newPurchase);
            _unit.Save();

            foreach (var item in shoppingCartItems)
            {
                _unit.PurchaseProductRepository.Insert(new PurchaseProduct()
                {
                    PurchaseId = newPurchase.Id,
                    ProductId = item.ProductId,
                    Price = item.Product.Price,
                    Count = item.Count
                });
                _unit.ShoppingCartItemRepository.Delete(item);

                Product product = _unit.ProductRepository.GetById(item.ProductId)!;
                product.Quantity -= item.Count;
                _unit.ProductRepository.Update(product);
            }

            _unit.Save();

            return RedirectToAction("Info", "Purchase", new {purchaseId = newPurchase.Id});
        }
    }
}