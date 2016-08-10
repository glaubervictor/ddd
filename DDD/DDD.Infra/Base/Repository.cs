using DDD.Domain.Base;
using DDD.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace DDD.Infra.Base
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        #region Members

        IQueryableUnitOfWork _UnitOfWork;
        IDbSet<TEntity> _Set;

        #endregion

        #region Constructor

        public Repository(IQueryableUnitOfWork unitOfWork, IDbSet<TEntity> set)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            _UnitOfWork = unitOfWork;
            _Set = set;
        }

        #endregion

        #region IRepository

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _UnitOfWork;
            }
        }

        public virtual void Add(TEntity item)
        {
            if (item != null)
                _Set.Add(item); // add new item in this set
            else
            {
                //LoggerFactory.CreateLog()
                //          .LogInfo(Messages.info_CannotAddNullEntity, typeof(TEntity).ToString());
            }
        }

        public virtual void AddRange(IEnumerable<TEntity> items)
        {
            if (items != null)
            {

                var newSet = _UnitOfWork.CreateSet<TEntity>();
                newSet.AddRange(items);
            }
        }

        public virtual void Remove(TEntity item)
        {
            if (item != null)
            {
                //attach item if not exist
                _UnitOfWork.Attach(item);

                //set as "removed"
                _Set.Remove(item);
            }
            else
            {
                // LoggerFactory.CreateLog()
                //           .LogInfo(Messages.info_CannotRemoveNullEntity, typeof(TEntity).ToString());
            }
        }

        public virtual void RemoveRange(IEnumerable<TEntity> items)
        {
            if (items != (IEnumerable<TEntity>)null)
            {
                //_UnitOfWork.Attach(items);
                var newSet = _UnitOfWork.CreateSet<TEntity>();
                newSet.RemoveRange(items);
            }
        }

        public virtual void TrackItem(TEntity item)
        {
            if (item != null)
                _UnitOfWork.Attach<TEntity>(item);
            else
            {
                // LoggerFactory.CreateLog()
                //          .LogInfo(Messages.info_CannotRemoveNullEntity, typeof(TEntity).ToString());
            }
        }

        public virtual void Modify(TEntity item)
        {
            if (item != null)
                _UnitOfWork.SetModified(item);
            else
            {
                //LoggerFactory.CreateLog()
                //          .LogInfo(Messages.info_CannotRemoveNullEntity, typeof(TEntity).ToString());
            }
        }

        public virtual TEntity Get(int id)
        {
            if (id > 0)
                return _Set.Find(id);
            else
                return null;
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _Set.AsEnumerable();
        }

        public virtual IEnumerable<TEntity> AllMatching(ISpecification<TEntity> specification)
        {
            return _Set.Where(specification.SatisfiedBy())
                       .AsEnumerable();
        }

        public virtual IEnumerable<TEntity> AllMatching<KProperty>(ISpecification<TEntity> specification, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending)
        {
            if (ascending)
            {
                return _Set.Where(specification.SatisfiedBy())
                           .OrderBy(orderByExpression)
                           .AsEnumerable();
            }
            else
            {
                return _Set.Where(specification.SatisfiedBy())
                           .OrderByDescending(orderByExpression)
                           .AsEnumerable();
            }
        }

        public virtual IEnumerable<TEntity> AllMatching<KProperty>(Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending)
        {
            if (ascending)
            {
                return _Set.OrderBy(orderByExpression)
                           .AsEnumerable();
            }
            else
            {
                return _Set.OrderByDescending(orderByExpression)
                           .AsEnumerable();
            }
        }

        public virtual IEnumerable<TEntity> GetPaged<KProperty>(int pageIndex, int pageCount, ISpecification<TEntity> specification, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending)
        {
            if (ascending)
            {
                return _Set.Where(specification.SatisfiedBy())
                           .OrderBy(orderByExpression)
                           .Skip(pageCount * (pageIndex - 1))
                           .Take(pageCount)
                           .AsEnumerable();
            }
            else
            {
                return _Set.Where(specification.SatisfiedBy())
                           .OrderByDescending(orderByExpression)
                           .Skip(pageCount * (pageIndex - 1))
                           .Take(pageCount)
                           .AsEnumerable();
            }
        }

        public virtual IEnumerable<TEntity> GetPaged(int pageIndex, int pageCount, ISpecification<TEntity> specification, string fieldSort, bool ascending)
        {
            if (string.IsNullOrWhiteSpace(fieldSort))
                fieldSort = "Id";

            //Type t = typeof(TEntity);

            /*if (!t.GetProperties().Select(c => c.Name).Contains(fieldSort))
                fieldSort = "Id";*/

            var param = Expression.Parameter(typeof(TEntity), "c");
            MemberExpression property = null;

            string[] fieldNames = fieldSort.Contains(".") ? fieldSort.Split('.') : Regex.Split(fieldSort, @"(?<!^)(?=[A-Z])");
            foreach (string filed in fieldNames)
            {
                if (property == null)
                {
                    property = Expression.Property(param, filed);
                }
                else
                {
                    property = Expression.Property(property, filed);
                }
            }
            
            //Expression conversion = Expression.Convert(Expression.Property(param, fieldSort), typeof(object));
            Expression conversion = Expression.Convert(property, typeof(object));
            var sortExpression = Expression.Lambda<Func<TEntity, object>>(conversion, param).Compile();

            if (ascending)
            {
                return _Set
                           .Where(specification.SatisfiedBy())
                           .OrderBy(sortExpression)
                           .Skip(pageCount * (pageIndex - 1))
                           .Take(pageCount)
                           .AsEnumerable();
            }
            else
            {
                return _Set
                           .Where(specification.SatisfiedBy())
                           .OrderByDescending(sortExpression)
                           .Skip(pageCount * (pageIndex - 1))
                           .Take(pageCount)
                           .AsEnumerable();
            }
        }

        public virtual IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter)
        {
            return _Set.Where(filter)
                       .AsEnumerable();
        }

        public virtual void Merge(TEntity persisted, TEntity current)
        {
            _UnitOfWork.ApplyCurrentValues(persisted, current);
        }

        public virtual void Commit()
        {
            _UnitOfWork.Commit();
        }

        public virtual void CommitAndRefreshChanges()
        {
            _UnitOfWork.CommitAndRefreshChanges();
        }

        public virtual void RollbackChanges()
        {
            _UnitOfWork.RollbackChanges();
        }

        public virtual long Count(ISpecification<TEntity> specification)
        {
            return _Set.Where(specification.SatisfiedBy()).Count();
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            if (_UnitOfWork != null)
                _UnitOfWork.Dispose();
        }

        #endregion

        #region Private

        IDbSet<TEntity> GetSet()
        {
            return _UnitOfWork.CreateSet<TEntity>();
        }

        #endregion
    }
}