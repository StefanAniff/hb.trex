using Trex.Core.Model;
using Trex.ServiceContracts;

namespace Trex.Core.EventArgs
{
    public class CustomerStatsEventArgs : System.EventArgs
    {
        public CustomerStatsEventArgs(CustomerStats customerStats)
        {
            if (customerStats != null)
            {
                CustomerStats = customerStats;
            }
            else
            {
                CustomerStats = new CustomerStats();
            }
        }

        public CustomerStats CustomerStats { get; set; }
    }
}