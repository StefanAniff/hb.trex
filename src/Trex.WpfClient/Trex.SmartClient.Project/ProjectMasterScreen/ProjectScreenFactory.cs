using System;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Project.ProjectAdministration;
using Trex.SmartClient.Project.TaskDisposition;

namespace Trex.SmartClient.Project.ProjectMasterScreen
{
    public class ProjectScreenFactory : IScreenFactory
    {
        private readonly IRegionNames _regionNames;
        private readonly IUnityContainer _unityContainer;

        public ProjectScreenFactory(IRegionNames regionNames, IUnityContainer unityContainer)
        {
            _regionNames = regionNames;
            _unityContainer = unityContainer;
        }

        public IScreen CreateScreen(IRegion region, Guid guid)
        {
            var projectScreen = new ProjectMasterScreen(guid, _unityContainer);
            projectScreen.InitializeScreen(region, guid);

            // Administration
            var projectAdminView = _unityContainer.Resolve<IProjectAdministrationView>();
            var projectAdminViewModel = _unityContainer.Resolve<IProjectAdministrationViewModel>();
            projectAdminView.ApplyViewModel(projectAdminViewModel);
            projectScreen.AddRegion(_regionNames.SubmenuRegion, projectAdminView);

            // Disposition
            var taskDispositionView = _unityContainer.Resolve<ITaskDispositionView>();
            var taskDispositionViewModel = _unityContainer.Resolve<ITaskDispositionViewModel>();
            taskDispositionView.ApplyViewModel(taskDispositionViewModel);
            projectScreen.AddRegion(_regionNames.SubmenuRegion, taskDispositionView);

            return projectScreen;
        }
    }
}