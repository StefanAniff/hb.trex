using NHibernate;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.UnitOfWork;

namespace Trex.Server.Infrastructure.Implemented
{
    public class DomainSettingRepository : GenericRepository<DomainSetting>, IDomainSettingRepository
    {
        public DomainSettingRepository(ISession openSession) : base(openSession)
        {
        }

        public DomainSettingRepository(IActiveSessionManager activeSessionManager) : base(activeSessionManager)
        {
        }

        public virtual DomainSetting GetByName(string name)
        {
            return Session
                .QueryOver<DomainSetting>()
                .WhereRestrictionOn(x => x.Name)
                .IsInsensitiveLike(name.ToLower())
                .SingleOrDefault();
        }
    }
}