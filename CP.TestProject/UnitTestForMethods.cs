using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Threading.Tasks;
using CP.DataAccess.Services.IServices;
using Moq;
using CP.DataAccess.ServicesBL;
using CP.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using CourseProjectDB.Areas.Customer.Controllers;
using CP.Models.VModels;
using CP.Models;
using System.ComponentModel.DataAnnotations;

namespace CP.Utility.UnitTests
{
	[TestFixture]
	public class UnitTestForMethods
	{
		private PurchaseService ps;

		[SetUp]
		public void Setup() 
		{
			ps = new PurchaseService();
            
		}

		[Test]
		[TestCase(10.1520, 9.0020, ExpectedResult = 1.15)]
		[TestCase(9.0020, 10.1520, ExpectedResult = 0)]
		public double Check_CountMoneyToReturn_Method(double DM, double NM)
		{
			double depositedMoney = DM;
			double neededMoney = NM;

			double result = ps.CountMoneyToReturn(depositedMoney, neededMoney);

			return result;
		}

		[Test]
		public void Check_CountSumInUAH_Method_Value()
		{
			double sumToChange = 300;
			double pdvPercent = 0.2;
			double price = 42.5;

			double result = ps.CountSumInUAH(sumToChange, pdvPercent, price);

			Assert.That(result, Is.EqualTo(15300).Within(0.01),
				"The returned value should be 15300");

			TestContext.WriteLine("Test \"Check_CountSumInUAH_Method_Value\" " +
				"completed successfully");
		}

		[Test]
		[TestCase(350.00)]
		[TestCase(0.00)]
		public void Check_AddingCurrency_Price(double price) 
		{
			InfoAboutCurrency Curr = new InfoAboutCurrency()
			{
				Name = "13",
				AskedCoursePriceTo = price,
				AvailOfAskedCourse = 300
			};

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(Curr);

            var isValid = Validator.TryValidateObject(Curr, 
				validationContext, validationResults, true);

			Assert.That(isValid, Is.False);
            Assert.That(validationResults, Is.Not.Empty);
            Assert.That(validationResults[0].ErrorMessage, 
				Is.EqualTo("Ціна не може бути більше 300 грн за одиницю та дорівнювати 0"));
        }
		[Test]
		[TestCase(10001)]
		[TestCase(0)]
		public void Check_AddingCurrency_Capacity(int capacity) 
		{
			InfoAboutCurrency Curr = new InfoAboutCurrency()
			{
				Name = "13",
				AskedCoursePriceTo = 20,
				AvailOfAskedCourse = capacity
            };

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(Curr);

            var isValid = Validator.TryValidateObject(Curr, 
				validationContext, validationResults, true);

			Assert.That(isValid, Is.False);
            Assert.That(validationResults, Is.Not.Empty);
            Assert.That(validationResults[0].ErrorMessage, 
				Is.EqualTo("Доступна кiлькiсть не може перевищувати 10000 та дорівнювати 0"));
        }

		[Test]
		[TestCase(100001)]
		[TestCase(0)]
		public void Check_PurchaseInputField_DepositedMoneyN(int DM) 
		{
			Purchase purch = new Purchase()
			{
                CurrencyID = 1,
				State = "3",
                DepositedMoney = DM,
                SumOfCurrency = 100,
            };

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(purch);

            var isValid = Validator.TryValidateObject(purch, 
				validationContext, validationResults, true);

			Assert.That(isValid, Is.False);
            Assert.That(validationResults, Is.Not.Empty);
        }
		[Test]
		[TestCase(5001)]
		[TestCase(0)]
		public void Check_PurchaseInputField_SumOfCurrencyN(int SOC) 
		{
			Purchase purch = new Purchase()
			{
                CurrencyID = 1,
				State = "3",
                DepositedMoney = 100,
                SumOfCurrency = SOC,
            };

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(purch);

            var isValid = Validator.TryValidateObject(purch, 
				validationContext, validationResults, true);

			Assert.That(isValid, Is.False);
            Assert.That(validationResults, Is.Not.Empty);
        }
		
		[Test]
		[TestCase(100000)]
		[TestCase(1)]
		public void Check_PurchaseInputField_DepositedMoneyP(int DM) 
		{
			Purchase purch = new Purchase()
			{
                CurrencyID = 1,
				State = "3",
                DepositedMoney = DM,
                SumOfCurrency = 100,
            };

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(purch);

            var isValid = Validator.TryValidateObject(purch, 
				validationContext, validationResults, true);

			Assert.That(isValid, Is.True);
            Assert.That(validationResults, Is.Empty);
        }
		[Test]
		[TestCase(5000)]
		[TestCase(1)]
		public void Check_PurchaseInputField_SumOfCurrencyP(int SOC) 
		{
			Purchase purch = new Purchase()
			{
                CurrencyID = 1,
				State = "3",
                DepositedMoney = 100,
                SumOfCurrency = SOC,
            };

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(purch);

            var isValid = Validator.TryValidateObject(purch, 
				validationContext, validationResults, true);

			Assert.That(isValid, Is.True);
            Assert.That(validationResults, Is.Empty);
        }
	}
}
