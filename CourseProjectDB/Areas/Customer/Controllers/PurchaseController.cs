using CP.DataAccess.Repository.IRepository;
using CP.Models;
using CP.Models.VModels;
using CP.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Stripe.Climate;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace CourseProjectDB.Areas.Customer.Controllers
{
    [Area("Customer")]
	[Authorize]
	public class PurchaseController : Controller
	{
		private readonly IRegister _register;
        public PurchaseController(IRegister register)
        {
			_register = register;
        }

        public IActionResult Index()
		{
			List<Purchase> objectsFromDb = _register.Purchase.GetAll().ToList();
			return View(objectsFromDb);
		}
        public IActionResult Create() 
        {
            PurchaseVM purchase = new() 
            {
                CurrencyList = _register.CurrencyInfo.GetAll()
                .Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.ID.ToString()
                }),
                Purchase = new Purchase()
            };
            if (User.IsInRole(SD.Role_Customer))
            {
                var user = HttpContext.User;
                var userId = user.FindFirstValue("UserID");
                purchase.Purchase.IDOfUser = int.Parse(userId);
                purchase.Purchase.User = _register.User.GetFirstOrDefault(u=>u.ID == purchase.Purchase.IDOfUser);
            }
            else 
            { 
                purchase.Purchase.IDOfUser = null; 
            }
            purchase.Purchase.State = SD.State_Created;

			return View(purchase);
        }
        [HttpPost]
        public IActionResult Create(PurchaseVM purchaseVM) 
        {
            
            Purchase purchase = new() 
            {
                SumInUAH = _register.Purchase.CountSumInUAH (purchaseVM.Purchase.SumOfCurrency,
                    Purchase.PDVPercent,
                    _register.CurrencyInfo.GetFirstOrDefault(u =>
                    u.ID==purchaseVM.Purchase.CurrencyID).AskedCoursePriceTo),
                 State=SD.State_Created,   
                 CurrencyID= purchaseVM.Purchase.CurrencyID,
                 IDOfUser = purchaseVM.Purchase.IDOfUser,
                 IDOfEmployee = null,
                 SumOfCurrency = purchaseVM.Purchase.SumOfCurrency,
                 DepositedMoney = 0,
                 MoneyToReturn = 0,
                 DateOfMakingPurchase = DateTime.Now,
            };
            _register.Purchase.Add(purchase);
            _register.Save();
            TempData["success"] = "Замовлення було створено успішно! 😀";
            if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee)) 
            {
                return RedirectToAction("Details", "Purchase", new { ID = purchase.ID });
            }
            else 
            {
                return RedirectToAction("Index", "Home");
            }
                
        }
        public IActionResult Details(int ID) 
        {

            PurchaseVM purchase = new() 
            {
                CurrencyList = _register.CurrencyInfo.GetAll()
                .Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.ID.ToString()
                }),
                Purchase = _register.Purchase.GetFirstOrDefault(u=>u.ID == ID),
            };
            purchase.Purchase.InfoAboutCurrency =
                _register.CurrencyInfo.GetFirstOrDefault(u => u.ID == purchase.Purchase.CurrencyID);
            purchase.Purchase.User =
                _register.User.GetFirstOrDefault(u => u.ID == purchase.Purchase.IDOfUser);
            if (User.IsInRole(SD.Role_Employee) || User.IsInRole(SD.Role_Admin))
            {
                var user = HttpContext.User;
                var userId = user.FindFirstValue("UserID");
                purchase.Purchase.IDOfEmployee = int.Parse(userId);
                purchase.Purchase.UserEmployee = _register.User.GetFirstOrDefault(u => 
                u.ID == purchase.Purchase.IDOfEmployee);
            }
            return View(purchase);
        }

        [HttpPost]
        public IActionResult CanclePurchase(PurchaseVM p) 
        {
            Purchase purchase = _register.Purchase.GetFirstOrDefault(u => u.ID == p.Purchase.ID);
            _register.Purchase.Delete(purchase);
            _register.Save();
            TempData["success"] = "Замовлення було видалено успішно!";
            return RedirectToAction("Index", "Purchase");
        }

		[HttpPost]
		public IActionResult ConfirmData(PurchaseVM p) 
        {
			Purchase purchase = _register.Purchase.GetFirstOrDefault(u => u.ID == p.Purchase.ID);
            if(p.Purchase.CurrencyID != purchase.CurrencyID) 
            {
                purchase.CurrencyID = p.Purchase.CurrencyID;
                
			}
            purchase.InfoAboutCurrency = _register.CurrencyInfo
                    .GetFirstOrDefault(u => u.ID == p.Purchase.CurrencyID);
            if (p.Purchase.SumOfCurrency != purchase.SumOfCurrency) 
            {
                purchase.SumOfCurrency = p.Purchase.SumOfCurrency;
                purchase.SumInUAH = _register.Purchase.CountSumInUAH(purchase.SumOfCurrency, 
                    Purchase.PDVPercent, purchase.InfoAboutCurrency.AskedCoursePriceTo);
			}
            purchase.State = SD.State_InProces;
            _register.Purchase.Update(purchase);
            _register.Save();
			return RedirectToAction("Details", "Purchase", new { ID = p.Purchase.ID });
		}


		[HttpPost]
        public IActionResult InProces(PurchaseVM p) 
        {
            Purchase purchase = _register.Purchase.GetFirstOrDefault(u => u.ID == p.Purchase.ID);

            purchase.DepositedMoney = p.Purchase.DepositedMoney;
            purchase.MoneyToReturn = _register.Purchase
                .CountMoneyToReturn((double)purchase.DepositedMoney, purchase.SumInUAH);
            purchase.State = SD.State_Completed;

            InfoAboutCurrency IAC = _register.CurrencyInfo.GetFirstOrDefault(u =>
            u.ID == purchase.CurrencyID);
            IAC.AvailOfAskedCourse -= (int)purchase.SumOfCurrency;

            _register.Purchase.Update(purchase);
			_register.CurrencyInfo.Update(IAC);

			_register.Save();
            TempData["success"] = "Операція зафіксована успішно!";
			return RedirectToAction("Details", "Purchase", new { ID = p.Purchase.ID });
		}
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<Purchase> ObjectsFromDb;

            if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
            {
                ObjectsFromDb = _register.Purchase
                .GetAll(includeProperties: "User").ToList();
            }
            else
            {
				var user = HttpContext.User;
				var userId = user.FindFirstValue("UserID");

				ObjectsFromDb = _register.Purchase.GetAll(u =>
                u.IDOfUser.ToString() == userId, includeProperties: "User");
			}

            switch (status)
            {

                case "Створено":
                    ObjectsFromDb = ObjectsFromDb.Where(u => u.State
                        ==SD.State_Created);
                    break;

                case "В процесі":
                    ObjectsFromDb = ObjectsFromDb.Where(u => u.State
                        == SD.State_InProces);
                    break;

                case "Завершено":
                    ObjectsFromDb = ObjectsFromDb.Where(u => u.State
                        == SD.State_Completed);
                    break;

                default:
                    break;
            }

            return Json(new { data = ObjectsFromDb });
        }
        #endregion
    }
}
