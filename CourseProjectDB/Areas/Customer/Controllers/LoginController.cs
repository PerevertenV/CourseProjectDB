using Microsoft.AspNetCore.Mvc;

namespace CourseProjectDB.Areas.Customer.Controllers
{
    public class LoginController : Controller
    {
        [Area("Customer")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
