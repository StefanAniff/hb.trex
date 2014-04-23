using Microsoft.Practices.Prism.Commands;

namespace Trex.SmartClient.Infrastructure.Commands
{
    public static class TaskCommands
    {
        public static CompositeCommand StartNewTask = new CompositeCommand();
        public static CompositeCommand TaskStarted = new CompositeCommand();
        public static CompositeCommand TaskIdle = new CompositeCommand();
        public static CompositeCommand PauseActiveTask = new CompositeCommand();
        public static CompositeCommand StopActiveTask = new CompositeCommand();
        public static CompositeCommand ToggleActiveTask = new CompositeCommand();
        public static CompositeCommand SaveActiveTask = new CompositeCommand();
        public static CompositeCommand ActivateTask = new CompositeCommand();
        public static CompositeCommand EditTaskStart = new CompositeCommand();
        public static CompositeCommand SaveTaskStart = new CompositeCommand();
        public static CompositeCommand SaveTaskCompleted = new CompositeCommand();
        public static CompositeCommand SaveTaskCancelled = new CompositeCommand();
        public static CompositeCommand AssignTask = new CompositeCommand();
        public static CompositeCommand TaskAssigned = new CompositeCommand();
        public static CompositeCommand TaskActivated = new CompositeCommand();
        public static CompositeCommand TaskDeactivated = new CompositeCommand();
        public static CompositeCommand TaskSelectCompleted = new CompositeCommand();
        public static CompositeCommand CloseInactiveTask= new CompositeCommand();
        public static CompositeCommand CloseActiveTask = new CompositeCommand();
        public static CompositeCommand DeactivateActiveTask = new CompositeCommand();
        public static CompositeCommand CloseAllInactiveTasks = new CompositeCommand();
    }
}
