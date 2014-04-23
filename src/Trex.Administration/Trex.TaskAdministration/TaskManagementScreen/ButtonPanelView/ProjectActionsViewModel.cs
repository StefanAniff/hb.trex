using Microsoft.Practices.Prism.Commands;
using Trex.Core.Implemented;
using Trex.ServiceContracts;
using Trex.TaskAdministration.Commands;

namespace Trex.TaskAdministration.TaskManagementScreen.ButtonPanelView
{
    public class ProjectActionsViewModel : ViewModelBase
    {
        private readonly Project _project;

        public ProjectActionsViewModel(Project project)
        {
            _project = project;
            EditProject = new DelegateCommand<object>(ExecuteEditProject);
            AddTask = new DelegateCommand<object>(ExecuteAddTask,CanAddTask);
            DeleteProject = new DelegateCommand<object>(ExecuteDeleteProject, CanDeleteProject);
        }

        private bool CanAddTask(object arg)
        {
            return !_project.Inactive;
        }

        public DelegateCommand<object> EditProject { get; set; }
        public DelegateCommand<object> DeleteProject { get; set; }

        public DelegateCommand<object> AddTask { get; set; }

        private void ExecuteDeleteProject(object obj)
        {
            InternalCommands.ProjectDeleteStart.Execute(_project);
        }

        private bool CanDeleteProject(object arg)
        {
            return _project.Tasks.Count == 0;
        }

        private void ExecuteAddTask(object obj)
        {
            InternalCommands.TaskAddStart.Execute(_project);
        }

        private void ExecuteEditProject(object obj)
        {
            InternalCommands.ProjectEditStart.Execute(_project);
        }
    }
}