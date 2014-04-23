using System;
using System.Windows.Forms;
using Microsoft.Practices.Prism.Commands;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Infrastructure.Commands;

namespace Trex.SmartClient.NotifyControl
{
    public partial class NotifyControl : UserControl
    {

        private IUserSession _userSession;
        public NotifyControl(INotificationListener notificationService,IUserSession userSession)
        {
            InitializeComponent();
            notificationService.NotificationRecieved += notificationService_OnNotification;
            ApplicationCommands.LoginSucceeded.RegisterCommand(new DelegateCommand<object>(Update));
            ApplicationCommands.UserLoggedOut.RegisterCommand(new DelegateCommand<object>(Update));
            _userSession = userSession;
        }

       

        private void Update(object obj)
        {
            startPauseActiveTaskAltF11ToolStripMenuItem.Enabled = _userSession.IsLoggedIn; 
            stopActiveTaskAltF10ToolStripMenuItem.Enabled = _userSession.IsLoggedIn;
            assignActiveTaskAltF9ToolStripMenuItem.Enabled = _userSession.IsLoggedIn;
        }

        void notificationService_OnNotification(object sender, Trex.SmartClient.Core.Eventargs.NotificationEventArgs e)
        {
            notifyIcon1.BalloonTipText = e.Message;
            notifyIcon1.BalloonTipTitle = e.Title;
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.BalloonTipClicked += new EventHandler(notifyIcon1_BalloonTipClicked);
            notifyIcon1.ShowBalloonTip((int)e.Duration.TotalMilliseconds);


        }

        void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            ApplicationCommands.OpenMainWindow.Execute(null);
        }

        private void startPauseActiveTaskAltF11ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaskCommands.ToggleActiveTask.Execute(null);
        }

        private void toggleWindowAltF5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationCommands.ToggleWindow.Execute(null);
        }

        private void startNewTaskAltF12ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaskCommands.StartNewTask.Execute(null);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationCommands.ExitApplication.Execute(null);
        }

        private void stopActiveTaskAltF10ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaskCommands.StopActiveTask.Execute(null);
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ApplicationCommands.OpenMainWindow.Execute(null);
        }

        private void assignActiveTaskAltF9ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaskCommands.AssignTask.Execute(null);
        }
    }
}