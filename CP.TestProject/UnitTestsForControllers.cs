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

namespace CP.TestProject
{
	[TestFixture]
	public class UnitTestsForControllers
	{
		private Mock<IRegister> _mockRegister;
		private Mock<IServiceBL> _mockService;
		private PurchaseController _controller;

		[SetUp]
		public void Setup()
		{
			_mockRegister = new Mock<IRegister>();
			_mockService = new Mock<IServiceBL>();

			_controller = new PurchaseController(_mockRegister.Object, _mockService.Object);
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
			_controller.TempData = tempData;

			_controller.ControllerContext = new ControllerContext
			{
				HttpContext = new DefaultHttpContext
				{
					User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "TestUser") }))
				}
			};

			// Act
			var result = _controller.Create(purchaseVM) as RedirectToActionResult;

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
			_controller.TempData = tempData;

			var result = _controller.CanclePurchase(purchaseVM) as RedirectToActionResult;

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ActionName, Is.EqualTo("Index")); 
			Assert.That(result.ControllerName, Is.EqualTo("Purchase"));

			Assert.That(_controller.TempData["success"], Is.EqualTo("Замовлення було скасовано успішно!"));
		}



		// Звільнення від мок-об'єктів
		[TearDown]
		public void TearDown()
		{
			_controller?.Dispose();
			_mockRegister = null;
			_mockService = null;
			_controller = null;
		}
	}
}
