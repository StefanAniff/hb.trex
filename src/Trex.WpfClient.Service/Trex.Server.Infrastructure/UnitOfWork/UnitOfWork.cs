using System;
using System.Data;
using NHibernate;

namespace Trex.Server.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly IActiveSessionManager sessionManager;
        private readonly ISession session;
        private readonly bool isRootUnitOfWork;

        public UnitOfWork(IActiveSessionManager sessionManager)
        {
            this.sessionManager = sessionManager;

            if (sessionManager.HasActiveSession)
            {
                isRootUnitOfWork = false;
                session = sessionManager.GetActiveSession();
            }
            else
            {
                isRootUnitOfWork = true;
                session = sessionManager.OpenSession();
            }
        }

        public void Clear()
        {
            session.Clear();
        }

        public void Flush()
        {
            session.Flush();
        }
        public ITransaction CreatReadonlyTransaction()
        {
            session.FlushMode = FlushMode.Never;
            return CreateTransaction(IsolationLevel.ReadCommitted);
        }

        public ITransaction CreateTransaction()
        {
            session.FlushMode = FlushMode.Auto;
            return CreateTransaction(IsolationLevel.ReadCommitted);
        }

        public ITransaction CreateTransaction(IsolationLevel isolationLevel)
        {
            if (session.Transaction != null && session.Transaction.IsActive)
            {
                throw new InvalidOperationException("nested transactions are not supported!");
            }
            return session.BeginTransaction(isolationLevel);
        }

        public void RollBack()
        {
            if (session.Transaction != null && session.Transaction.IsActive)
                session.Transaction.Rollback();
        }

        public void Dispose()
        {

            if (session != null)
            {
                session.Close();
                session.Dispose();
            }

            sessionManager.ClearActiveSession();

        }
    }
}
