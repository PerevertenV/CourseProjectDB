using CP.DataAccess.Data;
using CP.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP.DataAccess.Repository 
{
    public class Register : IRegister
    {
        private ApplicationDbContext _db;
        public IPurchaseRepository Purchase {  get; private set; }
        public IInfoAboutCurrencyRepository CurrencyInfo {  get; private set; }
        public IUserRepository User {  get; private set; }
        public Register(ApplicationDbContext db)
        {
            _db = db;
            Purchase = new PurchaseRepository(_db);
            CurrencyInfo = new InfoAboutCurrencyRepository(_db);
            User = new UserRepository(_db);

        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
