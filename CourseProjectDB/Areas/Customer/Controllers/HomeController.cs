using CourseProjectDB.Models;
using CP.DataAccess.Data;
using CP.DataAccess.Repository;
using CP.DataAccess.Repository.IRepository;
using CP.Models.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CourseProjectDB.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRegister _register;

        public HomeController(ILogger<HomeController> logger, IRegister register)
        {
            _logger = logger;
            _register = register;
        }

        public IActionResult Index()
        {
            List<InfoAboutCurrency> objectsFromDb= _register.CurrencyInfo.GetAll().ToList();
            return View(objectsFromDb);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
		#region API CALLS
		[HttpGet]
		public IActionResult GetAll()
		{
			List<InfoAboutCurrency> ObjectsFromDb = _register.CurrencyInfo.GetAll().ToList();
			return Json(new { data = ObjectsFromDb });
		}
		#endregion
	}
}
