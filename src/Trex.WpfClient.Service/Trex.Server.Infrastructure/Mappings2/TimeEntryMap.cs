using FluentNHibernate.Mapping;
using Trex.Server.Core.Model;

namespace Trex.Server.Infrastructure.Mappings2
{
    /// <summary>
    /// TimeEntry object for NHibernate mapped table 'Trex.Servers'.
    /// </summary>
    public class TimeEntryMap : ClassMap<TimeEntry>
    {
        public TimeEntryMap()
        {
            Id(x => x.Id);
            Map(x => x.StartTime);
            Map(x => x.EndTime);
            Map(x => x.PauseTime);
            Map(x => x.Description).Nullable();
            Map(x => x.BillableTime);
            Map(x => x.Billable);
            Map(x => x.Price);
            References(x => x.Task);
            References(x => x.User).Not.Nullable();
            Map(x => x.ChangeDate).Nullable();
            References(x => x.ChangedBy).Column("ChangedBy").Nullable();
            Map(x => x.CreateDate).Nullable();
            Map(x => x.TimeSpent);
            Map(x => x.Guid);
            References(x => x.Invoice).ForeignKey("InvoiceId").Nullable();
            References(x => x.TimeEntryType);
            Map(x => x.ClientSourceId);

            Table(ObjectNames.TableTimeEntry);
        }
    }
}