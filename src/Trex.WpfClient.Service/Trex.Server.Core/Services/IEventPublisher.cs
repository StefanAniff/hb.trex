using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trex.Server.Common.Interfaces;

namespace Trex.Server.Core.Services
{
    /// <summary>
    /// Publishes an event out of the application (most likely with Rebus)
    /// </summary>
    public interface IEventPublisher
    {
        void Publish(IEvent eventToPublish);
    }
}
