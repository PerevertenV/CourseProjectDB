using CP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP.DataAccess.Repository.IRepository
{
    public interface IInfoAboutCurrencyRepository : IRepository<InfoAboutCurrency>
    {
        void Update(InfoAboutCurrency obj);
    }
}
