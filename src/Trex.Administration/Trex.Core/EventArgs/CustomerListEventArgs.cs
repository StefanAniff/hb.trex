using System.Collections.Generic;
using Trex.Core.Model;
using Trex.ServiceContracts;

namespace Trex.Core.EventArgs
{
    public class CustomerListEventArgs : System.EventArgs
    {
        public CustomerListEventArgs(List<Customer> customers)
        {
            if (customers != null)
            {
                Customers = customers;
            }
            else
            {
                Customers = new List<Customer>();
            }
        }

        public List<Customer> Customers { get; set; }
    }
}