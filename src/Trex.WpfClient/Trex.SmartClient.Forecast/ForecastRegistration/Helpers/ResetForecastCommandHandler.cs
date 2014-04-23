using System.Text;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.Forecast.ForecastRegistration.Helpers
{
    public class ResetForecastCommandHandler
    {
        private readonly ICommonDialogs _commonDialogs;

        public ResetForecastCommandHandler(ICommonDialogs commonDialogs)
        {
            _commonDialogs = commonDialogs;
        }

        public void Execute(IForecastRegistrationViewModel vm)
        {
            var msg = new StringBuilder()
                .AppendLine("All unsaved changes will be lost!")
                .AppendLine()
                .AppendLine("Continue with reset?");

            if (!_commonDialogs.ContinueWarning(msg.ToString(), "Reset")) 
                return;

            vm.RefreshViewData();
        }

        public bool CanExecute(IForecastRegistrationViewModel vm)
        {
            if (vm.ForecastMonthIsLocked)
                return false;

            return vm.IsDirty;
        }
    }
}