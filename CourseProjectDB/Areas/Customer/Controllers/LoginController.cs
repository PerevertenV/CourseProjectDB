using CP.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CP.Models;
using CP.DataAccess.Services.IServices;

namespace CourseProjectDB.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class LoginController : Controller
    {
        private readonly IRegister _register;
        private readonly IServiceBL _service;
        public LoginController(IRegister register, IServiceBL service)
        {
            _register = register;       
            _service = service;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(User obj) 
        {
            bool success = true;
            List<User> users = _register.User.GetAll().ToList();
            foreach (var user in users) 
            {
                if(user.UserName == obj.UserName) 
                {
                    string Decodet = _service.User.DecryptString(user.Password);
                    success = false;
                    if(obj.Password == Decodet) 
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Name),
                            new Claim(ClaimTypes.Role, user.role),
                            new Claim("UserID", user.ID.ToString())
                        };

                        var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true // Можна змінити на false, якщо не потрібно, щоб користувач залишався автентифікованим після закриття браузера
                        };

                        HttpContext.SignInAsync(
                           CookieAuthenticationDefaults.AuthenticationScheme,
                           new ClaimsPrincipal(claimsIdentity),
                           authProperties).GetAwaiter().GetResult();

                        TempData["success"] = "Ви ввійшли успішно! 😀";
                        return Redirect("Home/Index");
                    }
                    else 
                    {
                        ModelState.AddModelError("password", "Некоректний пароль, спробуйте ще раз");
                        return View();
                    }
                }
            }
            if (success) 
            {
                ModelState.AddModelError("username", "Ми не знайшли користувача із таким username");
                return View();
            }
            return View();
        }
    }
}
