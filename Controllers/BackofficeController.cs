using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechShop.Data;
using TechShop.Models.Entity;
using TechShop.Models.ViewModels;

namespace TechShop.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class BackofficeController : Controller
    {
        private readonly UnitOfWork _unit = new();

        /* USERS MANAGEMENT */
        [HttpGet]
        [Route("/admin/users")]
        public IActionResult Users()
        {
            var users = _unit.UserRepository.Get(includeProperties: "UserRole").ToList();
            return View("Users", users);
        }

        /* CATEGORIES MANAGEMENT */
        [HttpGet]
        [Route("/admin/categories")]
        public IActionResult Categories()
        {
            var errMsg= TempData["ErrorMessage"] as string;
            ViewBag.ErrorMessage = errMsg!;
            
            var categories = _unit.CategoryRepository.Get().ToList();
            return View("Categories", categories);
        }

        [HttpGet]
        [Route("/admin/categories/create")]
        public IActionResult CreateCategory()
        {
            return View("CategoryInfo", new Category());
        }

        [HttpGet]
        [Route("/admin/categories/{id}")]
        public IActionResult CategoryInfo(int id)
        {
            var category = _unit.CategoryRepository.Get(x => x.Id == id).FirstOrDefault();
            if (category == null) return NotFound();

            return View("CategoryInfo", category);
        }

        [HttpPost]
        [Route("/admin/categories")]
        public IActionResult SaveCategory(Category category)
        {
            if (category.Id != null)
            {
                _unit.CategoryRepository.Update(category);
            }
            else
            {
                _unit.CategoryRepository.Insert(category);
            }

            _unit.Save();

            return RedirectToAction("Categories", "Backoffice");
        }

        [HttpGet]
        [Route("/admin/categories/delete/{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = _unit.CategoryRepository.Get(x => x.Id == id, includeProperties: "Products")
                .FirstOrDefault();

            if (category == null) return NotFound();

            if (category.Products.Any())
            {
                TempData["ErrorMessage"]="Can not delete because there's some product in this category!";
                return RedirectToAction("Categories", "Backoffice");
            }

            _unit.CategoryRepository.Delete(category);
            _unit.Save();

            return RedirectToAction("Categories", "Backoffice");
        }


        /* PRODUCTS MANAGEMENT */
        [HttpGet]
        [Route("/admin/products")]
        public IActionResult Products()
        {
            User user = _unit.UserRepository.Get(x => x.Email == User.Identity.Name, includeProperties: "UserRole")
                .FirstOrDefault();
            if (user == null) return NotFound();

            if (user.UserRole.Name != "Administrator")
                return RedirectToAction("Index", "Home");

            var products = _unit.ProductRepository.Get(x => x.IsDeleted == false, includeProperties: "Image,Category");
            ViewData["Products"] = products;
            ViewData["Categories"] = _unit.CategoryRepository.Get();
            return View("Products", products);
        }

        [HttpGet]
        [Route("/admin/products/create")]
        public IActionResult CreateProduct()
        {
            return View("ProductCreate", new ProductVM());
        }

        [HttpGet]
        [Route("/admin/products/{id}")]
        public IActionResult EditProduct(int id)
        {
            var product = _unit.ProductRepository
                .Get(x => x.Id == id && x.IsDeleted == false, includeProperties: "Category,Image")
                .FirstOrDefault();

            if (product == null) return NotFound();

            ProductVM model = new()
            {
                Model = product.Model,
                Producer = product.Producer,
                Price = product.Price,
                Id = product.Id,
                Description = product.Description,
                ImageName = product.Image?.Path,
                Quantity = product.Quantity
            };

            var categories = _unit.CategoryRepository.Get().ToList();
            var select = categories.FirstOrDefault(x => x.Id == product.CategoryId);
            var selectList = new SelectList(categories, "Id", "Name", select);
            model.Categories = selectList;

            return View("ProductsEdit", model);
        }

        [HttpGet]
        [Route("/admin/products/remove/{id}")]
        public IActionResult RemoveProduct(int id)
        {
            var product = _unit.ProductRepository.Get(x => x.Id == id).FirstOrDefault();

            if (product == null) return NotFound();

            product.IsDeleted = true;

            _unit.ProductRepository.Update(product);
            _unit.Save();

            return RedirectToAction("Products", "Backoffice");
        }


        public async Task<IActionResult> SaveProductAsync(ProductVM productVm)
        {
            var product = _unit.ProductRepository.Get(x => x.Id == productVm.Id).FirstOrDefault();

            if (product == null) return NotFound();

            var category = int.Parse(Request.Form["Categories"].ToString());

            if (productVm.Image != null)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image", productVm.Image.FileName);
                await productVm.Image.CopyToAsync(new FileStream(path, FileMode.Create));
                Image image = new() {Path = productVm.Image.FileName};
                _unit.ImageRepository.Insert(image);
                _unit.Save();
                product.ImageId = image.Id;
            }

            product.Model = productVm.Model;
            product.Producer = productVm.Producer;
            product.Price = productVm.Price;
            product.Description = productVm.Description;
            product.CategoryId = category;
            product.Quantity = productVm.Quantity;
            _unit.ProductRepository.Update(product);
            _unit.Save();

            return RedirectToAction("Products", "Backoffice");
        }


        /* PURCHASES MANAGEMENT */
        [HttpGet]
        [Route("/admin/purchases")]
        public IActionResult Purchases()
        {
            var purchases = _unit.PurchaseRepository.Get(includeProperties: "User,PurchaseProducts.Product")
                .ToList();
            return View(purchases);
        }

        [HttpPost]
        [Route("/admin/purchase")]
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