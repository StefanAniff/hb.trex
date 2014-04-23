using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Model;
using Trex.Core.Services;
using Trex.Infrastructure.Commands;
using Trex.ServiceContracts;
using Trex.TaskAdministration.Commands;
using Trex.TaskAdministration.Interfaces;
using Trex.TaskAdministration.TaskManagementScreen.TaskTreeView;
using DelegateCommand = Telerik.Windows.Controls.DelegateCommand;
using ViewModelBase = Trex.Core.Implemented.ViewModelBase;

namespace Trex.TaskAdministration.TaskManagementScreen.RightPanelView
{
    public class MainListViewModel : ViewModelBase, ITaskGridViewModel, IDisposable
    {
        private readonly IDataService _dataService;
        private readonly IUserRepository _userRepository;
        private readonly IExcelExportService _excelExportService;
        private readonly TreeItemSelectionFilter _selectionFilter;
        private ListItemModelBase _selectedItem;
        private TimeEntryFilter _timeEntryFilter;
        private double _totalBillableTime;
        private double _totalTimeSpent;
        private ObservableCollection<ListCustomerViewModel> _customers;

        public MainListViewModel(IDataService dataService, IUserRepository userRepository, TreeItemSelectionFilter selectionFilter)
        {
            TreeCommands.CustomerSelected.RegisterCommand(new DelegateCommand<TreeCustomerViewModel>(CustomerSelected));
            TreeCommands.CustomerDeselected.RegisterCommand(new DelegateCommand<TreeCustomerViewModel>(CustomerDeSelected));
            TreeCommands.ProjectSelected.RegisterCommand(new DelegateCommand<TreeProjectViewModel>(ProjectSelected));
            TreeCommands.ProjectDeSelected.RegisterCommand(new DelegateCommand<TreeProjectViewModel>(ProjectDeSelected));
            TreeCommands.TaskSelected.RegisterCommand(new DelegateCommand<TreeTaskViewModel>(TaskSelected));
            TreeCommands.TaskDeSelected.RegisterCommand(new DelegateCommand<TreeTaskViewModel>(TaskDeSelected));

            InternalCommands.UserFilterChanged.RegisterCommand(new DelegateCommand<TimeEntryFilter>(UserFilterChanged));
            InternalCommands.ItemSelected.RegisterCommand(new DelegateCommand<ListItemModelBase>(ItemSelected));

            InternalCommands.CustomerDeleteCompleted.RegisterCommand(new DelegateCommand<int?>(CustomerDeleteCompleted));
            InternalCommands.CustomerAddCompleted.RegisterCommand(new DelegateCommand<Customer>(CustomerAddCompleted));
            InternalCommands.CustomerEditCompleted.RegisterCommand(new DelegateCommand<Customer>(CustomerEditCompleted));

            InternalCommands.TaskEditCompleted.RegisterCommand(new DelegateCommand<Task>(TaskEditCompleted));
            InternalCommands.TaskAddCompleted.RegisterCommand(new DelegateCommand<Task>(TaskAddCompleted));
            InternalCommands.TaskDeleteCompleted.RegisterCommand(new DelegateCommand(TaskDeleteCompleted));

            InternalCommands.ProjectDeleteCompleted.RegisterCommand(new DelegateCommand(ProjectDeleteCompleted));
            InternalCommands.ProjectEditCompleted.RegisterCommand(new DelegateCommand<Project>(ProjectEditCompleted));
            InternalCommands.ProjectAddCompleted.RegisterCommand(new DelegateCommand<Project>(ProjectAddCompleted));

            InternalCommands.TimeEntryDeleteCompleted.RegisterCommand(new DelegateCommand(TimeEntryDeleteCompleted));
            InternalCommands.TimeEntryEditCompleted.RegisterCommand(new DelegateCommand<TimeEntry>(TimeEntryEditCompleted));

            InternalCommands.TimeEntryAddCompleted.RegisterCommand(new DelegateCommand<TimeEntry>(TimeEntryAddCompleted));            

            ApplicationCommands.RefreshData.RegisterCommand(new DelegateCommand<object>(ExecuteRefresh));

            Customers = new ObservableCollection<ListCustomerViewModel>();

            _selectionFilter = selectionFilter;
            _dataService = dataService;
            _userRepository = userRepository;

            UpdateInvoiceTemplates();
        }

        private void UpdateInvoiceTemplates()
        {
            _dataService.GetAllInvoiceTemplates().Subscribe(
                it => { TemplateList = it; });
        }

        private void ExecuteRefresh(object obj)
        {
            Customers.Clear();
        }

        private void CustomerAddCompleted(Customer customer)
        {
            if (customer == null)
            {
                return;
            }
            var customerListViewModel = new ListCustomerViewModel(customer, _selectedItem, _dataService, _userRepository);

            Customers.Add(customerListViewModel);
            SelectedItem = customerListViewModel;

        }

        private void CustomerEditCompleted(Customer customer)
        {
            if(customer == null)
                return;
            if (_selectedItem != null)
            {
                if (_selectedItem is ListCustomerViewModel)
                {
                    _selectedItem.Reload();
                }
            }
        }
        
        private void CustomerDeleteCompleted(int? customerId)
        {

            if(!customerId.HasValue)
                return;

            if (_selectedItem != null)
            {
                if (_selectedItem is ListCustomerViewModel)
                {
                    var customerToRemove = Customers.SingleOrDefault(c => c.Customer.Id == customerId);
                    Customers.Remove(customerToRemove);
                    _selectedItem = null;
                }
            }
        }

        private void ProjectAddCompleted(Project addedProject)
        {
            if (addedProject == null)
                return;

            if (_selectedItem != null)
            {
                if (_selectedItem is ListCustomerViewModel)
                {
                    var projectViewModel = _selectedItem.AddChild(addedProject);
                    _selectedItem.AddVisibleChild(projectViewModel);
                    SelectedItem = projectViewModel;
                }
            }
        }

        private void ProjectDeleteCompleted(object obj)
        {
            if (_selectedItem != null)
            {
                if (_selectedItem is ListProjectViewModel)
                {
                    _selectedItem.Remove();
                }
            }
        }

        public void ProjectSelected(TreeProjectViewModel project)
        {
            _selectionFilter.AddProject(project);

            var customerViewModel = Customers.SingleOrDefault(c => c.Customer.Id == project.CustomerId);

            if (customerViewModel != null)
            {
                var projectViewModel =
                    customerViewModel.Projects.SingleOrDefault(p => p.Project.Id == project.Project.Id);
                if (projectViewModel != null)
                {
                    customerViewModel.VisibleChildren.Remove(projectViewModel);
                }

                var projectToVisualize = customerViewModel.Projects.Single(x => x.Project.Id == project.Project.Id);
                projectToVisualize.SetAllChildrenVisibleByFilters(_timeEntryFilter, _selectionFilter);
                customerViewModel.AddVisibleChild(projectToVisualize);
            }
            else
            {
                var customer = project.ParentCustomer;
                var customerVm = customer.ListCustomerViewModel;

                var projectToVisualize = customerVm.Projects.Single(x => x.Project.Id == project.Project.Id);                
                projectToVisualize.SetAllChildrenVisibleByFilters(_timeEntryFilter, _selectionFilter);
                customerVm.AddVisibleChild(projectToVisualize);
                Customers.Add(customerVm);
            }
            OnPropertyChanged("Customers");
        }

        public void ProjectDeSelected(TreeProjectViewModel project)
        {
            _selectionFilter.RemoveProject(project);

            var customerViewModel = Customers.ToList().SingleOrDefault(c => c.Customer.Id == project.Project.CustomerID);

            if (customerViewModel != null)
            {
                var projectViewModel =
                    customerViewModel.VisibleChildren.SingleOrDefault(p => ((ListProjectViewModel)p).Project.Id == project.Project.Id);

                if (projectViewModel != null)
                {
                    customerViewModel.VisibleChildren.Remove(projectViewModel);
                }

                if (customerViewModel.VisibleChildren.Count == 0)
                {
                    Customers.Remove(customerViewModel);
                    _selectionFilter.RemoveCustomer(project.ParentCustomer);
                }
            }
        }

        private void ProjectEditCompleted(Project obj)
        {
            if(obj == null)
                return;
            if (_selectedItem != null)
            {
                if (_selectedItem is ListProjectViewModel)
                {
                    _selectedItem.Reload();
                }
            }
        }        

        private void TaskDeleteCompleted(object obj)
        {
            if (_selectedItem != null)
            {
                if (_selectedItem is ListTaskViewModel)
                {
                    _selectedItem.Remove();
                }
            }
        }

        private void TaskAddCompleted(Task obj)
        {
            if (_selectedItem != null)
            {
                if (_selectedItem is ListProjectViewModel)
                {   
                    var taskChild = _selectedItem.AddChild(obj);
                    _selectedItem.AddVisibleChild(taskChild);
                    SelectedItem = taskChild;
                }
            }
        }

        private void TaskEditCompleted(Task obj)
        {
            if(obj == null)
                return;
            if (_selectedItem != null)
            {
                if (_selectedItem is ListTaskViewModel)
                {
                    _selectedItem.Reload();
                }
            }
        }

        private void TimeEntryAddCompleted(TimeEntry obj)
        {
            if (_selectedItem != null)
            {
                if (_selectedItem is ListTaskViewModel)
                {
                    var timeEntryChild = _selectedItem.AddChild(obj);
                    _selectedItem.AddVisibleChild(timeEntryChild);
                    SelectedItem = timeEntryChild;
                }
            }
        }

        private void TimeEntryEditCompleted(TimeEntry obj)
        {
            if(obj == null)
                return;
            if (_selectedItem != null)
            {
                if (_selectedItem is ListTimeEntryViewModel)
                {
                    _selectedItem.Reload();
                }
            }
        }

        private void TimeEntryDeleteCompleted(object obj)
        {
            if (_selectedItem != null)
            {
                if (_selectedItem is ListTimeEntryViewModel)
                {
                    _selectedItem.Remove();             
                }
            }
        }

        private void ItemSelected(ListItemModelBase obj)
        {


            if (_selectedItem != null)
            {
                if (_selectedItem.Equals(obj))
                    return;

                _selectedItem.IsSelected = false;
            }
            _selectedItem = obj;
            if (_selectedItem != null)
                _selectedItem.IsSelected = true;
        }

        public ListItemModelBase SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");
                InternalCommands.ItemSelected.Execute(_selectedItem);
            }
        }

        public void CustomerSelected(TreeCustomerViewModel treeCustomerVm)
        {
            var customer = treeCustomerVm.Customer;

            _selectionFilter.AddCustomer(treeCustomerVm);
            var customerViewModel = Customers.ToList().SingleOrDefault(c => c.Customer.Id == customer.Id);
            if (customerViewModel != null)
            {
                Customers.Remove(customerViewModel);
            }

            var newCustomerViewModel = treeCustomerVm.ListCustomerViewModel;

            newCustomerViewModel.SetAllChildrenVisibleByFilters(_timeEntryFilter, _selectionFilter);
            Customers.Add(newCustomerViewModel);            
        }

        public void CustomerDeSelected(TreeCustomerViewModel treeCustomerVm)
        {
            _selectionFilter.RemoveCustomer(treeCustomerVm);
            var customerViewModel = Customers.ToList().SingleOrDefault(c => c.Customer.Id == treeCustomerVm.Customer.Id);
            if (customerViewModel == null) 
                return;

            customerViewModel.SetAllChildrenNonVisible();
            Customers.Remove(customerViewModel);
        }

        public void TaskSelected(TreeTaskViewModel task)
        {   
            _selectionFilter.AddTask(task);
            var customerViewModel = Customers.SingleOrDefault(c => c.Customer.CustomerID == task.Task.Project.CustomerID);

            if (customerViewModel != null) // Customer already in list
            {
                var projectViewModel =
                    customerViewModel.Projects.Single(p => p.Project.Id == task.Task.ProjectID);

                if (!customerViewModel.VisibleChildren.Contains(projectViewModel))
                    customerViewModel.AddVisibleChild(projectViewModel);

                var taskVm = projectViewModel.Tasks.Single(x => x.Task.Id == task.Task.Id);                
                taskVm.SetAllChildrenVisibleByFilters(_timeEntryFilter, _selectionFilter);
                projectViewModel.AddVisibleChild(taskVm);
            }
            else // Customer not in list. Add it
            {
                // Customer
                var customerVm = task.ParentProject.ParentCustomer.ListCustomerViewModel;

                // Project
                var projectVm = customerVm.Projects.Single(x => x.Project.Id == task.ParentProject.Project.Id);
                projectVm.SetAllChildrenNonVisible();
                customerVm.AddVisibleChild(projectVm);

                // Task
                var taskVm = projectVm.Tasks.Single(x => x.Task.Id == task.Task.Id);
                taskVm.SetAllChildrenVisibleByFilters(_timeEntryFilter, _selectionFilter);
                projectVm.AddVisibleChild(taskVm);

                Customers.Add(customerVm);
            }
            OnPropertyChanged("Customers");
        }

        private void TaskDeSelected(TreeTaskViewModel task)
        {
            _selectionFilter.RemoveTask(task);

            var customerViewModel = Customers.ToList().SingleOrDefault(c => c.Customer.Id == task.Task.Project.CustomerID);

            if (customerViewModel != null)
            {
                var projectViewModel =
                    customerViewModel.Children.SingleOrDefault(p => ((ListProjectViewModel)p).Project.Id == task.Task.ProjectID);

                if (projectViewModel != null)
                {
                    var taskViewModel =
                        projectViewModel.Children.SingleOrDefault(t => ((ListTaskViewModel)t).Task.Id == task.Task.Id);

                    if (taskViewModel != null)
                    {
                        projectViewModel.VisibleChildren.Remove(taskViewModel);
                    }

                    if (projectViewModel.VisibleChildren.Count == 0)
                    {
                        customerViewModel.VisibleChildren.Remove(projectViewModel);
                        _selectionFilter.RemoveProject(task.ParentProject);
                    }

                    if (customerViewModel.VisibleChildren.Count == 0)
                    {
                        Customers.Remove(customerViewModel);
                        _selectionFilter.RemoveCustomer(task.ParentProject.ParentCustomer);
                    }
                }
            }
            //UpdateTotalTimeBillable();
        }

        public void UserFilterChanged(TimeEntryFilter timeEntryFilter)
        {
            _timeEntryFilter = timeEntryFilter;
            ReloadView();
        }

        public void ReloadView()
        {
            Customers = null;

            var newCustomers = new ObservableCollection<ListCustomerViewModel>();
            foreach (var customer in _selectionFilter.SelectedCustomers)
            {
                newCustomers.Add(customer.ListCustomerViewModel);
                customer.ListCustomerViewModel.SetAllChildrenVisibleByFilters(_timeEntryFilter, _selectionFilter);
            }
            Customers = newCustomers;
        }

        #region Properties

        public DelegateCommand<TimeEntry> EditTimeEntryClick { get; set; }

        public ObservableCollection<ListCustomerViewModel> Customers
        {
            get { return _customers; }
            set
            {
                _customers = value;
                OnPropertyChanged("Customers");
            }
        }

        public string TotalTimeBillable
        {
            get { return _totalBillableTime.ToString("N2"); }
        }

        public string TotalTimeSpent
        {
            get { return _totalTimeSpent.ToString("N2"); }
        }

        public bool IsEstimatesEnabled
        {
            get
            {
                var isEstimatesEnabled = Customers.SelectMany(c => c.Customer.Projects).Any(p => p.IsEstimatesEnabled);
                return isEstimatesEnabled;
            }
        }

        public ObservableCollection<InvoiceTemplate> TemplateList { get; set; }

        #endregion

        public void Dispose()
        {

        }
    }
}