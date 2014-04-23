using System.Linq;
using System.Windows;
using Trex.Core.Interfaces;
using Trex.Core.Model;
using Trex.Core.Services;
using Trex.Infrastructure.Implemented;
using Trex.ServiceContracts;
using Trex.TaskAdministration.Commands;
using Trex.TaskAdministration.EventArgs;
using Trex.TaskAdministration.Interfaces;
using Trex.TaskAdministration.TaskManagementScreen.ButtonPanelView;

namespace Trex.TaskAdministration.TaskManagementScreen.RightPanelView
{
    public class ListTaskViewModel : ListItemModelBase
    {
        private ListItemModelBase _selectedItem;

        public ListTaskViewModel(Task task, ListItemModelBase parent, IDataService dataService, IUserRepository userRepository)
            : base(parent, task, dataService, userRepository)
        {
            ActionPanelViewFactory = new TaskActionPanelViewFactory(task);
            InitUserName();
        }


        private void InitUserName()
        {
             var creator  = UserRepository.GetById(Task.CreatedBy);
            if (creator != null)
                UserName = creator.Name;
            else
            {
                UserName = string.Empty;
            }
        }

        public Task Task
        {
            get { return (Task)Entity; }
        }

        public override string DisplayName
        {
            get { return Task.TaskName + VisibleChildrenCountString; }
        }

        private double _totalBillableTime;
        public override double TotalBillableTime
        {
            get { return CalculateTotalBillableTime(); }

        }

        public override double TotalTimeSpent
        {
            get
            {
                return Children.OfType<ListTimeEntryViewModel>().Sum(timeEntry => timeEntry.TotalTimeSpent);
            }
            set
            {
                base.TotalTimeSpent = value;
            }
        }

        public string TotalBillableTimeFormatted
        {
            get { return TotalBillableTime.ToString("N2"); }
        }

        public override double TotalEstimate
        {
            get
            {
                return Task.TimeEstimated;
            }
        }

        public override double TotalTimeLeft
        {
            get { return Task.TimeLeft; }
        }


        public override double TotalProgress
        {
            get { return CalculateTotalProgress(); }

        }

        public override double TotalRealisticProgress
        {
            get { return CalculateTotalRealisticProgress(); }

        }

        public override System.DateTime CreateDate
        {
            get { return Task.CreateDate; }
            set
            {
                base.CreateDate = value;
            }
        }

        public ListItemModelBase SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                if (_selectedItem != null)
                {
                    _selectedItem.IsSelected = true;
                }
                OnPropertyChanged("SelectedItem");
            }
        }

        //private void CalculateAll()
        //{
        //    Execute.InBackground(() =>
        //                             {
        //                                 CalculateTotalBillableTime();
        //                                 CalculateTotalProgress();
        //                                 CalculateTotalRealisticProgress();

        //                             }, CalulationEnded);

        //}

        //private void CalulationEnded()
        //{
        //    base.Update();

        //    if (Parent != null)
        //    {
        //        Parent.Reload();
        //    }
        //}

        private double CalculateTotalBillableTime()
        {
            _totalBillableTime = Children.OfType<ListTimeEntryViewModel>().Sum(timeEntry => timeEntry.TotalBillableTime );
            CalculateTotalProgress();
            return _totalBillableTime;

        }

        private double CalculateTotalProgress()
        {

            if (Task.TimeEstimated > 0 && _totalBillableTime > 0)
            {
                return (_totalBillableTime * 100 / Task.TimeEstimated);
            }

            return 0;
        }

        private double CalculateTotalRealisticProgress()
        {

            if (Task.TimeEstimated > 0)
            {
                return ((1 - (Task.TimeLeft / Task.TimeEstimated)) * 100);
            }

            return 0;
        }

        public bool IsEstimatesEnabled
        {
            get { return Task.Project.IsEstimatesEnabled; }
        }

        //public override void LoadChildren()
        //{
        //    foreach (var timeEntry in Task.TimeEntries)
        //    {
        //        Children.Add(new ListTimeEntryViewModel(timeEntry, this, DataService));
        //    }


        //}

        public override void Reload()
        {
            base.Update();
            Parent.Reload();

        }

        public override void RemoveChild(int childId)
        {
            //Remove View
            var timeEntryViewToRemove = Children.SingleOrDefault(task => ((ListTimeEntryViewModel)task).TimeEntry.Id == childId);
            if (timeEntryViewToRemove != null)
            {
                Children.Remove(timeEntryViewToRemove);
                if (VisibleChildren.Contains(timeEntryViewToRemove))
                    VisibleChildren.Remove(timeEntryViewToRemove);
            }

            Reload();
        }

        public override ListItemModelBase AddChild(IEntity entity)
        {
            var newChild = new ListTimeEntryViewModel(entity as TimeEntry, this, DataService, UserRepository);
            Children.Add(newChild);
            AddVisibleChild(newChild);

            Reload();
            return newChild;
        }

        public override void RecieveDraggable(IDraggable draggable)
        {
            var timeEntry = draggable.Entity as TimeEntry;
            if (timeEntry == null || timeEntry.DocumentType == 2)
            {
                MessageBox.Show("You can't drop this time entry, it have already been invoiced");
                return;
            }

            var eventArgs = new MoveEntityEventArgs(timeEntry, timeEntry.Task, Task);

            timeEntry.Task = Task;
            timeEntry.TaskID = Task.Id;
            DataService.SaveTimeEntry(timeEntry);

            DataService.SaveTask(Task);

            // Update visuals
            var draggedTimeEntry = draggable as ListTimeEntryViewModel;
            if (draggedTimeEntry == null)
                return;

            draggedTimeEntry.ParentTask.RemoveChild(timeEntry.Id);
            if (Children.All(item => item.Entity.Id != timeEntry.Id))
            {
                AddChild(draggedTimeEntry.Entity);
                IsExpanded = true;
            }

            // For treeview
            InternalCommands.MoveEntityRequest.Execute(eventArgs);

            Reload();
        }

        protected override void EntityDragged(MoveEntityEventArgs moveEntityEventArgs)
        {
            var target = moveEntityEventArgs.NewParent as Task;
            var source = moveEntityEventArgs.OldParent as Task;
            var movedEntity = moveEntityEventArgs.MovedEntity as TimeEntry;

            if (target == null || source == null || movedEntity == null)
            {
                return;
            }

            if (source.Guid == Task.Guid)
            {
                RemoveChild(moveEntityEventArgs.MovedEntity.Id);
            }

            if (target.Guid == Task.Guid)
            {
                if (Children.SingleOrDefault(item => item.Entity.Id == moveEntityEventArgs.MovedEntity.Id) == null)
                {
                    AddChild(movedEntity);
                }
            }
        }

        public override bool CanPass(TimeEntryFilter filter, TreeItemSelectionFilter selectionFilter)
        {
            return selectionFilter.HasTask(Task)
                    && !(filter != null && filter.HideEmptyTasks && !Task.TimeEntries.Any(filter.CanPassFilter));
        }
    }
}