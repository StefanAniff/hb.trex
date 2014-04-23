using Microsoft.Practices.Prism.Commands;

namespace Trex.SmartClient.Core.Utils
{
    public class ExtendedCompositeCommand : CompositeCommand
    {
        public override void Execute(object parameter)
        {
            Utils.Execute.InUIThread(() => base.Execute(parameter));
        }

        public override bool CanExecute(object parameter)
        {
            if (RegisteredCommands.Count < 1)
            {
                return true;
            }
            return base.CanExecute(parameter);
        }
    }
}