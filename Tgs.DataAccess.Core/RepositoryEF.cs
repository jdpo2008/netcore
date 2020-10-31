using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tgs.DataAccess.Core.Context;

namespace Tgs.DataAccess.Core
{
    public class RepositoryEF<TEntity> : IRepositoryEF<TEntity> where TEntity : class
    {

        private readonly IUnitOfWork _unitOfWork;

        public RepositoryEF(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Add(TEntity item)
        {
            if (item != (TEntity)null)
                _unitOfWork.Context.Set<TEntity>().Add(item); 
        }

        public void Update(TEntity item)
        {
            _unitOfWork.Context.Entry(item).State = EntityState.Modified;
            _unitOfWork.Context.Set<TEntity>().Attach(item);
        }

        public void Remove(TEntity item)
        {
            if (item != (TEntity)null)
                _unitOfWork.Context.Set<TEntity>().Remove(item);
        }

        public TEntity Get(params object[] key)
        {
            if (key != (TEntity)null)
                return _unitOfWork.Context.Set<TEntity>().Find(key);
            else
                return null;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _unitOfWork.Context.Set<TEntity>().AsEnumerable<TEntity>();
        }

        public IEnumerable<TEntity> GetAllAsNoTracking()
        {
            return _unitOfWork.Context.Set<TEntity>().AsNoTracking();
        }

        public IQueryable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> expresion)
        {
            return _unitOfWork.Context.Set<TEntity>().Where(expresion);
        }

        public IQueryable<TEntity> GetFilteredAsNoTracking(Expression<Func<TEntity, bool>> expresion)
        {
            return _unitOfWork.Context.Set<TEntity>().Where(expresion).AsNoTracking<TEntity>();
        }
    }
}
