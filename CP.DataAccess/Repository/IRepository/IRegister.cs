namespace CP.DataAccess.Repository.IRepository
{
    public interface IRegister
    {
        IPurchaseRepository Purchase {  get; }
        IInfoAboutCurrencyRepository CurrencyInfo {  get; }
        IUserRepository User { get; }
        IPaymentsRepository Payments { get; }
        void Save();
    }
}
