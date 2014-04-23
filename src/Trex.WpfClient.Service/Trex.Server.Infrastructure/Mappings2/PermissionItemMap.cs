using FluentNHibernate.Mapping;
using Trex.Server.Core.Model;

namespace Trex.Server.Infrastructure.Mappings2
{
    public class PermissionItemMap : ClassMap<PermissionItem>
    {
        public PermissionItemMap()
        {
            Id(x => x.Id);
            Map(x => x.PermissionName);
            Map(x => x.IsEnabled);
        }
    }
}