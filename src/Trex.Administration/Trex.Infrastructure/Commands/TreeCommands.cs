using Microsoft.Practices.Prism.Commands;

namespace Trex.Infrastructure.Commands
{
    public static class TreeCommands
    {
        public static CompositeCommand CustomerSelected = new CompositeCommand();
        public static CompositeCommand CustomerDeselected = new CompositeCommand();
        public static CompositeCommand ProjectSelected = new CompositeCommand();
        public static CompositeCommand ProjectDeSelected = new CompositeCommand();
        public static CompositeCommand TaskSelected = new CompositeCommand();
        public static CompositeCommand TaskDeSelected = new CompositeCommand();
    }
}