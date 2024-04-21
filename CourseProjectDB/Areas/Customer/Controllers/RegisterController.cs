using CP.Models.Models;
using CP.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using CP.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;



namespace CourseProjectDB.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class RegisterController : Controller
    {
        readonly IRegister _register;

        public RegisterController(IRegister register)
        {
            _register = register;   
        }
        public IActionResult Index()
        {
            Dictionary<string, string> list = new Dictionary<string, string>() 
            {
                { SD.Role_Admin, "Адмін" }, 
                { SD.Role_Employee, "Працівник" },
                { SD.Role_Customer, "Клієнт" }
            };
            IEnumerable<SelectListItem> RoleList = list.Select(u => new SelectListItem
            {
                Text = u.Value,
                Value = u.Key
            });
            ViewBag.List = RoleList;
            return View();
        }
        [HttpPost]
        public IActionResult Index(User obj, IFormCollection form)
        {
            string confirmPassword = form["confirmPassword"];
            List<User> users =  _register.User.GetAll().ToList();
            bool PasswordChecker = Regex.IsMatch(obj.Password, "[a-zA-Z]");
            foreach (User user in users) 
            {
                if(obj.UserName == user.UserName) 
                {
                    ModelState.AddModelError("username", "Нажаль, користувач із таким username вже існує");
                    return View();
                    //break;
                }
            }
            if (obj.Password.Length < 6 || obj.Password.Length > 15)
            {
                ModelState.AddModelError("password", "Пароль має містити від 5 до 15 символів");
                return View();
            }
            else if (!PasswordChecker)
            {
                ModelState.AddModelError("password", "Пароль має містити хочаб одну латинську букву");
                return View();
            }
            else if (confirmPassword != obj.Password)
            {
                ModelState.AddModelError("password", "Паролі мають збігатися");
                return View();
            }
            else
            {
                string WhichRole = User.IsInRole("Admin") ? obj.role : SD.Role_Customer;
                var UserToAdding = new User
                {
                    UserName = obj.UserName,
                    Name = obj.Name,
                    Password = _register.User.PasswordHashCoder(obj.Password),
                    role = WhichRole
                };

                _register.User.Add(UserToAdding);
                _register.Save();
                if (!User.Identity.IsAuthenticated) 
                { 
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, obj.Name),
                        new Claim(ClaimTypes.Role, WhichRole)
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
                }
                TempData["success"] = "Обліковий запис було створено успішно! 😀";
                return Redirect("Home/Index");
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
