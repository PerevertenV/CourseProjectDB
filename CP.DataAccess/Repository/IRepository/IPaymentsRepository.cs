using CP.Models;

namespace CP.DataAccess.Repository.IRepository
{
    public interface IPaymentsRepository : IRepository<Payments>
    {
        void Update(Payments obj);
    }
}
