using System;
using Trex.Core.Services;

namespace Trex.Infrastructure.Implemented
{
    public class CustomerFilterService : ICustomerFilterService
    {
        #region ICustomerFilterService Members

        public DateTime? LastEntry { get; set; }

        #endregion
    }
}