using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechShop.Data;
using TechShop.Models.Dto;
using TechShop.Models.Entity;

namespace TechShop.Controllers
{
    
    [Authorize(Roles = "Administrator")]
    public class BackofficeController : Controller
    {
        private readonly UnitOfWork _unit = new();

        /* List all users */
        [HttpGet]
        [Route("/admin/users")]
        public IActionResult Users()
        {
            var users = _unit.UserRepository.Get(includeProperties: "UserRole").ToList();
            return View("Users", users);
        }

        /* List all categories */
        [HttpGet]
        [Route("/admin/categories")]
        public IActionResult Categories()
        {
            var categories = _unit.CategoryRepository.Get().ToList();
            return View("Categories", categories);
        }

        /* List all product */
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

        /* Create new product */
        [HttpGet]
        [Route("/admin/products/new")]
        public IActionResult AddNewProduct()
        {
            return View("AddNewProduct", new ProductEditModel());
        }

        /* Update product information */
        [HttpGet]
        [Route("/admin/products/{id}")]
        public IActionResult EditProduct(int id)
        {
            var product = _unit.ProductRepository.Get(x => x.Id == id && x.IsDeleted == false, includeProperties: "Category,Image")
                .FirstOrDefault();

            if (product == null) return NotFound();

            ProductEditModel model = new()
            {
                Model = product.Model,
                Producer = product.Producer,
                Price = product.Price,
                Id = product.Id,
                Description = product.Description,
                ImageName = product.Image.Path,
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


        public async Task<IActionResult> SaveAsync(ProductEditModel editModel)
        {
            var product = _unit.ProductRepository.Get(x => x.Id == editModel.Id).FirstOrDefault();

            if (product == null) return NotFound();
            
            
            // Product product = null;
            // if (edit.Id != 0)
            // {
            //     product = _unit
            //         .ProductRepository
            //         .Get(x => x.Id == edit.Id)
            //         .FirstOrDefault();
            // }

            var category = int.Parse(Request.Form["Categories"].ToString());
            
            // if (editModel.Image != null)
            // {
            //     var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image", editModel.Image.FileName);
            //     await editModel.Image.CopyToAsync(new FileStream(path, FileMode.Create));
            //     _unit.ImageRepository.Insert(new Image() {Path = editModel.Image.FileName});
            //     _unit.Save();
            //     product.ImageId = _unit.ImageRepository.Get(x => x.Path == editModel.Image.FileName).FirstOrDefault()
            //         .Id;
            // }
            
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image", editModel.Image.FileName);
            await editModel.Image.CopyToAsync(new FileStream(path, FileMode.Create));
            _unit.ImageRepository.Insert(new Image() {Path = editModel.Image.FileName});
            _unit.Save();
            product.ImageId = _unit.ImageRepository.Get(x => x.Path == editModel.Image.FileName).FirstOrDefault()
                .Id;

            product.Model = editModel.Model;
            product.Producer = editModel.Producer;
            product.Price = editModel.Price;
            product.Description = editModel.Description;
            product.CategoryId = category;
            product.Quantity = editModel.Quantity;
            _unit.ProductRepository.Update(product);
            _unit.Save();
            
            // if (product != null)
            // {
            //     if (editModel.Image != null)
            //     {
            //         var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image", editModel.Image.FileName);
            //         await editModel.Image.CopyToAsync(new FileStream(path, FileMode.Create));
            //         _unit.ImageRepository.Insert(new Image() {Path = editModel.Image.FileName});
            //         _unit.Save();
            //         product.ImageId = _unit.ImageRepository.Get(x => x.Path == editModel.Image.FileName).FirstOrDefault()
            //             .Id;
            //     }
            //
            //     product.Model = editModel.Model;
            //     product.Producer = editModel.Producer;
            //     product.Price = editModel.Price;
            //     product.Description = editModel.Description;
            //     product.CategoryId = category;
            //     product.Quantity = editModel.Quantity;
            //     _unit
            //         .ProductRepository
            //         .Update(product);
            //     _unit.Save();
            // }
            // else
            // {
            //     int id = 7;
            //     if (editModel.Image != null)
            //     {
            //         editModel.Image = editModel.Image;
            //         var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image", editModel.Image.FileName);
            //         await editModel.Image.CopyToAsync(new FileStream(path, FileMode.Create));
            //         _unit.ImageRepository.Insert(new Image() {Path = editModel.Image.FileName});
            //         _unit.Save();
            //         id = _unit.ImageRepository.Get(x => x.Path == editModel.Image.FileName).FirstOrDefault().Id;
            //     }
            //
            //     product = new Product()
            //     {
            //         Model = editModel.Model,
            //         Producer = editModel.Producer,
            //         Price = editModel.Price,
            //         Description = editModel.Description,
            //         CategoryId = category,
            //         ImageId = id,
            //         Quantity = editModel.Quantity,
            //     };
            //
            //     _unit
            //         .ProductRepository
            //         .Insert(product);
            //     _unit.Save();
            // }

            return RedirectToAction("Products", "Backoffice");


            // var user = _unit.UserRepository.Get(x => x.Email == User.Identity.Name)
            //     .FirstOrDefault();
            // if (user != null)
            // {
            //     
            // }
            //
            // return RedirectToAction("Index", "Home");
        }


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