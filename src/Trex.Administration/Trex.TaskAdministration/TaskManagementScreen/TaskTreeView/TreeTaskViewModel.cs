using System.Windows.Media;
using Trex.Core.Services;
using Trex.Infrastructure.Commands;
using Trex.ServiceContracts;
using Trex.TaskAdministration.Commands;
using Trex.TaskAdministration.EventArgs;
using Trex.TaskAdministration.Interfaces;

namespace Trex.TaskAdministration.TaskManagementScreen.TaskTreeView
{
    public class TreeTaskViewModel : TreeViewItemViewModel
    {
        private readonly IDataService _dataService;

        public TreeTaskViewModel(TreeViewItemViewModel parent, Task task, IDataService dataService)
            : base(parent, task)
        {
            _dataService = dataService;
        }

        public override string DisplayName
        {
            get { return Task.TaskName + TimeEntryCountDisplay; }
        }

        public SolidColorBrush DisplayColor
        {
            get {
                return Task.Inactive ? InActiveColor : ActiveColor;
            }
        }

        public override bool IsSelected
        {
            get { return base.IsSelected; }
            set
            {
                base.IsSelected = value;
                if (!IsLoadOnDemandEnabled && IsSelected)
                {
                    TreeCommands.TaskSelected.Execute(this);
                }
                if (!IsSelected)
                {
                    TreeCommands.TaskDeSelected.Execute(this);
                }
            }
        }
        
        private string TimeEntryCountDisplay
        {
            get
            {
                var entries = Task.TimeEntries;
                return entries != null && entries.Count > 0 ? " (" + entries.Count + ")" : string.Empty;
            }
        }

        public Task Task
        {
            get { return (Task) Entity; }
        }

        public TreeProjectViewModel ParentProject
        {
            get { return Parent as TreeProjectViewModel; }
        }

        public override void RecieveDraggable(IDraggable draggable)
        {
            var timeEntry = draggable.Entity as TimeEntry;

            var eventArgs = new MoveEntityEventArgs(timeEntry, timeEntry.Task, Task);

            timeEntry.Task.TimeEntries.Remove(timeEntry);
            Task.TimeEntries.Add(timeEntry);

            timeEntry.Task = Task;

            _dataService.SaveTimeEntry(timeEntry);

            InternalCommands.MoveEntityRequest.Execute(eventArgs);
        }
    }
}