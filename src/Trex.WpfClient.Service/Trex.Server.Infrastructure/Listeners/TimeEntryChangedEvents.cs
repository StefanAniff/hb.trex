using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using Trex.Server.Common.Subscribe;
using Trex.Server.Core;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.Implemented;

namespace Trex.Server.Infrastructure.Listeners
{
    public class TimeEntryChangedEvents : IOnEntityCreated<TimeEntry>,
                                          IOnEntityUpdated<TimeEntry>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IMembershipProvider _membershipProvider;

        public TimeEntryChangedEvents(IEventPublisher eventPublisher, IMembershipProvider membershipProvider)
        {
            _eventPublisher = eventPublisher;
            _membershipProvider = membershipProvider;
        }

        public void Created(TimeEntry entity)
        {
            _eventPublisher.Publish(new TimeEntryCreated(entity.Id, _membershipProvider.GetApplicationName()));
        }

        public void Updated(TimeEntry entity)
        {
            _eventPublisher.Publish(new TimeEntryUpdated(entity.Id, _membershipProvider.GetApplicationName()));
        }
    }
}