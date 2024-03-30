using CourseProjectDB.Areas.Customer.Controllers;
using CP.DataAccess.Data;
using CP.Models.Models;
using CP.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseProjectDB.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = SD.Role_Admin)]
	public class CurrencyController : Controller
	{
		private readonly ApplicationDbContext _db;

		public CurrencyController(ApplicationDbContext db)
		{
			
			_db = db;
		}

		public IActionResult Index()
		{
			List<InfoAboutCurrency> objectsFromDb = _db.InfoAboutCurrency.ToList();
			return View(objectsFromDb);
		}
		public IActionResult Upsert(int? id) // update + insert = upsert))
		{ 
			InfoAboutCurrency IAC = new();
			if(id == null || id == 0)
			{
				return View(IAC);
			}
			else
			{
				IAC = _db.InfoAboutCurrency.FirstOrDefault(u=> u.ID == id);
				return View(IAC);
			}
		}
		[HttpPost]
		public IActionResult Upsert(InfoAboutCurrency IAC) 
		{
			if(IAC.ID == 0) 
			{
				_db.InfoAboutCurrency.Add(IAC);
				TempData["success"] = "Валюту було додано успішно!";
			}
			else
			{
				_db.InfoAboutCurrency.Update(IAC);
				TempData["success"] = "Валюту було оновлено успішно!";
			}
			_db.SaveChanges();
			return RedirectToAction("Index");
		}

		#region API CALLS
		[HttpGet]
		public IActionResult GetAll()
		{
			List<InfoAboutCurrency> ObjectsFromDb = _db.InfoAboutCurrency.ToList();
			return Json(new { data = ObjectsFromDb });
		}
		[HttpDelete]
		public IActionResult Delete(int? id) 
		{ 
			var CurrencyToBeDeleted = _db.InfoAboutCurrency.FirstOrDefault(u=>u.ID== id);
			if (CurrencyToBeDeleted == null)
			{
				return Json(new { succes = false, message = "Помилка під час видалення" });
			}
			_db.InfoAboutCurrency.Remove(CurrencyToBeDeleted);
			_db.SaveChanges();

			return Json(new { succes = true, message = "Валюту було видалено успішно!" });
		}
		#endregion
	}
}
