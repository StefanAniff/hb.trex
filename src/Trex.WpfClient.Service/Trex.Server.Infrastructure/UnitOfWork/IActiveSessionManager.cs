using NHibernate;

namespace Trex.Server.Infrastructure.UnitOfWork
{
    public interface IActiveSessionManager : IManageSessions
    {
        ISession OpenSession();
        void ClearActiveSession();
        void Commit();
        void Close();
        bool HasActiveSession { get; }
        void ErrorHappened();
        void BeginTransaction();
    }
    public interface IManageSessions
    {
        ISession GetActiveSession();
    }
}