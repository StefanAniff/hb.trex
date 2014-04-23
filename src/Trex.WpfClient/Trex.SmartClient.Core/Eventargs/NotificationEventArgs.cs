using System;

namespace Trex.SmartClient.Core.Eventargs
{
    public class NotificationEventArgs:System.EventArgs
    {
        public NotificationEventArgs(string title,string message, TimeSpan duration)
        {
            Title = title;
            Message = message;
            Duration = duration;
        }

        public string Title { get; set; }
        public string Message { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
