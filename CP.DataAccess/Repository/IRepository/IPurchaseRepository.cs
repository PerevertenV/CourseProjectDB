using CP.Models;


namespace CP.DataAccess.Repository.IRepository
{
    public interface IPurchaseRepository : IRepository<Purchase>
	{
		double CountSumInUAH(double SumToChange, double PDVpercent, double Price);
		double CountMoneyToReturn(double DepositedMoney, double NeededMoney);
		void Update(Purchase obj);
	}
}
