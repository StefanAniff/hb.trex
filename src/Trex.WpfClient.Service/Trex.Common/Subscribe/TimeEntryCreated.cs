using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trex.Server.Common.Interfaces;

namespace Trex.Server.Common.Subscribe
{
    public class TimeEntryCreated : EntityChanged, IEvent
    {
        public TimeEntryCreated(int entityId, string applicationName)
            : base(entityId, applicationName)
        {
        }
    }

    public class TimeEntryUpdated : EntityChanged, IEvent
    {
        public TimeEntryUpdated(int entityId, string applicationName)
            : base(entityId, applicationName)
        {
        }
    }
}