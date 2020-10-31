using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Tgs.DataAccess.Core
{
    interface IRepositoryEF<TEntity> where TEntity : class
    {
        void Add(TEntity item);
        void Update(TEntity item);
        void Remove(TEntity item);
        TEntity Get(params object[] key);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAllAsNoTracking();
        IQueryable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> expresion);
        IQueryable<TEntity> GetFilteredAsNoTracking(Expression<Func<TEntity, bool>> expresion);

    }
}
