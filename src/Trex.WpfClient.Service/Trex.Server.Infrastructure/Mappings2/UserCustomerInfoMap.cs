using FluentNHibernate.Mapping;
using Trex.Server.Core.Model;

namespace Trex.Server.Infrastructure.Mappings2
{
    public class UserCustomerInfoMap : ClassMap<UserCustomerInfo>
    {
        public UserCustomerInfoMap()
        {
            CompositeId()
                .KeyProperty(x => x.UserID)
                .KeyProperty(x => x.CustomerID);
            Map(x => x.PricePrHour).Column("Price").Nullable();
            Table("UsersCustomers");
        }
    }
}