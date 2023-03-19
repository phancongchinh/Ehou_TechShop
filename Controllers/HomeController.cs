using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TechShop.Data;
using TechShop.Models;

namespace TechShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly RepositoryContainer _container = new();

        public IActionResult Index()
        {
            ViewData["Categories"] = _container.CategoryRepository.Get();
            var products = _container.ProductRepository.Get(includeProperties: "Image,Category");
            ViewData["Products"] = products;
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorVM {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}