using System;
using System.Text;
using System.Windows;
using Trex.Core.Interfaces;
using Trex.Infrastructure.Commands;

namespace Trex.Infrastructure.Implemented
{
    public class DataLoadingNotifier : IDataLoadingNotifier
    {
        public void NotifySystemLoadingData()
        {
            Execute.InUIThread(() => ApplicationCommands.SystemBusy.Execute("Loading data"));
        }

        public void NotifySystemIdle()
        {
            Execute.InUIThread(() => ApplicationCommands.SystemIdle.Execute(null));
        }

        public void HandleLoadFailed(Exception exception)
        {
            NotifySystemIdle();
            var msg = new StringBuilder()
                .AppendLine("An error occured with message:")
                .AppendLine()
                .AppendLine(exception != null ? exception.Message : "Exception is null")
                .ToString();

            MessageBox.Show(msg);
        }
    }
}