using CP.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP.DataAccess.Repository.IRepository
{
    internal interface IUserRepository
    {
        string PasswordHashCoder(string password);
        void Update(User obj);
    }
}
