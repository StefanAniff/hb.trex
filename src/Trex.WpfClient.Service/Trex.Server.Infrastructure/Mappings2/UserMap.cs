using FluentNHibernate.Mapping;
using Trex.Server.Core.Model;

namespace Trex.Server.Infrastructure.Mappings2
{
    /// <summary>
    /// User object for NHibernate mapped table 'Users'.
    /// </summary>
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.UserID);
            Map(x => x.UserName);
            Map(x => x.Name).Nullable();
            Map(x => x.Email).Nullable();
            Map(x => x.Price);
            Map(x => x.Inactive);
            HasManyToMany(x => x.Projects).AsBag().Cascade.None();
            HasMany(x => x.CustomerInfo).Cascade.None();
            HasMany(x => x.ForecastMonths).AsBag().LazyLoad().Cascade.AllDeleteOrphan();
            Map(x => x.NumOfTimeEntries).LazyLoad().Formula("(select Count(*) from TimeEntries t where t.UserId = UserId)");
            Map(x => x.TotalTime).LazyLoad().Formula("(select Sum(t.TimeSpent) from TimeEntries t where t.UserId = UserId)");
            Map(x => x.TotalBillableTime).LazyLoad().Formula("(select Sum(t.BillableTime) from TimeEntries t where t.UserId = UserId)");

            Table(ObjectNames.TableUser);
        }
    }
}