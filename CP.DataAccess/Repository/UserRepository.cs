using CP.DataAccess.Data;
using CP.DataAccess.Repository.IRepository;
using CP.Models.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CP.DataAccess.Repository
{
    public class UserRepository : IUserRepository
    {
        private ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public string PasswordHashCoder(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Хешуємо пароль з використанням солі і встановлюємо параметри для похідного ключа
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // Повертаємо результат у форматі "сіль:хеш"
            return $"{Convert.ToBase64String(salt)}:{hashed}";
        }

        public void Update(User obj)
        {
            _db.Users.Update(obj);
        }
    }
}
