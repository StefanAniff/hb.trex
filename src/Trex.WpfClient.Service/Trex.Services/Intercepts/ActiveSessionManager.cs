using System;
using System.Data;
using NHibernate;
using Trex.Server.Infrastructure;
using Trex.Server.Infrastructure.State;
using Trex.Server.Infrastructure.UnitOfWork;

namespace TrexSL.Web.Intercepts
{
    public class RequestResponseSessionManager : IActiveSessionManager
    {
        private const string sessionKey = "_currentSession";
        private readonly IRequestState _requestState;
        private readonly INHibernateSessionFactory _sessionFactory;
        private bool _error;

        public RequestResponseSessionManager(IRequestState requestState, INHibernateSessionFactory sessionFactory)
        {
            this._requestState = requestState;
            _sessionFactory = sessionFactory;
        }

        public ISession GetActiveSession()
        {
            if (Current == null)
            {
                throw new InvalidOperationException("There is no active ISession instance for this thread");
            }

            return Current;
        }

        public ISession OpenSession()
        {
            if (Current != null)
            {
                throw new InvalidOperationException("There is already an active ISession instance for this thread");
            }

            var cur = _sessionFactory.CreateSession();
            cur.FlushMode = FlushMode.Never; //readonly by default
            Current = cur;
            return cur;
        }

        public void BeginTransaction()
        {
            Current.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void ClearActiveSession()
        {
            Current = null;
        }

        public void Commit()
        {
            Current.Transaction.Commit();
        }

        public virtual void Close()
        {
            try
            {
                DoClose();
            }
            catch (Exception ex)
            {
                // Not much to do.
                //NLog.Logger.Debug("Got exception in Close() (exception was handled): ", ex);
            }
        }

        private void DoClose()
        {
            try
            {
                if (_error)
                {
                    if (Current.Transaction.IsActive) Current.Transaction.Rollback();
                    Current.Clear(); // Evicts objects
                }

                // Evict queries stored in the session-scope 2. level cache
                //_sessionFactory.EvictQueries(SessionConstants.SESSION_SCOPED_2_LEVEL_CACHE);
            }
            finally
            {
                try
                {
                    Current.Close();
                }
                catch (Exception ex)
                {
                    // Not much to do.
                    //_log.Debug("Got exception when closing session (exception was handled): ", ex);
                }
                Cleanup();
            }
        }

        public virtual void ErrorHappened()
        {
            _error = true;
        }

        private void Cleanup()
        {
            // Make ready for next
            _error = false;
            ClearActiveSession();
        }

        public bool HasActiveSession
        {
            get { return Current != null; }
        }

        protected virtual ISession Current
        {
            get
            {
                return _requestState.Get<ISession>(sessionKey);
            }

            set
            {
                _requestState.Store(sessionKey, value);
            }
        }
    }
}