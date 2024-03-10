using CourseProjectDB.Models;
using CP.DataAccess.Data;
using CP.Models.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CourseProjectDB.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            List<InfoAboutCurrency> objectsFromDb= _db.InfoAboutCurrency.ToList();
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
			List<InfoAboutCurrency> ObjectsFromDb = _db.InfoAboutCurrency.ToList();
			return Json(new { data = ObjectsFromDb });
		}
		#endregion
	}
}
