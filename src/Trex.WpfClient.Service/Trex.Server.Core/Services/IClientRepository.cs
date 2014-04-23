using System;
using Trex.Server.Core.Model;

namespace Trex.Server.Core.Services
{
    public interface IClientRepository
    {
        Client FindClientByCustomerId(string customerId);
    }
}
