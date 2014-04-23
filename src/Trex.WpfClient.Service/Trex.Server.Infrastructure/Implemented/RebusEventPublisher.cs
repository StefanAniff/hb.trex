using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rebus;
using Trex.Server.Common.Interfaces;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented
{
    /// <summary>
    /// Implementation of <see cref="IEventPublisher"/> that uses Rebus to publish events
    /// </summary>
    public class RebusEventPublisher : IEventPublisher
    {
        readonly IBus _bus;

        public RebusEventPublisher(IBus bus)
        {
            _bus = bus;
        }

        public void Publish(IEvent eventToPublish)
        {
            try
            {
                _bus.Publish(eventToPublish);
            }
            catch (Exception exception)
            {
                throw new ApplicationException(string.Format(@"Could not publish event {0} - got an exception: {1}",
                                                             eventToPublish, exception));
            }
        }
    }
}
