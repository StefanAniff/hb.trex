using FluentNHibernate.Mapping;
using Trex.Server.Core.Model;

namespace Trex.Server.Infrastructure.Mappings2
{
    /// <summary>
    /// Customer object for NHibernate mapped table 'Customers'.
    /// </summary>
    public class CompanyMap : ClassMap<Company>
    {
        public CompanyMap()
        {
            Id(x => x.CustomerID).Column("CustomerID");
            Map(x => x.Guid).Not.Nullable();
            Map(x => x.CustomerName).Not.Nullable();
            Map(x => x.PhoneNumber).Nullable();
            Map(x => x.Email).Nullable();
            Map(x => x.CreateDate).Not.Nullable();
            References(x => x.CreatedBy).Column("CreatedBy").Not.Nullable();
            Map(x => x.ChangeDate).Nullable();
            References(x => x.ChangedBy).Column("ChangedBy").Nullable();
            Map(x => x.Inactive).Not.Nullable();
            HasMany(x => x.Projects).Inverse().Cascade.All();
            Map(x => x.StreetAddress).Nullable();
            Map(x => x.Address2).Nullable();
            Map(x => x.ZipCode).Nullable();
            Map(x => x.City).Nullable();
            Map(x => x.Country).Nullable();
            Map(x => x.ContactName).Nullable();
            Map(x => x.ContactPhone).Nullable();
            Map(x => x.PaymentTermsNumberOfDays).Not.Nullable();
            Map(x => x.PaymentTermsIncludeCurrentMonth).Not.Nullable();
            HasMany(x => x.Invoices).Inverse().Cascade.All();
            HasMany(x => x.TimeEntryTypes).Cascade.All();
            Map(x => x.InheritsTimeEntryTypes).Not.Nullable();
            Map(x => x.Internal).Not.Nullable();
            //Map(x => x.TotalNotInvoicedTime).LazyLoad()
            //    .Formula("(select sum(te.billabletime) from projects p left join tasks t on p.projectid = t.projectid left join timeentries te on te.taskid = t.taskid where p.customerid = CustomerID and te.billable = 1 and te.invoiceid is null)");
            //Map(x => x.DistinctPrices).LazyLoad()
            //    .Formula("(select count(distinct te.price) from projects p left join tasks t on p.projectid = t.projectid left join timeentries te on te.taskid = t.taskid where p.customerid = CustomerID and te.billable = 1 and te.invoiceid is null)");
            //Map(x => x.InventoryValue).LazyLoad()
            //    .Formula("(select sum(te.price * te.BillableTime) from projects p left join tasks  t on p.projectid = t.projectid left join timeentries te on te.taskid = t.taskid where p.customerid = CustomerID and te.billable = 1 and te.invoiceid is null)");
            //Map(x => x.FirstTimeEntryDate).LazyLoad()
            //    .Formula("(select min(te.StartTime) from projects p left join tasks  t on p.projectid = t.projectid left join timeentries te on te.taskid = t.taskid where p.customerid = CustomerID and te.billable = 1 and te.invoiceid is null)");
     
            Table(ObjectNames.TableCustomers);
        }
    }
}