using DDD.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DDD.Domain.Base
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        IUnitOfWork UnitOfWork { get; }

        void Add(TEntity item);

        void AddRange(IEnumerable<TEntity> items);

        void Remove(TEntity item);

        void RemoveRange(IEnumerable<TEntity> items);

        void Modify(TEntity item);

        void TrackItem(TEntity item);

        void Merge(TEntity persisted, TEntity current);

        void Commit();

        TEntity Get(Int32 id);
        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> AllMatching(ISpecification<TEntity> specification);

        IEnumerable<TEntity> AllMatching<KProperty>(Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending);

        IEnumerable<TEntity> AllMatching<KProperty>(ISpecification<TEntity> specification, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending);

        IEnumerable<TEntity> GetPaged<KProperty>(int pageIndex, int pageCount, ISpecification<TEntity> specification, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending);

        IEnumerable<TEntity> GetPaged(int pageIndex, int pageCount, ISpecification<TEntity> specification, string fieldSort, bool ascending);

        IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter);

        long Count(ISpecification<TEntity> specification);
    }
}
