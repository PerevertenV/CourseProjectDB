using CP.Models;


namespace CP.DataAccess.Repository.IRepository
{
    public interface IInfoAboutCurrencyRepository : IRepository<InfoAboutCurrency>
    {
        void Update(InfoAboutCurrency obj);
    }
}
