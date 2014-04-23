using Microsoft.Practices.Prism.Commands;

namespace Trex.Roles.Commands
{
    public static class InternalCommands
    {
        public static CompositeCommand CreateNewRoleStart = new CompositeCommand();
        public static CompositeCommand CreateNewRoleCompleted = new CompositeCommand();

        public static CompositeCommand DeleteRoleStart = new CompositeCommand();
        public static CompositeCommand DeleteRoleCompleted = new CompositeCommand();
    }
}