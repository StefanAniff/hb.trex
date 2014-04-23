using Trex.Core.Interfaces;
using Trex.Core.Model;
using Trex.ServiceContracts;
using Trex.TaskAdministration.Interfaces;

namespace Trex.TaskAdministration.TaskManagementScreen.ButtonPanelView
{
    public class ProjectActionPanelViewFactory : IActionPanelViewFactory
    {
        private readonly Project _project;

        public ProjectActionPanelViewFactory(Project project)
        {
            _project = project;
        }

        #region IActionPanelViewFactory Members

        public IView CreateActionPanelView()
        {
            var projectActionsView = new ProjectActionsView();
            var projectActionsViewModel = new ProjectActionsViewModel(_project);

            projectActionsView.ViewModel = projectActionsViewModel;
            return projectActionsView;
        }

        #endregion
    }
}