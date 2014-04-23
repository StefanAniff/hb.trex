using FluentNHibernate.Mapping;
using Trex.Server.Core.Model;

namespace Trex.Server.Infrastructure.Mappings2
{
    public class DomainSettingMap : ClassMap<DomainSetting>
    {
        public DomainSettingMap()
        {
            Id(x => x.Id).Column("Id"); ;
            Map(x => x.Name).Not.Nullable();
            Map(x => x.Value).Not.Nullable();

            Table(ObjectNames.TableDomainSettings);
        }
    }
}