using Microsoft.Practices.Prism.Commands;

namespace Trex.Infrastructure.Commands
{
    public static class ApplicationCommands
    {
        public static CompositeCommand ChangeScreenCommand = new CompositeCommand();

        public static CompositeCommand ModuleLoadComplete = new CompositeCommand();
        public static CompositeCommand LoginSucceeded = new CompositeCommand();
        public static CompositeCommand UserLoggedOut = new CompositeCommand();
        public static CompositeCommand GoToLogin = new CompositeCommand();
        public static CompositeCommand CustomerDataReady = new CompositeCommand();
        public static CompositeCommand SystemBusy = new CompositeCommand();
        public static CompositeCommand SystemIdle = new CompositeCommand();
        public static CompositeCommand RefreshData = new CompositeCommand();
        public static CompositeCommand GotoFullScreenMode = new CompositeCommand();
        public static CompositeCommand LoginFailed = new CompositeCommand();
    }
}