using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
