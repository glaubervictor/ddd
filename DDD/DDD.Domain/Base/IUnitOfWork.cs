using System;

namespace DDD.Domain.Base
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        void CommitAndRefreshChanges();
        void RollbackChanges();
    }
}
