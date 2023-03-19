using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TechShop.Data;
using TechShop.Models.Dto;

namespace TechShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly RepositoryContainer _container = new();

        // GET
        [Route("Product/{id}")]
        public IActionResult Index(int? id)
        {
            if (!User.Identity.IsAuthenticated || id == null) return RedirectToAction("Index", "Home");

            var product = _container.ProductRepository.Get(x => x.Id == id, includeProperties: "Image,Category")
                .FirstOrDefault();

            if (product == null) return RedirectToAction("Index", "Home");

            var inpurchase = false;
            var incart = false;
            var canEditProduct = false;


            var purchases = _container.PurchaseProductRepository
                .Get(x => x.ProductId == product.Id, includeProperties: "Purchase,Product");
            var user = _container.UserRepository
                .Get(x => x.Email == User.Identity.Name, includeProperties: "Role")
                .FirstOrDefault();
            inpurchase = purchases.Any(x => x.Purchase.UserId == user.Id);
            incart = _container.ShoppingCartItemRepository
                .Get(x => x.ProductId == product.Id && x.UserId == user.Id && x.Count != 0)
                .Count != 0;

            canEditProduct = user.UserRole.Id == 1;
            _container.Save();

            ProductVM model = new()
            {
                Product = product,
                InPurchase = inpurchase,
                InShopingCart = incart,
                CanEditProduct = canEditProduct
            };

            return View();
        }
    }
}