using CP.DataAccess.Data;
using CP.DataAccess.Repository.IRepository;
using CP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP.DataAccess.Repository
{
    public class PaymentsRepository : Repository<Payments>, IPaymentsRepository
    {
        private ApplicationDbContext _db;
        public PaymentsRepository(ApplicationDbContext? db) : base(db)
        {
            _db = db;
        }

        public void Update(Payments obj) 
        {
            _db.Payments.Update(obj);
        }
    }
}
