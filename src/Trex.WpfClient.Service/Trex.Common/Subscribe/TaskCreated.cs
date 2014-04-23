using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trex.Server.Common.Interfaces;

namespace Trex.Server.Common.Subscribe
{
    public class TaskCreated : EntityChanged, IEvent
    {
        public TaskCreated(int entityId, string applicationName)
            : base(entityId,  applicationName)
        {
        }
    }

    public class TaskUpdated : EntityChanged, IEvent
    {
        public TaskUpdated(int entityId, string applicationName)
            : base(entityId, applicationName)
        {
        }
    }
}