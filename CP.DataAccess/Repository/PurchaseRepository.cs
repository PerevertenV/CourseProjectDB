using CP.DataAccess.Data;
using CP.DataAccess.Repository.IRepository;
using CP.Models;

namespace CP.DataAccess.Repository
{
    public class PurchaseRepository : Repository<Purchase>, IPurchaseRepository
    {
        private ApplicationDbContext _db;
        public PurchaseRepository(ApplicationDbContext? db) : base(db)
        {
            _db = db;
        }

        public void Update(Purchase obj)
        {
            _db.Purchase.Update(obj);
        }
    }
}
