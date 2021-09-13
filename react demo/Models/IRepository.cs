using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace react_demo.Models
{
    public interface IRepository<TEntity> where TEntity: class
    {
        Task Add(TEntity entity);
        void Remove(TEntity entity);
        Task<TEntity> Get(long id);
        Task<IEnumerable<TEntity>> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
    }
}