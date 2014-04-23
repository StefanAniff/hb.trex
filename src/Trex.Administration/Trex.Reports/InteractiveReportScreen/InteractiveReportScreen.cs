using System;
using Microsoft.Practices.Unity;
using Trex.Core.Attributes;
using Trex.Core.Implemented;
using Trex.Core.Services;
using Trex.Reports.InteractiveReportScreen.InteractiveReportView;

namespace Trex.Reports.InteractiveReportScreen
{
    [Screen(Name = "InteractiveReportScreen", CanBeDeactivated = true)]
    public class InteractiveReportScreen : ScreenBase
    {
        private readonly IUnityContainer _unityContainer;
        public InteractiveReportScreen(Guid guid, IUnityContainer unityContainer) : base(guid, unityContainer)
        {
            _unityContainer = unityContainer;
        }

        protected override void Initialize()
        {
            var dataService = Container.Resolve<IDataService>();

                var view = new InteractiveReport();
                var model = new InteractiveReportViewModel(dataService);

                view.ViewModel = model;

                MasterView = view;
        }
    }
}