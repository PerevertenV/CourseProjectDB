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

	}
}
