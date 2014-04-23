using Trex.Server.Common.Subscribe;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.Implemented;

namespace Trex.Server.Infrastructure.Listeners
{
    public class TaskChangedEvents : IOnEntityCreated<Task>,
                                     IOnEntityUpdated<Task>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IMembershipProvider _membershipProvider;

        public TaskChangedEvents(IEventPublisher eventPublisher, IMembershipProvider membershipProvider)
        {
            _eventPublisher = eventPublisher;
            _membershipProvider = membershipProvider;
        }

        public void Created(Task entity)
        {
            _eventPublisher.Publish(new TaskCreated(entity.TaskID, _membershipProvider.GetApplicationName()));
        }

        public void Updated(Task entity)
        {
            _eventPublisher.Publish(new TaskUpdated(entity.TaskID, _membershipProvider.GetApplicationName()));
        }
    }
}