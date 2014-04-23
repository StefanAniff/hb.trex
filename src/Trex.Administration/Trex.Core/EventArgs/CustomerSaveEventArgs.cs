using Trex.Core.Model;
using Trex.ServiceContracts;

namespace Trex.Core.EventArgs
{
    public class CustomerSaveEventArgs : System.EventArgs
    {
        public CustomerSaveEventArgs(Customer customer)
        {
            Customer = customer;
        }

        public Customer Customer { get; set; }
    }
}