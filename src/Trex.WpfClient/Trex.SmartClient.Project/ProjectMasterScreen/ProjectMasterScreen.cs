using System;
using Microsoft.Practices.Unity;
using Trex.SmartClient.Core.Attributes;
using Trex.SmartClient.Core.Implemented;

namespace Trex.SmartClient.Project.ProjectMasterScreen
{
    [Screen(IsStartScreen = false, CanBeDeactivated = true)]
    public class ProjectMasterScreen : ScreenBase
    {
        public ProjectMasterScreen(Guid guid, IUnityContainer unityContainer) : base(guid)
        {
            var view = unityContainer.Resolve<IProjectMasterView>();
            var viewModel = unityContainer.Resolve<IProjectMasterViewModel>();
            view.ApplyViewModel(viewModel);

            MasterView = view;
        }
    }
}