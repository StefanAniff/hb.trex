using FluentNHibernate.Mapping;
using Trex.Server.Core.Model;

namespace Trex.Server.Infrastructure.Mappings2
{
    /// <summary>
    /// Project object for NHibernate mapped table 'Projects'.
    /// </summary>
    public class ProjectMap : ClassMap<Project>
    {
        public ProjectMap()
        {
            Id(x => x.ProjectID);
            Map(x => x.Guid);
            Map(x => x.ProjectName);
            References(x => x.Company).Not.Nullable().Column("CustomerID");
            References(x => x.CreatedBy).Column("CreatedBy");
            Map(x => x.CreateDate);
            Map(x => x.ChangeDate);
            References(x => x.ChangedBy).Nullable().Column("ChangedBy");
            HasMany(x => x.Tasks).Cascade.AllDeleteOrphan().Where("ParentId IS NULL");
            //HasManyToMany(x => x.Users).AsBag().LazyLoad();
            Map(x => x.Inactive);
            Map(x => x.IsEstimatesEnabled);

            Table(ObjectNames.TableProject);
        }
    }
}