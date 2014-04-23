using System;
using Trex.Core.Model;
using Trex.Core.Services;
using Trex.ServiceContracts;
using Trex.TaskAdministration.TaskManagementScreen.ButtonPanelView;

namespace Trex.TaskAdministration.TaskManagementScreen.RightPanelView
{
    public class ListTimeEntryViewModel : ListItemModelBase
    {
        private ListItemModelBase _selectedItem;
        private readonly ListTaskViewModel _parent;

        public ListTimeEntryViewModel(TimeEntry timeEntry, ListItemModelBase parent, IDataService dataService, IUserRepository userRepository)
            : base(parent, timeEntry, dataService, userRepository)
        {

            _parent = parent as ListTaskViewModel;

            ActionPanelViewFactory = new TimeEntryActionPanelViewFactory(timeEntry);
            InitUserName();
        }

        private void InitUserName()
        {
            var creator = UserRepository.GetById(TimeEntry.UserID);
            if (creator != null)
                UserName = creator.Name;
            else
            {
                UserName = string.Empty;
            }
        }

        public TimeEntry TimeEntry
        {
            get { return (TimeEntry)Entity; }
        }

        public override string DisplayName
        {
            get { return TimeEntry.Description; }
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

        public override double TotalBillableTime
        {
            get { return TimeEntry.Billable ? TimeEntry.BillableTime : 0; }
        }

        public override double TotalTimeSpent
        {
            get
            {
                return TimeEntry.TimeSpent;
            }
            set
            {
                base.TotalTimeSpent = value;
            }
        }

        public  DateTime StartDate
        {
            get { return TimeEntry.StartTime; }
        }

        public string ClientSource { get { return TimeEntry.ServiceClient.ToString(); } }

        public override DateTime CreateDate { get { return TimeEntry.CreateDate.HasValue ? TimeEntry.CreateDate.Value: DateTime.MinValue; } }


        public override bool IsSelected
        {
            get { return base.IsSelected; }
            set
            {
                if (!value)
                {
                    ParentTask.SelectedItem = null;
                }
                base.IsSelected = value;
            }
        }

        public ListTaskViewModel ParentTask
        {
            get { return _parent; }
        }

        public override ListItemModelBase AddChild(IEntity entity)
        {
            return null;
        }

        public override void Reload()
        {
            Update();
            Parent.Reload();
        }

        public override bool CanPass(TimeEntryFilter filter, TreeItemSelectionFilter selectionFilter)
        {
            return filter == null || filter.CanPassFilter(TimeEntry);
        }
    }
}