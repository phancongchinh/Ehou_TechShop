using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechShop.Data;
using TechShop.Models.Entity;

namespace TechShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private UnitOfWork _unit = new();

        [HttpGet]
        [Authorize]
        [Route("/cart")]
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToActionPermanent("Index", "Home");

            IEnumerable<ShoppingCartItem> shoppingCartItems = _unit.ShoppingCartItemRepository
                .Get(x => x.User.Email == User.Identity.Name, includeProperties: "User,Product")
                .ToList();

            return View(shoppingCartItems);
        }

        [HttpPost]
        [Authorize]
        [Route("/cart")]
        public IActionResult UpdateCart(int productId, int quantity, bool forceUpdateQuantity)
        {
            var product = _unit.ProductRepository.GetById(productId);

            if (product == null) return NotFound();

            var shoppingCartItem = _unit.ShoppingCartItemRepository
                .Get(x => x.User.Email == User.Identity.Name && x.ProductId == productId,
                    includeProperties: "User").FirstOrDefault();

            User user = _unit.UserRepository.Get(x => x.Email == User.Identity.Name).First();
            if (shoppingCartItem == null)
            {
                _unit.ShoppingCartItemRepository.Insert(new ShoppingCartItem()
                {
                    UserId = user.Id,
                    ProductId = productId,
                    Count = quantity
                });
            }
            else
            {
                shoppingCartItem.Count = forceUpdateQuantity ? quantity : shoppingCartItem.Count + quantity;
                _unit.ShoppingCartItemRepository.Update(shoppingCartItem);
            }

            _unit.Save();

            return RedirectToAction("Index", "ShoppingCart");
        }
    }
}