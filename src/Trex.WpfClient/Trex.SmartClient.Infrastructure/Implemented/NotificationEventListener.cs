using System;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Eventargs;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Infrastructure.Commands;

namespace Trex.SmartClient.Infrastructure.Implemented
{
    public class NotificationEventListener : INotificationListener
    {
        public event EventHandler<NotificationEventArgs> NotificationRecieved;

        public NotificationEventListener()
        {
            ApplicationCommands.DoNotification.RegisterCommand(new DelegateCommand<NotificationEventArgs>(Notify));
        }

        private void Notify(NotificationEventArgs obj)
        {
            if (NotificationRecieved != null)
            {
                NotificationRecieved(this, obj);
            }
        }
    }
}
