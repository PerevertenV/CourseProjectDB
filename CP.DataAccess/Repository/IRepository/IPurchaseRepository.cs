using CP.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP.DataAccess.Repository.IRepository
{
	public interface IPurchaseRepository : IRepository<Purchase>
	{
		double CountSumInUAH(double SumToChange, double PDVpercent, double Price);
		double CountMoneyToReturn(double DepositedMoney, double NeededMoney);
		void Update(Purchase obj);
	}
}
