using Trex.Core.Interfaces;
using Trex.Core.Model;
using Trex.ServiceContracts;
using Trex.TaskAdministration.Interfaces;

namespace Trex.TaskAdministration.TaskManagementScreen.ButtonPanelView
{
    public class TaskActionPanelViewFactory : IActionPanelViewFactory
    {
        public Task Task;

        public TaskActionPanelViewFactory(Task task)
        {
            Task = task;
        }

        #region IActionPanelViewFactory Members

        public IView CreateActionPanelView()
        {
            var taskActionsViewModel = new TaskActionsViewModel(Task);
            var taskActionsView = new TaskActionsView();
            taskActionsView.ViewModel = taskActionsViewModel;

            return taskActionsView;
        }

        #endregion
    }
}