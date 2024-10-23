using CourseProjectDB.Areas.Customer.Controllers;
using CP.DataAccess.Repository.IRepository;
using CP.DataAccess.Services.IServices;
using CP.Models.VModels;
using CP.Models;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Infrastructure;
using System.Security.Claims;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using CourseProjectDB.Areas.Admin.Controllers;
using System.ComponentModel.DataAnnotations;

namespace CP.TestProject
{
	[TestFixture]
	public class UnitTestsForControllers
	{
		private Mock<IRegister> _mockRegister;
		private Mock<IServiceBL> _mockService;
		private PurchaseController _controllerPurchase;
		private CurrencyController _controllerCurrency;

        [SetUp]
		public void Setup()
		{
			_mockRegister = new Mock<IRegister>();
			_mockService = new Mock<IServiceBL>();

			_controllerPurchase = new PurchaseController(_mockRegister.Object, _mockService.Object);
			_controllerCurrency = new CurrencyController(_mockRegister.Object);
		}

		[Test]
		[TestCase(80, 100, ExpectedResult = 80)]
		[TestCase(150, 100, ExpectedResult = 100)]
		public double UpdatePurchaseSum(int PVMSum, int IACSum)
		{
			var purchaseVM = new PurchaseVM
			{
				Purchase = new Purchase
				{
					CurrencyID = 1,
					SumOfCurrency = PVMSum // Використовуємо значення з параметрів
				}
			};

			var infoAboutCurrency = new InfoAboutCurrency
			{
				ID = 1,
				AvailOfAskedCourse = IACSum // Використовуємо значення з параметрів
			};

			// Налаштування моків
			_mockRegister.Setup(r => r.CurrencyInfo.GetFirstOrDefault(It.IsAny<Expression<Func<InfoAboutCurrency, bool>>>(), null, false))
				.Returns(infoAboutCurrency);

			_mockService.Setup(s => s.Purchase.CountSumInUAH(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>()))
				.Returns(0);

			// Змінна для зберігання доданого об'єкта
			Purchase addedPurchase = null;

			var mockPurchaseRepo = new Mock<IPurchaseRepository>();
			mockPurchaseRepo.Setup(r => r.Add(It.IsAny<Purchase>()))
				.Callback<Purchase>(purchase => addedPurchase = purchase); // Зберігаємо об'єкт у змінній
			mockPurchaseRepo.Setup(r => r.Update(It.IsAny<Purchase>())).Verifiable();
			_mockRegister.Setup(r => r.Purchase).Returns(mockPurchaseRepo.Object);

			var tempDataProviderMock = new Mock<ITempDataProvider>();
			var tempData = new TempDataDictionary(new DefaultHttpContext(), tempDataProviderMock.Object);

			// Додаємо TempData до контролера
			_controllerPurchase.TempData = tempData;

			_controllerPurchase.ControllerContext = new ControllerContext
			{
				HttpContext = new DefaultHttpContext
				{
					User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "TestUser") }))
				}
			};

			// Act
			var result = _controllerPurchase.Create(purchaseVM) as RedirectToActionResult;

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(addedPurchase, Is.Not.Null); // Перевіряємо, що об'єкт був доданий
			Assert.That(addedPurchase.SumOfCurrency, Is.EqualTo(Math.Min(PVMSum, IACSum))); // Перевіряємо значення
			return addedPurchase.SumOfCurrency; // Повертаємо оновлену суму
		}

		[Test]
		public void CanclePurchase()
		{
			var purchaseId = 1;
			var purchaseVM = new PurchaseVM
			{
				Purchase = new Purchase
				{
					ID = purchaseId
				}
			};

			var purchaseToDelete = new Purchase
			{
				ID = purchaseId
			};

			_mockRegister.Setup(r => r.Purchase.GetFirstOrDefault(It.IsAny<Expression<Func<Purchase, bool>>>(), null, false))
				.Returns(purchaseToDelete);

			var mockPurchaseRepo = new Mock<IPurchaseRepository>();
			mockPurchaseRepo.Setup(r => r.Delete(It.IsAny<Purchase>())).Verifiable(); // Перевіряємо, що викликається Delete

			_mockRegister.Setup(r => r.Purchase).Returns(mockPurchaseRepo.Object);

			var tempDataProviderMock = new Mock<ITempDataProvider>();
			var tempData = new TempDataDictionary(new DefaultHttpContext(), tempDataProviderMock.Object);
			_controllerPurchase.TempData = tempData;

			var result = _controllerPurchase.CanclePurchase(purchaseVM) as RedirectToActionResult;

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ActionName, Is.EqualTo("Index")); 
			Assert.That(result.ControllerName, Is.EqualTo("Purchase"));

			Assert.That(_controllerPurchase.TempData["success"], Is.EqualTo("Замовлення було скасовано успішно!"));
		}

        [TestCase("USD", "USD")] 
        public void Upsert_ExistingCurrency_ReturnsViewWithModelError(string existingCurrencyName, string newCurrencyName)
        {
           
            var existingCurrency = new InfoAboutCurrency { ID = 1, Name = existingCurrencyName };
            var newCurrency = new InfoAboutCurrency { ID = 0, Name = newCurrencyName }; 

            
            _mockRegister.Setup(r => r.CurrencyInfo.GetAll(It.IsAny<Expression<Func<InfoAboutCurrency, bool>>>(), null))
				.Returns(new List<InfoAboutCurrency> { existingCurrency });

            
            var result = _controllerCurrency.Upsert(newCurrency) as ViewResult;

           
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Model, Is.EqualTo(newCurrency));
            Assert.That(_controllerCurrency.ModelState.IsValid, Is.False);
            Assert.That(_controllerCurrency.ModelState.ContainsKey("name"), Is.True);
            Assert.That(_controllerCurrency.ModelState["name"].Errors[0].ErrorMessage, Is.EqualTo("Валюта із таким кодом уже існує"));
        }

		[Test]
		public void FullTest_ForAddingCurrency() 
		{
            var currencyForSending = new InfoAboutCurrency 
			{	
				ID = 0, 
				Name = "13",
                AskedCoursePriceTo = 35,
                AvailOfAskedCourse = 300
            };

            _mockRegister.Setup(r => r.CurrencyInfo.GetAll(It.IsAny<Expression<Func<InfoAboutCurrency, bool>>>(), null))
               .Returns(new List<InfoAboutCurrency>());

            var tempDataProviderMock = new Mock<ITempDataProvider>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), tempDataProviderMock.Object);
            _controllerCurrency.TempData = tempData;
            
			_mockRegister.Setup(r => r.Save());

            _mockRegister.Setup(r => r.CurrencyInfo.Add(It.IsAny<InfoAboutCurrency>()));

            var result = _controllerCurrency.Upsert(currencyForSending) as ViewResult;

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(currencyForSending);

            var isValid = Validator.TryValidateObject(currencyForSending,
                validationContext, validationResults, true);

            Assert.That(isValid, Is.True);
            Assert.That(validationResults, Is.Empty);
            Assert.That(result, Is.Null);
            Assert.That(tempData["success"], Is.EqualTo("Валюту було додано успішно!"));

            // Перевіряємо, що метод Add був викликаний
            _mockRegister.Verify(r => r.CurrencyInfo.Add(currencyForSending), Times.Once);
            // Перевіряємо, що метод Save був викликаний
            _mockRegister.Verify(r => r.Save(), Times.Once);

        }

        // Звільнення від мок-об'єктів
        [TearDown]
		public void TearDown()
		{
			_controllerPurchase?.Dispose();
			_controllerCurrency?.Dispose();
            _mockRegister = null;
			_mockService = null;
			_controllerPurchase = null;
            _controllerCurrency = null;

		}
	}
}
