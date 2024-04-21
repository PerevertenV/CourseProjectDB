using CP.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP.DataAccess.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        string PasswordHashCoder(string password);
        public string DecryptString(string encryptedText);
        void Update(User obj);
    }
}
