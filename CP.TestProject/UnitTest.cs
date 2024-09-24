using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Threading.Tasks;
using CP.DataAccess.Services.IServices;
using Moq;
using CP.DataAccess.ServicesBL;

namespace CP.Utility.UnitTests
{
	[TestFixture]
	public class UnitTest
	{
		private PurchaseService ps;

		[SetUp]
		public void Setup() 
		{
			ps = new PurchaseService();
		}

		[Test]
		public void Check_CountMoneyToReturn_Method_Value()
		{
			double depositedMoney = 10.1520;
			double neededMoney = 9.0020;

			double result = ps.CountMoneyToReturn(depositedMoney, neededMoney);

			Assert.That(result, Is.EqualTo(1.15).Within(0.01),
				"The returned value should be 1.15");
			TestContext.WriteLine("Test \"Check_CountMoneyToReturn_Method_Value\" " +
				"completed successfully");
		}
		[Test]
		public void Check_CountMoneyToReturn_Method_Bool()
		{
			double neededMoney = 10.1520;
			double depositedMoney = 9.0020;

			double result = ps.CountMoneyToReturn(depositedMoney, neededMoney);

			Assert.That(result, Is.EqualTo(0),
				"The returned value should be 0");
			TestContext.WriteLine("Test \"Check_CountMoneyToReturn_Method_Bool\" " +
				"completed successfully");
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
