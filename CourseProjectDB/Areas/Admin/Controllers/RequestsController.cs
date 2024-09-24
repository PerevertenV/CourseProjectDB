using CP.DataAccess.Repository.IRepository;
using CP.Models;
using CP.Models.VModels;
using CP.Utility.StatciData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;

namespace CourseProjectDB.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class RequestsController : Controller
    {
        private readonly IRegister _register;
        public RequestsController(IRegister register)
        {
            _register = register;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult FirstRequest()
        {
            RequestsVM requestsVM = new()
            {
                SelectUser = _register.User.GetAll()
                .Where(u => u.role == SD.Role_Employee || u.role == SD.Role_Admin)
                .Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.ID.ToString()
                })
            };
            return View(requestsVM);
        }
        [HttpPost]
		public IActionResult FirstRequest(RequestsVM requestsVM)
		{
			RequestsVM requestsVMUpdated = new RequestsVM();
			var purchaseList = _register.Purchase.GetAll();
			var selectListItems = new List<SelectListItem>();

			foreach (var purchase in purchaseList)
			{
				if (purchase.IDOfEmployee == requestsVM.User.ID)
				{
					var currency = _register.CurrencyInfo
						.GetFirstOrDefault(u => u.ID == purchase.CurrencyID);
					if (currency != null)
					{
						selectListItems.Add(new SelectListItem
						{
							Text = currency.Name,
							Value = currency.ID.ToString()
						});
					}
				}
			}
			requestsVMUpdated.SelectUser = _register.User.GetAll()
				.Where(u => u.role == SD.Role_Employee || u.role == SD.Role_Admin)
				.Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.ID.ToString()
				});
			requestsVMUpdated.User = requestsVM.User;
			requestsVMUpdated.SelectIAC = selectListItems;
			return View(requestsVMUpdated);
		}
		public IActionResult SecondRequest() 
		{
			List<User> ObjectsFromDb = new List<User>();
			ObjectsFromDb = _register.User.GetAll().Where(u => u.role == SD.Role_Employee).ToList();
			return View(ObjectsFromDb);
		}
        public IActionResult ThirdRequest()
        {
            return View();
        }
        [HttpPost]
		public IActionResult ThirdRequest(RequestsVM r) 
		{
			List<Purchase> ListOfPurchase = _register.Purchase.GetAll().Where(u=>u.DateOfMakingPurchase 
			>= r.StartDateTime && u.DateOfMakingPurchase <= r.EndDateTime).ToList();
            ViewBag.ListOfPurchase = ListOfPurchase;
            return View();
		}
		public IActionResult FourthRequest()
        {
            return View();
        }
        [HttpPost]
		public IActionResult FourthRequest(RequestsVM r) 
		{
			List<Purchase> ListOfPurchase = _register.Purchase.GetAll().Where(u=>u.DateOfMakingPurchase 
			>= r.StartDateTime && u.DateOfMakingPurchase <= r.EndDateTime).ToList();
            ViewBag.Count = ListOfPurchase.Count;
            return View();
		}
        public IActionResult FifthRequest()
        {
            int counter = 0;
            RequestsVM requestsVM= new RequestsVM();
            var purchaseList = _register.Purchase.GetAll();
            var currList = _register.CurrencyInfo.GetAll();
            var selectListItems = new List<SelectListItem>();
            foreach (var item in currList)
            {
                foreach (var purchase in purchaseList)
                {
                    if (item.ID == purchase.CurrencyID)
                    {
                        counter++;
                    }
                }
                selectListItems.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = counter.ToString()
                });
                counter = 0;

            }
            requestsVM.SelectIAC = selectListItems;
            return View(requestsVM);
        }
        public IActionResult SixthRequest() 
        {
            int counter = 0;
            RequestsVM requestVM = new RequestsVM();
            List<Purchase> pl = _register.Purchase.GetAll().ToList();
            List<User> ul = _register.User.GetAll().Where(u=>u.role==SD.Role_Customer).ToList();
            Dictionary<int, int> p = new Dictionary<int, int>();
            foreach (User u in ul) 
            {
                foreach(Purchase pur in pl) 
                { 
                    if(u.ID == pur.IDOfUser) 
                    {
                        counter++;
                    }
                }
                p.Add(u.ID, counter);
                counter = 0;
            }
            int IdOfUser = p.OrderByDescending(kvp => kvp.Value).First().Key;
            int Value = p.OrderByDescending(kvp => kvp.Value).First().Value;
            requestVM.User = _register.User.GetFirstOrDefault(u => u.ID == IdOfUser);
            ViewBag.Value = Value;
            return View(requestVM);
        }
        public IActionResult SeventhRequest() 
        {
            List<Payments> paymentsList = _register.Payments.GetAll().ToList();

            // Список типів платежів, які вас цікавлять
            List<string> typesOfInterest = new List<string>
            {
                SD.Payments_Type_Salary,
                SD.Payments_Type_BC,
                SD.Payments_Type_Util
            };


            DateTime oneMonthAgo = DateTime.Now.AddMonths(-1);
            List<string> paymentsLastMonth = paymentsList
                .Where(p => p.DateOfMakingPayments > oneMonthAgo && p.DateOfMakingPayments <= DateTime.Now)
                .Select(p => p.Type)
                .Distinct()
                .ToList();

            List<string> typesNotPerformedLastMonth = typesOfInterest
                .Where(t => !paymentsLastMonth.Contains(t))
                .ToList();

            ViewBag.ListWithTypes = typesNotPerformedLastMonth;
            return View();
        }
        public IActionResult EighthRequest() 
        {
            // Отримання всіх покупок
            List<Purchase> purchaseList = _register.Purchase.GetAll().ToList();

            // Отримання всіх користувачів з роллю клієнта
            List<User> userList = _register.User.GetAll()
                .Where(u => u.role == SD.Role_Customer).ToList();

            // Дата одного місяця тому
            DateTime oneMonthAgo = DateTime.Now.AddMonths(-1);

            // Відфільтрувати покупки за останній місяць
            List<Purchase> purchasesLastMonth = purchaseList
                .Where(p => p.DateOfMakingPurchase > oneMonthAgo && p.DateOfMakingPurchase <= DateTime.Now).ToList();

            // Отримати список ID користувачів, які зробили покупки за останній місяць
            List<int?> userIdsWithPurchasesLastMonth = purchasesLastMonth
                .Select(p => p.IDOfUser).Distinct().ToList();

            // Відфільтрувати користувачів, які не робили покупок за останній місяць
            List<User> usersWithoutPurchasesLastMonth = userList
                .Where(u => !userIdsWithPurchasesLastMonth.Contains(u.ID)).ToList();

            // Передати список користувачів у ViewBag для відображення
            ViewBag.UserList = usersWithoutPurchasesLastMonth;

            return View();
        }
        public IActionResult NinethRequest() 
        {
            // Отримання всіх покупок
            List<Purchase> purchaseList = _register.Purchase.GetAll().ToList();

            // Отримання всіх користувачів з роллю клієнта
            List<InfoAboutCurrency> currList = _register.CurrencyInfo.GetAll().ToList();

            // Дата одного місяця тому
            DateTime oneMonthAgo = DateTime.Now.AddMonths(-1);

            // Відфільтрувати покупки за останній місяць
            List<Purchase> purchasesLastMonth = purchaseList
                .Where(p => p.DateOfMakingPurchase > oneMonthAgo && p.DateOfMakingPurchase <= DateTime.Now).ToList();

            // Отримати список ID користувачів, які зробили покупки за останній місяць
            List<int?> currencyIdsWithPurchasesLastMonth = purchasesLastMonth
                .Select(p => p.CurrencyID).Distinct().ToList();

            // Відфільтрувати користувачів, які не робили покупок за останній місяць
            List<InfoAboutCurrency> currWithoutPurchasesLastMonth = currList
                .Where(u => !currencyIdsWithPurchasesLastMonth.Contains(u.ID)).ToList();

            // Передати список користувачів у ViewBag для відображення
            ViewBag.CurrList = currWithoutPurchasesLastMonth;

            return View();
        } 
        public IActionResult TenthRequest() 
        {
            List<Purchase> operationList = _register.Purchase.GetAll().ToList();

            List<User> employeeList = _register.User.GetAll()
                .Where(e => e.role == SD.Role_Employee).ToList();

            DateTime oneWeekAgo = DateTime.Now.AddDays(-7);

            List<Purchase> operationsLastWeek = operationList
                .Where(o => o.DateOfMakingPurchase > oneWeekAgo && o.DateOfMakingPurchase <= DateTime.Now)
                .ToList();

            List<int?> employeeIdsWithOperationsLastWeekNullable = operationsLastWeek
                .Select(o => o.IDOfEmployee)
                .Distinct()
                .ToList();

            List<int> employeeIdsWithOperationsLastWeek = employeeIdsWithOperationsLastWeekNullable
                .Where(id => id.HasValue)
                .Select(id => id.Value)
                .ToList();

            List<User> employeesWithOperationsLastWeek = employeeList
                .Where(e => employeeIdsWithOperationsLastWeek.Contains(e.ID))
                .ToList();

            List<User> employeesWithoutOperationsLastWeek = employeeList
                .Where(e => !employeeIdsWithOperationsLastWeek.Contains(e.ID))
                .ToList();

            ViewBag.EmployeeWith = employeesWithOperationsLastWeek;
            ViewBag.EmployeeWithout = employeesWithoutOperationsLastWeek;

            return View();
        }
        public IActionResult EleventhRequest() 
        {
            List<Purchase> allPurchases = _register.Purchase.GetAll().ToList();

            foreach (Purchase purchase in allPurchases)
            {
                purchase.InfoAboutCurrency = _register.CurrencyInfo
                    .GetFirstOrDefault(u => u.ID == purchase.CurrencyID);
            }

            List<InfoAboutCurrency> allCurrencies = _register.CurrencyInfo.GetAll().ToList();

            var maxPurchasesByCurrency = allPurchases
                .GroupBy(p => p.CurrencyID)
                .Select(g => new
                {
                    CurrencyID = g.Key,
                    MaxPurchase = g.OrderByDescending(p => p.SumInUAH).FirstOrDefault()
                })
                .ToList();
            // Передача результатів у ViewBag
            ViewBag.Purchase = maxPurchasesByCurrency;
            return View();
        }

        #region API CALLS
        [HttpGet]
		public IActionResult GetEmployee()
		{
			IEnumerable<User> ObjectsFromDb;
			ObjectsFromDb = _register.User.GetAll().Where(u=>u.role == SD.Role_Employee).ToList();
			return Json(new { data = ObjectsFromDb });
		}
		#endregion
	}
}
