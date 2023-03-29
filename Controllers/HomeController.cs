using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TechShop.Data;
using TechShop.Models;

namespace TechShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly UnitOfWork _unit = new();

        [HttpGet]
        public IActionResult Index()
        {
            var products = _unit.ProductRepository.Get(x=> x.IsDeleted == false, includeProperties: "Image,Category");
            ViewData["Products"] = products;
            ViewData["Categories"] = _unit.CategoryRepository.Get();
            return View(products);
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorVM {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}