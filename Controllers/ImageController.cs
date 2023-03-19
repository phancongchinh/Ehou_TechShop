using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TechShop.Data;
using TechShop.Models;

namespace TechShop.Controllers
{
    public class ImageController : Controller
    {
        private readonly RepositoryContainer _container = new ();

        // GET
        [Route("image/{name}")]
        public IActionResult Get(string path)
        {
            var image = _container.ImageRepository.Get(x => x.Path == path).FirstOrDefault();
            if (image == null) return View("~/Views/Error/Error404.cshtml", new ErrorVM());
            
            var fullpath = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles", image.Path);
            FileStream img;
            try
            {
                img = System.IO.File.OpenRead(fullpath);
            }
            catch (Exception e)
            {
                fullpath = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles", "empty.jpg");
                img = System.IO.File.OpenRead(fullpath);
            }
            return File(img, "image/*");
        }
    }
}