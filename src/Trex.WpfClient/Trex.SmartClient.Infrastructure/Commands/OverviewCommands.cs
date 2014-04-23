using Microsoft.Practices.Prism.Commands;

namespace Trex.SmartClient.Infrastructure.Commands
{
    public class OverviewCommands
    {
        public static CompositeCommand GoToDaySubMenu = new CompositeCommand();
        public static CompositeCommand AddNewTask = new CompositeCommand();
        public static CompositeCommand  GoToPreviousView = new CompositeCommand();
    }
}