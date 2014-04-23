using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trex.Server.Infrastructure.BaseClasses;
using Trex.ServiceContracts;

namespace Trex.Server.Infrastructure.Implemented
{
    public class CustomerInvoiceGroupService : LogableBase, ICustomerInvoiceGroupService
    {
        private readonly ICustomerInvoiceGroupRepository _customerInvoiceGroupRepository;
        private readonly IRepository _repository;

        public CustomerInvoiceGroupService(ICustomerInvoiceGroupRepository customerInvoiceGroupRepository, IRepository repository)
        {
            _customerInvoiceGroupRepository = customerInvoiceGroupRepository;
            _repository = repository;
        }

        public List<CustomerInvoiceGroup> GetCustomerInvoiceGroupById(int customerId)
        {
            var customer = _repository.GetCustomerById(customerId);

            return _customerInvoiceGroupRepository.GetCustomerInvoiceGroupIDByCustomerID(customer);
        }

        public void InsertCustomerInvoiceGroupIntoDatabase(CustomerInvoiceGroup group)
        {
            _customerInvoiceGroupRepository.InsertInDatabase(group);
        }

        public void DeleteCustomerInvoiceGroupIntoDatabase(int CustomerInvoiceGroupId)
        {
            _customerInvoiceGroupRepository.DeleteInDatabase(CustomerInvoiceGroupId);
        }

        public CustomerInvoiceGroup GetCustomerInvoiceGroupByCustomerInvoiceGroupId(int CustomerInvoiceGroupId)
        {
            return _customerInvoiceGroupRepository.GetCustomerInvoiceGroupByCustomerInvoiceGroupID(CustomerInvoiceGroupId);
        }

        public void OverwriteCig(CustomerInvoiceGroup cig)
        {
            _customerInvoiceGroupRepository.OverwriteCig(cig);
        }

        public ServerResponse DeleteCustomerInvoiceGroup(int customerInvoiceGroupId)
        {
            return _customerInvoiceGroupRepository.DeleteCustomerInvoiceGroup(customerInvoiceGroupId);
        }

        
    }
}
