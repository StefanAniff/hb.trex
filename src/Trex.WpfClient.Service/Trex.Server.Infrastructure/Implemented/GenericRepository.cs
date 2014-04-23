using System.Collections.Generic;
using System.Linq;
using NHibernate;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.UnitOfWork;

namespace Trex.Server.Infrastructure.Implemented
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        protected readonly IActiveSessionManager ActiveSessionManager;
        protected readonly ISession OpenSession;

        public GenericRepository(ISession openSession)
        {
            OpenSession = openSession;
        }

        public GenericRepository(IActiveSessionManager activeSessionManager)
        {
            ActiveSessionManager = activeSessionManager;
        }

        public TEntity GetById(int id)
        {
            var currentSession = OpenSession ?? ActiveSessionManager.GetActiveSession();

            var returnObject = currentSession.Get<TEntity>(id);
            return returnObject;
        }

        public IList<TEntity> GetAll()
        {   
            ISession currentSession = OpenSession ?? ActiveSessionManager.GetActiveSession();
            return currentSession.CreateCriteria<TEntity>().List<TEntity>();
        }

        public TEntity SaveOrUpdate(TEntity entity)
        {
            ISession currentSession = OpenSession ?? ActiveSessionManager.GetActiveSession();

            currentSession.SaveOrUpdate(entity);
            return GetById(entity.EntityId);
        }

        public IEnumerable<TEntity> SaveOrUpdateList(IEnumerable<TEntity> entities)
        {
            return entities.Select(SaveOrUpdate).ToList();
        }


        public void Delete(TEntity entity)
        {
            ISession currentSession = OpenSession ?? ActiveSessionManager.GetActiveSession();
            currentSession.Delete(entity);
        }

        public ISession Session
        {
            get { return OpenSession ?? ActiveSessionManager.GetActiveSession(); }
        }
    }
}
