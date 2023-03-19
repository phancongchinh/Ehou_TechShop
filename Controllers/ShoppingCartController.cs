using Microsoft.AspNetCore.Mvc;

namespace TechShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}