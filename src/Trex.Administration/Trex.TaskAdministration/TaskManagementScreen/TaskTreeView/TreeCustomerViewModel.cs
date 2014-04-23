using System.Windows.Media;
using Trex.Core.Model;
using Trex.Core.Services;
using Trex.Infrastructure.Commands;
using Trex.ServiceContracts;
using Trex.TaskAdministration.Commands;
using Trex.TaskAdministration.EventArgs;
using Trex.TaskAdministration.Interfaces;
using System;
using Trex.TaskAdministration.TaskManagementScreen.RightPanelView;

namespace Trex.TaskAdministration.TaskManagementScreen.TaskTreeView
{
    public class TreeCustomerViewModel : TreeViewItemViewModel, ICustomerViewModel
    {
        private readonly IDataService _dataService;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserRepository _userRepository;
        private readonly TaskManagementFilters _fetchFilters;

        private bool _isLoading;
        private bool _loaded;

        public TreeCustomerViewModel(IDataService dataService, Customer customer, ICustomerRepository customerRepository, IUserRepository userRepository, TaskManagementFilters fetchFilters)
            : base(null, customer)
        {
            _dataService = dataService;
            _customerRepository = customerRepository;
            _userRepository = userRepository;
            _fetchFilters = fetchFilters;
        }

        public override string DisplayName
        {
            get
            {
                return Customer.CustomerName + ChildrenCountDisplay;
            }
        }

        public SolidColorBrush DisplayColor
        {
            get
            {
                if (Customer.Inactive)
                    return InActiveColor;
                else
                {
                    return ActiveColor;
                }
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
                    TreeCommands.CustomerSelected.Execute(this);
                }
                if (!IsSelected)
                {
                    TreeCommands.CustomerDeselected.Execute(this);
                }
            }
        }

        #region ICustomerViewModel Members

        public Customer Customer
        {
            get { return (Customer)Entity; }
            set { Entity = value; }
        }

        #endregion

        protected override void LoadChildren()
        {
            if (!ShouldLoadChildren) 
                return;

            IsLoading = true;            
            _customerRepository.ReloadCustomer(new GetCustomerByIdCriterias
                {
                    CustomerId = Customer.CustomerID,
                    IncludeInactive = _fetchFilters.ShowInactive,
                    TimeEntryFrom = _fetchFilters.TimeEntryFilterFrom.Date,
                    TimeEntryTo = _fetchFilters.TimeEntryFilterTo.Date.AddDays(1).AddSeconds(-1), // To last second of date
                    TaskFrom = _fetchFilters.TaskFilterFrom.Date,
                    TaskTo = _fetchFilters.TaskFilterTo.Date.AddDays(1).AddSeconds(-1)            // To last second of date
                }, CustomerReloaded );
        }

        private bool ShouldLoadChildren
        {
            get { return !Loaded && !IsLoading; }
        }

        private void CustomerReloaded(Customer reloadedCustomer)
        {
            Customer = reloadedCustomer;
            foreach (var project in Customer.Projects)
            {
                var projectViewModel = new TreeProjectViewModel(this, project, _dataService)
                    {
                        IsLoadOnDemandEnabled = false,

                    };
                Children.Add(projectViewModel);
            }

            if (IsSelected)
            {
                TreeCommands.CustomerSelected.Execute(this);
            }
            IsLoadOnDemandEnabled = false;
            OnLoadChildrenCompleted();
            Loaded = true;
            IsLoading = false;
        }

        public override void AddChild(IEntity entity)
        {
            Children.Add(new TreeProjectViewModel(this, entity as Project, _dataService));;
        }

        public override void RecieveDraggable(IDraggable draggable)
        {
            var project = draggable.Entity as Project;
            var eventArgs = new MoveEntityEventArgs(project, project.Customer, Customer);
            project.Customer = Customer;
            project.CustomerID = Customer.Id;

            InternalCommands.MoveEntityRequest.Execute(eventArgs);

            _dataService.SaveProject(project).Subscribe(
                result => { }
                );
        }

        #region Lazy ListCustomerViewModel

        private ListCustomerViewModel _listCustomerViewModel;

        public ListCustomerViewModel ListCustomerViewModel
        {
            get { return _listCustomerViewModel ?? (_listCustomerViewModel = GetCustomerViewModel(Customer)); }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged("IsLoading");
            }
        }

        public bool Loaded
        {
            get { return _loaded; }
            set
            {
                _loaded = value;
                OnPropertyChanged("Loaded");
            }
        }

        private ListCustomerViewModel GetCustomerViewModel(Customer customer)
        {
            var customerViewModel = new ListCustomerViewModel(customer, null, _dataService, _userRepository);
            foreach (var project in customer.Projects)
            {
                var gridProjectViewModel = GetProjectViewModel(project, customerViewModel);
                if (gridProjectViewModel != null)
                {
                    customerViewModel.Children.Add(gridProjectViewModel);
                }
            }

            return customerViewModel;
        }

        private ListProjectViewModel GetProjectViewModel(Project project, ListItemModelBase parent)
        {
            var projectViewModel = new ListProjectViewModel(project, parent, _dataService, _userRepository);
            foreach (var task in project.Tasks)
            {                
                var taskViewModel = GetTaskViewModel(task, projectViewModel);
                if (taskViewModel != null)
                    projectViewModel.Children.Add(taskViewModel);             
            }

            return projectViewModel;
        }

        private ListTaskViewModel GetTaskViewModel(Task task, ListItemModelBase parent)
        {
            var taskViewModel = new ListTaskViewModel(task, parent, _dataService, _userRepository);
            foreach (var timeEntry in task.TimeEntries)
            {
                var timeEntryViewModel = GetTimeEntryViewModel(timeEntry, taskViewModel);
                if (timeEntryViewModel != null)
                    taskViewModel.Children.Add(timeEntryViewModel);
            }

            return taskViewModel;
        }

        private ListTimeEntryViewModel GetTimeEntryViewModel(TimeEntry timeEntry, ListTaskViewModel parent)
        {            
            return new ListTimeEntryViewModel(timeEntry, parent, _dataService, _userRepository);
        }

        #endregion

    }
}