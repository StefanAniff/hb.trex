using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Model;
using Trex.Core.Services;
using Trex.ServiceContracts;
using Trex.TaskAdministration.Commands;
using Trex.TaskAdministration.Interfaces;
using ViewModelBase = Trex.Core.Implemented.ViewModelBase;

namespace Trex.TaskAdministration.TaskManagementScreen.TaskTreeView
{
    public class TaskTreeViewModel : ViewModelBase, ITaskTreeViewModel
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IDataService _dataService;
        private readonly IUserRepository _userRepository;
        private readonly TaskManagementFilters _fetchFilters;
        private ObservableCollection<TreeCustomerViewModel> _customers;
        private Project _searchProject;
        private Task _searchTask;
        //public event EventHandler<ProjectListEventArgs> OnGetProjectsByCustomerCompleted;

        public TaskTreeViewModel(ICustomerRepository customerRepository, IDataService dataService, IUserRepository userRepository, TaskManagementFilters fetchFilters)
        {
            _customerRepository = customerRepository;
            _customerRepository.DataLoaded += _customerRepository_DataLoaded;
            _dataService = dataService;
            _userRepository = userRepository;
            _fetchFilters = fetchFilters;
            //BindCustomerViewModels();
            //IsBusy = !_customerRepository.IsDataLoaded;
            Customers = new ObservableCollection<TreeCustomerViewModel>();
            IsLoadOnDemandEnabled = true;
            WireUpEvents();

            InternalCommands.TaskEditCompleted.RegisterCommand(new DelegateCommand<Task>(TaskEditCompleted));
            InternalCommands.ProjectEditCompleted.RegisterCommand(new DelegateCommand<Project>(ProjectEditCompleted));
            InternalCommands.TaskAddCompleted.RegisterCommand(new DelegateCommand<Task>(TaskAddCompleted));
            InternalCommands.TaskDeleteCompleted.RegisterCommand(new DelegateCommand<int?>(TaskDeleteCompleted));
            InternalCommands.ProjectDeleteCompleted.RegisterCommand(new DelegateCommand<int?>(ProjectDeleteCompleted));
            InternalCommands.ProjectAddCompleted.RegisterCommand(new DelegateCommand<Project>(ProjectAddCompleted));
            InternalCommands.CustomerDeleteCompleted.RegisterCommand(new DelegateCommand<int?>(CustomerDeleteCompleted));
            InternalCommands.CustomerEditCompleted.RegisterCommand(new DelegateCommand<Customer>(CustomerEditCompleted));
            InternalCommands.CustomerAddCompleted.RegisterCommand(new DelegateCommand<Customer>(CustomerAddCompleted));
            InternalCommands.TaskSearchCompleted.RegisterCommand(new DelegateCommand<IEntity>(TaskSearchCompleted));
            //InternalCommands.CustomerInvoiceGroupComplete.RegisterCommand(new DelegateCommand<Customer>(CustomerInvoiceGroupCompleted));

            //OnItemLoadOnDemand = new DelegateCommand<RadTreeViewItem>(ItemLoadOnDemand);
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        public bool IsLoadOnDemandEnabled { get; set; }

        #region ITaskTreeViewModel Members

        public ObservableCollection<TreeCustomerViewModel> Customers
        {
            get { return _customers; }
            private set
            {
                _customers = value;
                OnPropertyChanged("Customers");
            }
        }

        #endregion

        private void TaskSearchCompleted(IEntity obj)
        {
            if (obj is Task)
            {
                _searchTask = (Task)obj;
                var customerViewModel = Customers.SingleOrDefault(
                    treeCustomer => treeCustomer.Customer.Id == _searchTask.Project.CustomerID);

                if (customerViewModel != null)
                {
                    if (customerViewModel.Children.Count == 0)
                    {
                        customerViewModel.LoadChildrenCompleted += customerViewModel_LoadChildrenCompleted;
                        customerViewModel.IsExpanded = true;
                    }
                    else
                    {
                        ExpandTask(_searchTask);
                    }
                }
            }
            else if (obj is Project)
            {
                _searchProject = (Project)obj;
                var customerViewModel = Customers.SingleOrDefault(
                    treeCustomer => treeCustomer.Customer.Id == _searchProject.Customer.Id);

                if (customerViewModel != null)
                {
                    if (customerViewModel.Children.Count == 0)
                    {
                        customerViewModel.LoadChildrenCompleted += customerViewModel_LoadProjectChildrenCompleted;
                        customerViewModel.IsExpanded = true;
                    }
                    else
                    {
                        ExpandProject(_searchProject);
                    }
                }
            }
        }

        private void customerViewModel_LoadProjectChildrenCompleted(object sender, System.EventArgs e)
        {
            ExpandProject(_searchProject);
        }

        private void customerViewModel_LoadChildrenCompleted(object sender, System.EventArgs e)
        {
            ExpandTask(_searchTask);
        }

        private void ExpandProject(Project project)
        {
            var projectViewModel =
                Customers.SelectMany(c => c.Children).SingleOrDefault(p => ((TreeProjectViewModel)p).Project.Id == project.Id);
            if (projectViewModel != null)
            {
                projectViewModel.IsExpanded = true;
                projectViewModel.IsSelected = true;
            }
        }

        private void ExpandTask(Task task)
        {
            var taskViewModel =
                Customers.SelectMany(c => c.Children).SelectMany(p => p.Children).SingleOrDefault(
                    treeTask => treeTask is TreeTaskViewModel && ((TreeTaskViewModel)treeTask).Task.Id == task.Id);
            if (taskViewModel != null)
            {
                taskViewModel.IsExpanded = true;
                taskViewModel.IsSelected = true;
            }
        }

        private void _customerRepository_DataLoaded(object sender, System.EventArgs e)
        {
            BindCustomerViewModels();

        }

        private void CustomerAddCompleted(Customer obj)
        {
            Customers.Add(new TreeCustomerViewModel(_dataService, obj, _customerRepository, _userRepository, _fetchFilters));
        }

        private void CustomerEditCompleted(Customer obj)
        {
            if (obj == null)
            {
                return;
            }
            var customerViewModel = Customers.SingleOrDefault(
                treeCustomer => treeCustomer.Customer.Id == obj.Id);

            customerViewModel.Reload();
        }

        private void CustomerDeleteCompleted(int? customerId)
        {
            if(!customerId.HasValue)
                return;
            var customerViewModel = Customers.SingleOrDefault(
                treeCustomer => treeCustomer.Customer.Id == customerId);

            if (customerViewModel != null)
            {
                Customers.Remove(customerViewModel);
            }
        }

        private void ProjectDeleteCompleted(int? projectId)
        {
            if(!projectId.HasValue)
                return;
            

            var projectViewModel = Customers.SelectMany(c => c.Children).SingleOrDefault(
                treeProject => treeProject is TreeProjectViewModel && ((TreeProjectViewModel)treeProject).Project.Id == projectId);

            if (projectViewModel != null)
            {
                projectViewModel.Remove();
            }
        }

        private void TaskDeleteCompleted(int? taskId)
        {

            if(!taskId.HasValue)
                return;
            var taskViewModel =
                Customers.SelectMany(c => c.Children).SelectMany(p => p.Children).SingleOrDefault(
                    treeTask => treeTask is TreeTaskViewModel && ((TreeTaskViewModel)treeTask).Task.Id == taskId);

            taskViewModel.Remove();
        }

        private void TaskAddCompleted(Task obj)
        {
            var projectViewModel = Customers.SelectMany(c => c.Children).SingleOrDefault(
                treeProject => treeProject is TreeProjectViewModel && ((TreeProjectViewModel)treeProject).Project.Id == obj.ProjectID);

            if (projectViewModel != null)
            {
                projectViewModel.Children.Add(new TreeTaskViewModel(projectViewModel, obj, _dataService));
            }
        }

        private void ProjectEditCompleted(Project obj)
        {
            if(obj == null)
                return;
            var projectViewModel = Customers.SelectMany(c => c.Children).SingleOrDefault(
                treeProject => treeProject is TreeProjectViewModel && ((TreeProjectViewModel)treeProject).Project.Id == obj.Id);

            if (projectViewModel != null)
            {
                projectViewModel.Reload();
            }
        }

        private void ProjectAddCompleted(Project project)
        {
            if (project == null)
                return;
            var customerViewModel = Customers.SingleOrDefault(
                treeCustomer => treeCustomer.Customer.Id == project.CustomerID);

            if (customerViewModel != null)
            {
                customerViewModel.Children.Add(new TreeProjectViewModel(customerViewModel, project, _dataService));
            }
        }

        private void TaskEditCompleted(Task task)
        {
            if (task == null)
                return;
            var taskViewModel =
                Customers.SelectMany(c => c.Children).SelectMany(p => p.Children).SingleOrDefault(
                    treeTask => treeTask is TreeTaskViewModel && ((TreeTaskViewModel)treeTask).Task.Id == task.Id);

            if (taskViewModel != null)
            {
                taskViewModel.Reload();
            }
        }

        private void WireUpEvents()
        {
            _customerRepository.Customers.CollectionChanged += Customers_CollectionChanged;
        }

        private void Customers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        foreach (Customer customer in e.NewItems)
                        {
                            var customerViewModel = new TreeCustomerViewModel(_dataService, customer, _customerRepository, _userRepository, _fetchFilters);
                            customerViewModel.IsLoadOnDemandEnabled = IsLoadOnDemandEnabled;
                            Customers.Add(customerViewModel);
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    {
                        foreach (Customer customer in e.OldItems)
                        {
                            var customerToRemove = Customers.SingleOrDefault(viewModel => viewModel.Customer.Id == customer.Id);
                            if (customerToRemove != null)
                            {
                                Customers.Remove(customerToRemove);
                            }
                        }
                    }
                    break;
            }
        }

        private void BindCustomerViewModels()
        {
            var customers = new ObservableCollection<TreeCustomerViewModel>();
            foreach (var customer in _customerRepository.Customers.OrderBy(c=>c.CustomerName))
            {
                var customerViewModel = new TreeCustomerViewModel(_dataService, customer, _customerRepository, _userRepository, _fetchFilters)
                                            {IsLoadOnDemandEnabled = IsLoadOnDemandEnabled};
                customers.Add(customerViewModel);
            }
            Customers = customers;
            //IsBusy = false;
        }
    }
}