using System;
using System.Data;
using NHibernate;

namespace Trex.Server.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Clear();
        void Flush();
        ITransaction CreateTransaction();
        ITransaction CreateTransaction(IsolationLevel isolationLevel);
        void RollBack();
        ITransaction CreatReadonlyTransaction();
    }
}
