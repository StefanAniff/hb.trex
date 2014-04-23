using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Trex.Core.Services;
using Trex.Infrastructure.Commands;
using Trex.ServiceContracts;
using Trex.TaskAdministration.Commands;
using Trex.TaskAdministration.EventArgs;
using Trex.TaskAdministration.Interfaces;

namespace Trex.TaskAdministration.TaskManagementScreen.TaskTreeView
{
    public class TreeProjectViewModel : TreeViewItemViewModel, IProjectViewModel
    {
        private readonly IDataService _dataService;

        public TreeProjectViewModel(TreeViewItemViewModel parent, Project project, IDataService dataService)
            : base(parent, project)
        {
            _dataService = dataService;

            LoadTasks(project.Tasks);
        }

        public override string DisplayName
        {
            get { return string.Format(Project.ProjectName + ChildrenCountDisplay); }
        }

        public SolidColorBrush DisplayColor
        {
            get
            {
                return Project.Inactive ? InActiveColor : ActiveColor;
            }
        }

        public override bool IsSelected
        {
            get { return base.IsSelected; }
            set
            {
                base.IsSelected = value;
                if (IsSelected)
                {
                    TreeCommands.ProjectSelected.Execute(this);
                }
                else
                {
                    TreeCommands.ProjectDeSelected.Execute(this);
                }
            }
        }

        #region IProjectViewModel Members

        public Project Project
        {
            get { return (Project)Entity; }
        }

        public int CustomerId
        {
            get
            {
                var parentCustomer = ParentCustomer;
                return parentCustomer != null ? parentCustomer.Customer.Id : 0;
            }
        }

        public TreeCustomerViewModel ParentCustomer
        {
            get { return (TreeCustomerViewModel) Parent; }
        }

        #endregion

        //private void _dataService_GetTasksByProjectCompleted(object sender, ProjectListEventArgs e)
        //{
        //    _dataService.GetTasksByProjectCompleted -= _dataService_GetTasksByProjectCompleted;
        //    if (e.Projects.Count == 0)
        //    {
        //        return;
        //    }

        //    var tasks = e.Projects[0].Tasks;
        //    LoadTasks(tasks);
        //}

        public override void RecieveDraggable(IDraggable draggable)
        {
            var task = draggable.Entity as Task;
            var eventArgs = new MoveEntityEventArgs(task, task.Project, Project);

            task.Project = Project;
            task.ProjectID = Project.Id;
            _dataService.SaveTask(task);

            InternalCommands.MoveEntityRequest.Execute(eventArgs);
        }

        public override void AddChild(IEntity entity)
        {
            Children.Add(new TreeTaskViewModel(this, entity as Task, _dataService));

            if (Project.Tasks.FirstOrDefault(t => t.Id == ((Task)entity).Id) == null)
            {
                Project.Tasks.Add(entity as Task);
            }
        }

        private void LoadTasks(IEnumerable<Task> tasks)
        {
            foreach (var task in tasks)
            {
                task.Project = Project;

                var taskViewModel = new TreeTaskViewModel(this, task, _dataService);
                taskViewModel.IsLoadOnDemandEnabled = false;
                //LoadSubTasks(task, taskViewModel);
                Children.Add(taskViewModel);
            }
        }

        //private void LoadSubTasks(Task task, TreeViewItemViewModel viewModel)
        //{
        //    foreach (var subtask in task.SubTasks)
        //    {
        //        subtask.Project = Project;
        //        var taskViewModel = new TreeTaskViewModel(viewModel, subtask, _dataService);
        //        taskViewModel.IsLoadOnDemandEnabled = false;
        //        viewModel.Children.Add(taskViewModel);
        //        LoadSubTasks(subtask, viewModel);
        //    }
        //}

        protected override void LoadChildren()
        {

            foreach (var task in Project.Tasks)
            {
                task.Project = Project;

                var taskViewModel = new TreeTaskViewModel(this, task, _dataService) { IsLoadOnDemandEnabled = false };
                //LoadSubTasks(task, taskViewModel);
                Children.Add(taskViewModel);
            }
            //_dataService.GetTasksByProjectCompleted += _dataService_GetTasksByProjectCompleted;
            //_dataService.GetTasksByProject(Project, true, true);
        }
    }
}