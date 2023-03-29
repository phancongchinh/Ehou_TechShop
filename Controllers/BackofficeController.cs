using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechShop.Data;
using TechShop.Models.Dto;
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
        [Route("/admin/products")]
        public IActionResult AdminProducts()
        {
            User user = _unit.UserRepository.Get(x => x.Email == User.Identity.Name, includeProperties: "UserRole")
                .FirstOrDefault();
            if (user == null) return NotFound();

            if (user.UserRole.Name != "Administrator")
                return RedirectToAction("Index", "Home");

            var products = _unit.ProductRepository.Get(includeProperties: "Image,Category");
            ViewData["Products"] = products;
            ViewData["Categories"] = _unit.CategoryRepository.Get();
            return View("Products", products);
        }

        [HttpGet]
        [Route("/admin/products/edit/{id}")]
        public IActionResult EditProduct(int? id)
        {
            Product product = null;
            if (id != null)
            {
                product = _unit
                    .ProductRepository
                    .Get(x => x.Id == id, includeProperties: "Category,Image")
                    .FirstOrDefault();
            }
            ProductEditModel model = new ProductEditModel();
           if (product != null)
           {
                model.Model = product.Model;
                model.Producer = product.Producer;
                model.Price = product.Price;
                model.Id = product.Id;
                model.Description = product.Description;
                model.ImageName = product.Image?.Path;
                model.Quantity = product.Quantity;
                var categories = _unit.CategoryRepository.Get().ToList();
                var sel = categories.Where(x => x.Id == product.CategoryId).FirstOrDefault();
                var sellist = new SelectList(categories, "Id", "Name", sel);
                model.Categories = sellist;
           }
           else
            {
                model.Categories = new SelectList(_unit.CategoryRepository.Get().ToList(), "Id", "Name");
            }

            return View("ProductsEdit", model);
        }

        [HttpGet]
        [Route("/admin/products/remove/{id}")]
        public IActionResult RemoveProduct(int? id, [FromQuery(Name = "redirect")] string redirect)
        {
            var user = _unit
                .UserRepository.Get(x => x.Email == User.Identity.Name, includeProperties: "UserRole")
                .FirstOrDefault();
            if (id != null && User.Identity.IsAuthenticated && user.UserRole.Name == "Administrator")
            {
                var shopingCart = _unit.ShoppingCartItemRepository.Get(x => x.ProductId == id).FirstOrDefault();
                if (shopingCart == null)
                {
                    _unit.ProductRepository.Delete((int)id);
                    _unit.Save();
                }
              
            }
            return Redirect(redirect);

        }


        public async Task<IActionResult> SaveAsync(ProductEditModel edit)
        {

            var user = _unit
                .UserRepository.Get(x => x.Email == User.Identity.Name)
                .FirstOrDefault();
            if (user != null)
            {
                Product product = null;
                if (edit.Id != 0)
                {
                    product = _unit
                        .ProductRepository
                        .Get(x => x.Id == edit.Id)
                        .FirstOrDefault();
                }

                var category = int.Parse(Request.Form["Categories"].ToString());
                if (product != null)
                {
                    if (edit.Image != null)
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image", edit.Image.FileName);
                        await edit.Image.CopyToAsync(new FileStream(path, FileMode.Create));
                        _unit.ImageRepository.Insert(new Image() { Path = edit.Image.FileName });
                        _unit.Save();
                        product.ImageId = _unit.ImageRepository.Get(x => x.Path == edit.Image.FileName).FirstOrDefault().Id;
                    }
                    product.Model = edit.Model;
                    product.Producer = edit.Producer;
                    product.Price = edit.Price;
                    product.Description = edit.Description;
                    product.CategoryId = category;
                    product.Quantity = edit.Quantity;
                    _unit
                        .ProductRepository
                        .Update(product);
                    _unit.Save();
                }
                else
                {
                    int id = 7;
                    if (edit.Image != null)
                    {
                        edit.Image = edit.Image;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image", edit.Image.FileName);
                        await edit.Image.CopyToAsync(new FileStream(path, FileMode.Create));
                        _unit.ImageRepository.Insert(new Image() { Path = edit.Image.FileName });
                        _unit.Save();
                        id = _unit.ImageRepository.Get(x => x.Path == edit.Image.FileName).FirstOrDefault().Id;
                    }
                    product = new Product()
                    {
                        Model = edit.Model,
                        Producer = edit.Producer,
                        Price = edit.Price,
                        Description = edit.Description,
                        CategoryId = category,
                        ImageId = id,
                        Quantity = edit.Quantity,
                    };

                    _unit
                        .ProductRepository
                        .Insert(product);
                    _unit.Save();
                }

                return Redirect("/admin/products");
            }
            return RedirectToAction("Index", "Home");
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