using CourseProjectDB.Areas.Customer.Controllers;
using CP.DataAccess.Data;
using CP.DataAccess.Repository;
using CP.DataAccess.Repository.IRepository;
using CP.Models.Models;
using CP.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseProjectDB.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
	[Authorize(Roles = "Admin, Employee")]
	public class CurrencyController : Controller
	{
		private readonly IRegister _register;

		public CurrencyController(IRegister register)
		{

			_register = register;
		}

		public IActionResult Index()
		{
			List<InfoAboutCurrency> objectsFromDb = _register.CurrencyInfo.GetAll().ToList();
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
				IAC = _register.CurrencyInfo.GetFirstOrDefault(u=> u.ID == id);
				return View(IAC);
			}
		}
		[HttpPost]
		public IActionResult Upsert(InfoAboutCurrency IAC) 
		{
			if(IAC.ID == 0) 
			{
				_register.CurrencyInfo.Add(IAC);
				TempData["success"] = "Валюту було додано успішно!";
			}
			else
			{
				_register.CurrencyInfo.Update(IAC);
				TempData["success"] = "Валюту було оновлено успішно!";
			}
			_register.Save();
			return RedirectToAction("Index");
		}

		#region API CALLS
		[HttpGet]
		public IActionResult GetAll()
		{
			List<InfoAboutCurrency> ObjectsFromDb = _register.CurrencyInfo.GetAll().ToList();
			return Json(new { data = ObjectsFromDb });
		}
		[HttpDelete]
		public IActionResult Delete(int? id) 
		{ 
			var CurrencyToBeDeleted = _register.CurrencyInfo.GetFirstOrDefault(u=>u.ID== id);
			if (CurrencyToBeDeleted == null)
			{
				return Json(new { succes = false, message = "Помилка під час видалення" });
			}
			_register.CurrencyInfo.Delete(CurrencyToBeDeleted);
			_register.Save();

			return Json(new { succes = true, message = "Валюту було видалено успішно!" });
		}
		#endregion
	}
}
