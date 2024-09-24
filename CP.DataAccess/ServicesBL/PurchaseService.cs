using CP.DataAccess.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP.DataAccess.ServicesBL
{
	public class PurchaseService: IPurchaseService
	{
		public double CountMoneyToReturn(double DepositedMoney, double NeededMoney)
		{
			if (DepositedMoney < NeededMoney) return 0;
			else return Math.Round(DepositedMoney - NeededMoney, 2);
		}

		public double CountSumInUAH(double SumToChange, double PDVpercent, double Price)
		{
			return Math.Round(SumToChange * Price + SumToChange * Price * PDVpercent, 2);
		}
	}
}
