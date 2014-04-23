using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Services;
using Trex.ServiceContracts;
using Trex.TaskAdministration.Commands;
using System.Linq;
using ViewModelBase = Trex.Core.Implemented.ViewModelBase;

namespace Trex.TaskAdministration.Dialogs.EditProjectView
{
    public interface IEditProjectViewModel
    {
        [Required]
        string ProjectName { get; set; }

        bool IsActive { get; set; }
        bool IsInActive { get; set; }
        bool IsEstimatesEnabled { get; set; }
        DelegateCommand<object> SaveProject { get; set; }
        DelegateCommand<object> CancelCommand { get; set; }
        ObservableCollection<CustomerInvoiceGroup> CustomerInvoiceGroupComboBoxItems { get; set; }
        CustomerInvoiceGroup SelectedCustomerInvoiceGroupItem { get; set; }
        int SelectedCustomerInvoiceGroupIndex { get; set; }
        void GetCustomerInvoiceGroupNames(bool isUpdate);
        void UpdateCustomerInvoiceGroupListDone(Customer obj);
        event PropertyChangedEventHandler PropertyChanged;
        void Update();
        void Close();
    }

    public class EditProjectViewModel : ViewModelBase, IEditProjectViewModel
    {
        private readonly IDataService _dataService;
        private readonly bool _isNew;

        private Project _project;
        private Project _tempProject;
        private ObservableCollection<CustomerInvoiceGroup> _customerInvoiceGroupComboBoxItems;
        private CustomerInvoiceGroup _selectedCustomerInvoiceGroupItem;
        private int _selectedCustomerInvoiceGroupIndex;
        private DelegateCommand<Customer> UpdateCustomerInvoiceGroupList;

        public EditProjectViewModel(Project project, IDataService dataService)
        {
            _project = project;
            _dataService = dataService;
            _isNew = _project.Id == 0;
            SaveProject = new DelegateCommand<object>(ExecuteSaveProject, CanSaveProject);
            CancelCommand = new DelegateCommand<object>(CancelEdit);
            UpdateCustomerInvoiceGroupList = new DelegateCommand<Customer>(UpdateCustomerInvoiceGroupListDone);

            InternalCommands.UpdateCustomerInvoiceGroupList.RegisterCommand(UpdateCustomerInvoiceGroupList);

            if (IsFixedPrice)
                EstimatedHoursEnabled = true;
            else
                EstimatedHoursEnabled = false;

            GetCustomerInvoiceGroupNames(false);
        }

        [Required]
        public string ProjectName
        {
            get { return _project.ProjectName; }
            set
            {
                _project.ProjectName = value;

                OnPropertyChanged("ProjectName");
                Validate("ProjectName", value);
            }
        }

        public bool IsActive
        {
            get { return !_project.Inactive; }
            set
            {
                _project.Inactive = !value;
                OnPropertyChanged("IsActive");
            }
        }

        public bool IsInActive
        {
            get { return _project.Inactive; }
            set
            {
                _project.Inactive = value;
                OnPropertyChanged("IsInActive");
            }
        }

        public bool IsEstimatesEnabled
        {
            get { return _project.IsEstimatesEnabled; }
            set
            {
                _project.IsEstimatesEnabled = value;
                OnPropertyChanged("IsEstimatesEnabled");
            }
        }

        public bool IsFixedPrice
        {
            get { return _project.FixedPriceProject; }
            set
            {
                _project.FixedPriceProject = value;
                OnPropertyChanged("EstimatedHoursEnabled");
                OnPropertyChanged("IsFixedPrice");
            }
        }

        public decimal? FixedPrice
        {
            get { return _project.FixedPrice; }
            set
            {
                _project.FixedPrice = value;
                if (value != null)
                {
                    EstimatedHoursEnabled = true;
                    IsFixedPrice = true;
                }
                else
                {
                    EstimatedHoursEnabled = false;
                    EstimatedHours = null;
                    IsFixedPrice = false;
                }

                OnPropertyChanged("EstimatedHoursEnabled");
                OnPropertyChanged("FixedPrice");
            }
        }

        public int? EstimatedHours
        {
            get { return _project.EstimatedHours; }
            set
            {
                _project.EstimatedHours = value;
                OnPropertyChanged("EstimatedHours");
            }
        }

        public bool EstimatedHoursEnabled { get; set; }

        public DelegateCommand<object> SaveProject { get; set; }

        public DelegateCommand<object> CancelCommand { get; set; }

        private bool CanSaveProject(object arg)
        {
            return (!String.IsNullOrEmpty(ProjectName) && SelectedCustomerInvoiceGroupItem != null);
        }

        private void CancelEdit(object obj)
        {
            InternalCommands.ProjectEditCompleted.Execute(null);
        }

        private void ExecuteSaveProject(object obj)
        {
            var customer = _project.Customer;
            var cig =
                CustomerInvoiceGroupComboBoxItems.First(
                    c => c.CustomerInvoiceGroupID == SelectedCustomerInvoiceGroupItem.CustomerInvoiceGroupID);

            _project.Customer = null;
            //_project.CustomerInvoiceGroup = null;

            _project.CustomerInvoiceGroupID = SelectedCustomerInvoiceGroupItem.CustomerInvoiceGroupID;

            _project.ChangeTracker.OriginalValues.Clear();

            _dataService.SaveProject(_project).Subscribe(
                project =>
                {

                    if (_isNew)
                    {
                        if (project == null)
                        {
                            MessageBox.Show("BUG, Can't save right now.. please try press F5 and try again");
                            InternalCommands.ProjectAddCompleted.Execute(null);
                        }
                        else
                        {
                            _project.ProjectID = project.ProjectID;
                            _project.Customer = customer;

                            _project.CustomerInvoiceGroup = cig;
                            _project.AcceptChanges();
                            InternalCommands.ProjectAddCompleted.Execute(_project);
                        }
                    }
                    else
                    {
                        _project.Customer = customer;

                        _project.CustomerInvoiceGroup = CustomerInvoiceGroupComboBoxItems.First(i => i.CustomerInvoiceGroupID == SelectedCustomerInvoiceGroupItem.CustomerInvoiceGroupID);
                        _project.AcceptChanges();
                        InternalCommands.ProjectEditCompleted.Execute(_project);
                    }
                }
                );
        }

        public ObservableCollection<CustomerInvoiceGroup> CustomerInvoiceGroupComboBoxItems
        {
            get { return _customerInvoiceGroupComboBoxItems; }
            set
            {
                _customerInvoiceGroupComboBoxItems = value;
                OnPropertyChanged("CustomerInvoiceGroupComboBoxItems");
            }
        }

        public CustomerInvoiceGroup SelectedCustomerInvoiceGroupItem
        {
            get { return _selectedCustomerInvoiceGroupItem; }
            set
            {
                _selectedCustomerInvoiceGroupItem = value;
                if (value != null && value.Label == "Add new...")
                    OpenEditCustomerInvoiceGroup();

                OnPropertyChanged("SelectedCustomerInvoiceGroupItem");
                Validate("SelectedCustomerInvoiceGroupItem", value);
            }
        }

        private void OpenEditCustomerInvoiceGroup()
        {
            InternalCommands.CustomerInvoiceGroupAddStart.Execute(_project.Customer);
        }

        public int SelectedCustomerInvoiceGroupIndex
        {
            get { return _selectedCustomerInvoiceGroupIndex; }
            set
            {
                _selectedCustomerInvoiceGroupIndex = value;
                OnPropertyChanged("SelectedCustomerInvoiceGroupIndex");
            }
        }

        public void GetCustomerInvoiceGroupNames(bool isUpdate)
        {
            var temp = new ObservableCollection<CustomerInvoiceGroup>();

            _dataService.GetCustomerInvoiceGroupByCustomerId(_project.CustomerID).Subscribe(
                i =>
                {
                    foreach (var customerInvoiceGroup in i)
                    {
                        temp.Add(customerInvoiceGroup);
                    }

                    CustomerInvoiceGroupComboBoxItems = temp;
                    CustomerInvoiceGroupComboBoxItems.Add(new CustomerInvoiceGroup { Label = "Add new..." });

                    if (_project.CustomerInvoiceGroupID != 0 && !isUpdate)
                        SelectedCustomerInvoiceGroupItem = temp.First(t => t.CustomerInvoiceGroupID == _project.CustomerInvoiceGroupID);

                    else if (isUpdate)
                        SelectedCustomerInvoiceGroupItem = temp.Last(r => r.CustomerID != 0);
                });
        }

        public void UpdateCustomerInvoiceGroupListDone(Customer obj)
        {
            GetCustomerInvoiceGroupNames(true);
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            SaveProject.RaiseCanExecuteChanged();
        }
    }
}