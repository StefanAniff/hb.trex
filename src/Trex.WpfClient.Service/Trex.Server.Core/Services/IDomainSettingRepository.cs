using Trex.Server.Core.Model;

namespace Trex.Server.Core.Services
{
    public interface IDomainSettingRepository : IRepository<DomainSetting>
    {
        DomainSetting GetByName(string name);
    }
}