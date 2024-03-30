using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP.DataAccess.Repository.IRepository
{
    internal interface IRegisterRepository
    {
        IPurchaseRepository Purchase {  get; }
        IInfoAboutCurrencyRepository CurrencyInfo {  get; }
        IUserRepository User { get; }
        void Save();
    }
}
