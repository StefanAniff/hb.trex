using System;

namespace Trex.Core.Services
{
    public interface ICustomerFilterService
    {
        DateTime? LastEntry { get; set; }
    }
}