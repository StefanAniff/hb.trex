using FluentNHibernate.Mapping;
using Trex.Server.Core.Model;

namespace Trex.Server.Infrastructure.Mappings2
{
    public class ClientMap : ClassMap<Client>
    {
        public ClientMap()
        {
            Id(x => x.Id).Column("Id");
            Map(x => x.CompanyName).Not.Nullable();
            Map(x => x.VatNumber);
            Map(x => x.Country);
            Map(x => x.Address1);
            Map(x => x.Address2);
            Map(x => x.Address3);
            Map(x => x.Address4);
            Map(x => x.Address5);
            Map(x => x.CreatorUserName);
            Map(x => x.CreatorFullName);
            Map(x => x.CreatorPhone);
            Map(x => x.CreatorEmail);
            Map(x => x.CustomerId).Length(50);
            Map(x => x.ConnectionString).Nullable();
            Map(x => x.Inactive);
            Map(x => x.InactiveDate).Nullable();
            Map(x => x.IsLockedOut);
            Map(x => x.LockedOutDate).Nullable();
            Map(x => x.CreateDate);
            Map(x => x.IsActivated);
            Map(x => x.ActivationId);

            Table("Customers");
        }
    }
}
