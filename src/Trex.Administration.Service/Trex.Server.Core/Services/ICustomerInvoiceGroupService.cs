using System.Collections.Generic;
using Trex.ServiceContracts;

namespace Trex.Server.Infrastructure.Implemented
{
    public interface ICustomerInvoiceGroupService
    {
        List<CustomerInvoiceGroup> GetCustomerInvoiceGroupById(int customerId);
        void InsertCustomerInvoiceGroupIntoDatabase(CustomerInvoiceGroup group);
        void DeleteCustomerInvoiceGroupIntoDatabase(int CustomerInvoiceGroupId);
        void OverwriteCig(CustomerInvoiceGroup cig);
        ServerResponse DeleteCustomerInvoiceGroup(int customerInvoiceGroupId);
        CustomerInvoiceGroup GetCustomerInvoiceGroupByCustomerInvoiceGroupId(int CustomerInvoiceGroupId);
    }
}