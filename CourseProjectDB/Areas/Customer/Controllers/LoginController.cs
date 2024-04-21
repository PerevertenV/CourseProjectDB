using CP.DataAccess.Repository.IRepository;
using CP.Models.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseProjectDB.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class LoginController : Controller
    {
        private readonly IRegister _register;
        public LoginController(IRegister register)
        {
            _register = register;       
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
                    string Decodet = _register.User.DecryptString(user.Password);
                    success = false;
                    if(obj.Password == Decodet) 
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Name),
                            new Claim(ClaimTypes.Role, user.role)
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
