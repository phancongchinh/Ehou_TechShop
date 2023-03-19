﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TechShop.Data;

namespace TechShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly RepositoryContainer _container = new();

        [HttpGet]
        [Route("Product/{id}")]
        public IActionResult Index(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Home");

            var product = _container.ProductRepository.Get(x => x.Id == id, includeProperties: "Image,Category")
                .FirstOrDefault();

            if (product == null) return NotFound();

            return View(product);
        }
    }
}