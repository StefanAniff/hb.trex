using Trex.Core.Implemented;
using Trex.Core.Services;

namespace Trex.Reports.StatusReportScreen.StatusReportView
{
    public class StatusReportViewModel : ViewModelBase
    {
        private readonly IUserSettingsService _userSettingsService;

        public StatusReportViewModel(IUserSettingsService userSettingsService)
        {
            _userSettingsService = userSettingsService;
            LoginSettings = _userSettingsService.GetSettings();
        }

        public ILoginSettings LoginSettings { get; set; }
    }
}