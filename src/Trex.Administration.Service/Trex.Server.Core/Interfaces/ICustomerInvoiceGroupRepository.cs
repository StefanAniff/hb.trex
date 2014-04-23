using System.Collections.Generic;
using Trex.ServiceContracts;

namespace Trex.Server.Infrastructure.Implemented
{
    public interface ICustomerInvoiceGroupRepository
    {
        List<CustomerInvoiceGroup> GetCustomerInvoiceGroupIDByCustomerID(Customer customer);
        int GetCustomerIdByCustomerInvoiceGroupId(int customerInvoiceGroupId);
        void InsertInDatabase(CustomerInvoiceGroup group);
        void DeleteInDatabase(int CustomerInvoiceGroupId);
        void OverwriteCig(CustomerInvoiceGroup cig);
        ServerResponse DeleteCustomerInvoiceGroup(int customerInvoiceGroupId);
        CustomerInvoiceGroup GetCustomerInvoiceGroupByCustomerInvoiceGroupID(int customerInvoiceGroupId);
    }
}