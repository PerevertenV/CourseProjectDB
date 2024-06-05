using CP.DataAccess.Data;
using CP.DataAccess.Repository.IRepository;

namespace CP.DataAccess.Repository 
{
    public class Register : IRegister
    {
        private ApplicationDbContext _db;
        public IPurchaseRepository Purchase {  get; private set; }
        public IInfoAboutCurrencyRepository CurrencyInfo {  get; private set; }
        public IUserRepository User {  get; private set; }
        public IPaymentsRepository Payments {  get; private set; }
        public Register(ApplicationDbContext db)
        {
            _db = db;
            Purchase = new PurchaseRepository(_db);
            CurrencyInfo = new InfoAboutCurrencyRepository(_db);
            User = new UserRepository(_db);
            Payments = new PaymentsRepository(_db);

        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
