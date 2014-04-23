using Microsoft.Practices.Prism.Events;
using Trex.Core.Interfaces;

namespace Trex.Infrastructure.Events
{
    public class MenuItemRegisteredEvent : CompositePresentationEvent<IMenuInfo> {}
}