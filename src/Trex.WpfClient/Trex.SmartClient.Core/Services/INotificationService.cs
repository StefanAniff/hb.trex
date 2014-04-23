using System;
using Trex.SmartClient.Core.Eventargs;

namespace Trex.SmartClient.Core.Services
{
    public interface INotificationService : IDisposable
    {
        event EventHandler<NotificationEventArgs> OnNotification;
    }
}
