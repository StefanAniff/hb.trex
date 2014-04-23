using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trex.Server.Common.Interfaces
{
    public abstract class EntityChanged
    {
        public string ApplicationName { get; set; }

        protected EntityChanged(int entityId, string applicationName)
        {
            ApplicationName = applicationName;
            if (entityId == 0)
            {
                throw new ArgumentException(
                    "Specified entity ID was 0! Are you trying to publish the event with the ID of a transient entity?",
                    "entityId");
            }

            EntityId = entityId;
        }

        public int EntityId { get; private set; }
    }
}