using CP.DataAccess.Repository.IRepository;
using CP.Models;
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
			var UserToBeDeleted = _register.User.GetFirstOrDefault(u => u.ID == id);
			if (UserToBeDeleted == null)
			{
				return Json(new { succes = false, message = "Помилка під час видалення" });
			}
			List<Purchase> purchases = _register.Purchase.GetAll().ToList();
			foreach (Purchase purchase in purchases) 
			{
				if(purchase.IDOfUser == id) 
				{
					purchase.IDOfUser = null;
				}
			}
			_register.User.Delete(UserToBeDeleted);
			_register.Save();

			return Json(new { succes = true, message = "Користувача було видалено успішно!" });
		}
		#endregion
	}
}
