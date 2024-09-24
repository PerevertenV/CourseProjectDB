using CP.Models;


namespace CP.DataAccess.Repository.IRepository
{
    public interface IPurchaseRepository : IRepository<Purchase>
	{
		
		void Update(Purchase obj);
	}
}
