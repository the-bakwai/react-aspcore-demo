using System.Threading.Tasks;

namespace react_demo.Models
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly TodoDataContext _context;

        public UnitOfWork(TodoDataContext context)
        {
            _context = context;

            Todos = new TodoRepository(_context);
            Users = new UserRepository(_context);
        }
        
        public ITodoRepository Todos { get; }
        public IUserRepository Users { get; }

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}