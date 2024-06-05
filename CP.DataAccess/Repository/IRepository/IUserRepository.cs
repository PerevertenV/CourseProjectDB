using CP.Models;


namespace CP.DataAccess.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        string PasswordHashCoder(string password);
        public string DecryptString(string encryptedText);
        void Update(User obj);
    }
}
