using System;
using System.Linq;
using System.Linq.Expressions;
using Dvor.Common.Enums;

namespace Dvor.Common.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetMany(
            Expression<Func<T, bool>> filter,
            Expression<Func<T, object>> orderBy = null,
            TrackingState trackingState = TrackingState.Enabled,
            params string[] includes);

        T Get(
            Expression<Func<T, bool>> condition,
            TrackingState trackingState = TrackingState.Enabled,
            params string[] includes);

        IQueryable<T> GetAll(
            TrackingState trackingState = TrackingState.Enabled,
            params string[] includes);

        void Create(T item);

        void Update(T item);

        void Delete(string id);

        bool IsExist(Expression<Func<T, bool>> condition);
    }
}