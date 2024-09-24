using CP.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using CP.Models;
using CP.Models.VModels;
using CP.Utility.StatciData;

namespace CourseProjectDB.Areas.Admin.Controllers
{
    [Authorize]
	[Area("Admin")]
	[Authorize(Roles = SD.Role_Admin)]
	public class PaymentsController : Controller
	{
		private readonly IRegister _register;

		public PaymentsController(IRegister register)
		{
			_register = register;
		}

		public IActionResult Index()
		{
			List<Payments> objectsFromDb = _register.Payments.GetAll().ToList();
			return View(objectsFromDb);
		}
		public IActionResult Check(int id) 
		{
			var Payment = _register.Payments.GetFirstOrDefault(u => u.ID == id);
			return View(Payment);
		}
		public IActionResult Insert() 
		{
			Dictionary<string, string> list = new Dictionary<string, string>()
			{
				{ SD.Payments_Type_BC , SD.Payments_Type_BC },
				{ SD.Payments_Type_Util, SD.Payments_Type_Util },
				{ SD.Payments_Type_Salary, SD.Payments_Type_Salary}
			};
			PaymentsVM paymentsVM = new()
			{
				CurrencyList = _register.CurrencyInfo.GetAll()
				.Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.ID.ToString()
				}),
				EmployeeList = _register.User.GetAll(u => u.role == SD.Role_Employee)
				.Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.ID.ToString()
				}),
				PaymentsTypeList = list.Select(u => new SelectListItem
				{
					Text = u.Value,
					Value = u.Key.ToString()
				}),
				Payments = new Payments()
			};
			return View(paymentsVM);
		}
		[HttpPost]
		public IActionResult Insert(PaymentsVM paymentsVM, int? selectedEmployeeId, int? selectedCurrencyId)
		{
			paymentsVM.Payments.DateOfMakingPayments = DateTime.Now;
			if (paymentsVM.Payments.ID == 0)
			{
				if(paymentsVM.Payments.Type == SD.Payments_Type_Salary) 
				{
					var selectedEmployee = _register.User.GetFirstOrDefault(u=> u.ID == selectedEmployeeId);
					paymentsVM.Payments.Description += "\n\n  Заробітня плата для працівника "
						+ selectedEmployee.Name +" була виплачена у розмірі "+ paymentsVM.Payments.Sum+" UAH.";
						
				}
				if(paymentsVM.Payments.Type == SD.Payments_Type_BC) 
				{
					var selectedCurrency = _register.CurrencyInfo.GetFirstOrDefault(u => u.ID == selectedCurrencyId);

					int CurrencyAdd = (int)(paymentsVM.Payments.Sum / selectedCurrency.AskedCoursePriceTo);
					selectedCurrency.AvailOfAskedCourse += CurrencyAdd;
					_register.CurrencyInfo.Update(selectedCurrency);

					paymentsVM.Payments.Description += "\n\n  Було закуплено валюту "
						+ selectedCurrency.Name + " на суму " + paymentsVM.Payments.Sum + " UAH.";
				}
				_register.Payments.Add(paymentsVM.Payments);
				TempData["success"] = "Платіж було створено успішно!";
			}
			_register.Save();
			return RedirectToAction("Index");
		}
	}
}
