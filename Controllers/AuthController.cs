using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using TechShop.Data;
using TechShop.Models.Dto;
using TechShop.Models.Entity;
using TechShop.Services;

namespace TechShop.Controllers
{
    public class AuthController : Controller
    {
        private RepositoryContainer _container = new();

        [HttpPost]
        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new(ClaimsIdentity.DefaultRoleClaimType, user.UserRole.Name)
            };
            ClaimsIdentity id = new(
                claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType
            );

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = _container.UserRepository
                .Get(x => x.Email == model.Email && x.PasswordHash == PasswordConverter.Hash(model.Password),
            includeProperties: "UserRole").FirstOrDefault();
            if (user != null)
            {
                await Authenticate(user);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Incorrect email or password input");

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = _container.UserRepository.Get(x => x.Email == model.Email).FirstOrDefault();
            if (user == null)
            {
                user = new User
                {
                    Name = model.Name,
                    Phone = model.Phone,
                    Email = model.Email,
                    PasswordHash = PasswordConverter.Hash(model.Password),
                };
                var userRole = _container.UserRoleRepository.Get(x => x.Id == 2).FirstOrDefault();
                if (userRole != null) user.UserRole = userRole;

                _container.UserRepository.Insert(user);
                _container.Save();

                await Authenticate(user);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "The same account already exist");
            return View(model);
        }
    }
}