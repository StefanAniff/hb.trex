using System;
using Trex.SmartClient.Core.Eventargs;

namespace Trex.SmartClient.Core.Services
{
    public interface INotificationListener
    {
        event EventHandler<NotificationEventArgs> NotificationRecieved;
    }
}
