using System.Threading.Tasks;

namespace Faucet.API.Data
{
    public interface IUnitOfWork
    {    
        Task Commit();
    }

    internal class UnitOfWork : IUnitOfWork
    {
        private readonly FaucetDbContext _context;

        public UnitOfWork(FaucetDbContext context)
        {
            _context = context;
        }

        public Task Commit()
        {
            return _context.SaveChangesAsync();
        }
    }
}
