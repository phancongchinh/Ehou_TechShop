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
        private RepositoryContainer _container = new();

        [HttpGet]
        [Authorize]
        [Route("/cart")]
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToActionPermanent("Index", "Home");

            IEnumerable<ShoppingCartItem> shoppingCartItems = _container.ShoppingCartItemRepository
                .Get(x => x.User.Email == User.Identity.Name, includeProperties: "User,Product")
                .ToList();
            var total = shoppingCartItems.Sum(x => x.Product.Price * x.Count);

            ViewData.Add("TotalPrice", total);

            return View(shoppingCartItems);
        }

        [HttpPost]
        [Authorize]
        [Route("/cart")]
        public IActionResult UpdateCart(int productId, int count)
        {
            var product = _container.ProductRepository.GetById(productId);

            if (product == null) return NotFound();

            var shoppingCartItem = _container.ShoppingCartItemRepository
                .Get(x => x.User.Email == User.Identity.Name && x.ProductId == productId,
                    includeProperties: "User").FirstOrDefault();

            if (shoppingCartItem == null) return RedirectToAction("Index", "ShoppingCart");

            shoppingCartItem.Count = count;
            _container.ShoppingCartItemRepository.Update(shoppingCartItem);

            return RedirectToAction("Index", "ShoppingCart");
        }
    }
}