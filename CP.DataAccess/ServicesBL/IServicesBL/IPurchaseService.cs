using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP.DataAccess.Services.IServices
{
	public interface IPurchaseService
	{
		public double CountSumInUAH(double SumToChange, double PDVpercent, double Price);
		public double CountMoneyToReturn(double DepositedMoney, double NeededMoney);
	}
}
