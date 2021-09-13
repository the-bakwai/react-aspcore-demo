using System;
using System.Threading.Tasks;

namespace react_demo.Models
{
    public interface IUnitOfWork: IDisposable
    {
        ITodoRepository Todos { get; }
        IUserRepository Users { get; }
        Task<int> SaveChanges();
    }
}