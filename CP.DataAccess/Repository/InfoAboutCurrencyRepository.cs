using CP.DataAccess.Data;
using CP.DataAccess.Repository.IRepository;
using CP.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP.DataAccess.Repository
{
    public class InfoAboutCurrencyRepository : Repository<InfoAboutCurrency>, IInfoAboutCurrencyRepository
    {
        private ApplicationDbContext _db;
        public InfoAboutCurrencyRepository(ApplicationDbContext? db) : base(db)
        {
            _db = db;
        }

        public void Update(InfoAboutCurrency obj) 
        {
            _db.InfoAboutCurrency.Update(obj);
        }
    }
}
