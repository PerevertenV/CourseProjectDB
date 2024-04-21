using CP.DataAccess.Repository.IRepository;
using CP.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseProjectDB.Areas.Admin.Controllers
{
	[Authorize]
	[Authorize(Roles = "Admin")]
	[Area("Admin")]
	public class UserController : Controller
    {
        private readonly IRegister _register;
        public UserController(IRegister register)
        {
            _register = register;
        }
        public IActionResult Index()
        {
            List<User> ListOfObjects = _register.User.GetAll().ToList();
            return View(ListOfObjects);
        }

		#region API CALLS
		[HttpGet]
		public IActionResult GetAll()
		{
			List<User> ObjectsFromDb = _register.User.GetAll().ToList();
			return Json(new { data = ObjectsFromDb });
		}
		[HttpDelete]
		public IActionResult Delete(int? id)
		{
			var CurrencyToBeDeleted = _register.User.GetFirstOrDefault(u => u.ID == id);
			if (CurrencyToBeDeleted == null)
			{
				return Json(new { succes = false, message = "Помилка під час видалення" });
			}
			_register.User.Delete(CurrencyToBeDeleted);
			_register.Save();

			return Json(new { succes = true, message = "Користувача було видалено успішно!" });
		}
		#endregion
	}
}
