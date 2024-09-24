using CP.DataAccess.Data;
using CP.DataAccess.Repository.IRepository;
using CP.Models;
using System.Security.Cryptography;
using System.Text;

namespace CP.DataAccess.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(User obj)
        {
            _db.Users.Update(obj);
        }
    }
}
