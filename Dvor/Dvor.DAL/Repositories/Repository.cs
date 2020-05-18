using Dvor.Common.Enums;
using Dvor.Common.Interfaces;
using Dvor.DAL.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Dvor.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DvorContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(DvorContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> GetAll(
            TrackingState trackingState = TrackingState.Enabled,
            params string[] includes)
        {
            var result = trackingState == TrackingState.Enabled ? _dbSet : _dbSet.AsNoTracking();
            result = includes.Aggregate(result, (current, include) => current.Include(include));

            return result;
        }

        public IQueryable<T> GetMany(Expression<Func<T, bool>> filter,
            Expression<Func<T, object>> orderBy = null,
            TrackingState trackingState = TrackingState.Enabled,
            params string[] includes)
        {
            IQueryable<T> result = includes.Aggregate<string, IQueryable<T>>(_dbSet, (current, include) => current.Include(include));
            result = trackingState == TrackingState.Enabled ? result.Where(filter) : result.AsNoTracking().Where(filter);

            if (orderBy != null)
            {
                result = result.OrderBy(orderBy);
            }

            return result;
        }

        public T Get(Expression<Func<T, bool>> condition,
            TrackingState trackingState = TrackingState.Enabled,
            params string[] includes)
        {
            IQueryable<T> result = includes.Aggregate<string, IQueryable<T>>(_dbSet, (current, include) => current.Include(include));

            return trackingState == TrackingState.Enabled
                ? result.FirstOrDefault(condition)
                : result.AsNoTracking().FirstOrDefault(condition);
        }

        public void Create(T item)
        {
            _dbSet.Add(item);
        }

        public void Update(T item)
        {
            _context.Update(item);
        }

        public void Delete(string id)
        {
            var item = _dbSet.Find(id);
            _dbSet.Remove(item);
        }

        public bool IsExist(Expression<Func<T, bool>> condition)
        {
            return _dbSet.Any(condition);
        }
    }
}