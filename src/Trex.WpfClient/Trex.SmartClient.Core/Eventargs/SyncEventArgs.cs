using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trex.SmartClient.Core.Eventargs
{
    public delegate void SyncEventHandler(SyncEventArgs args);

    public class SyncEventArgs : EventArgs
    {
        public string SyncMessage { get; set; }

        public SyncEventArgs(string syncMessage)
        {
            SyncMessage = syncMessage;
        }
    }
}
