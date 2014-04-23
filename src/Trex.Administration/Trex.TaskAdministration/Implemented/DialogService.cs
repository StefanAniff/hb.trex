#region

using System;
using System.Linq;
using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Trex.Core.Services;
using Trex.ServiceContracts;
using Trex.TaskAdministration.Commands;
using Trex.TaskAdministration.Dialogs.EditCustomerInvoiceGroupView;
using Trex.TaskAdministration.Dialogs.EditCustomerTimeEntryTypesView;
using Trex.TaskAdministration.Dialogs.EditCustomerView;
using Trex.TaskAdministration.Dialogs.EditProjectView;
using Trex.TaskAdministration.Dialogs.EditTaskView;
using Trex.TaskAdministration.Dialogs.EditTimeEntryTypeView;
using Trex.TaskAdministration.Dialogs.EditTimeEntryView;
using Trex.TaskAdministration.Interfaces;
using Trex.TaskAdministration.Resources;
using DelegateCommand = Telerik.Windows.Controls.DelegateCommand;

#endregion

namespace Trex.TaskAdministration.Implemented
{
    public class DialogService : IDialogService
    {
        private readonly IDataService _dataService;
        private readonly IEstimateService _estimateService;
        private readonly IUserRepository _users;
        private readonly IUserSession _userSession;
        private EditCustomerInvoiceGroupView _civ;

        public DialogService(IDataService dataService, IUserSession userSession, IEstimateService estimateService, IUserRepository users)
        {
            _dataService = dataService;
            _userSession = userSession;
            _estimateService = estimateService;
            _users = users;

            InternalCommands.TaskDeleteStart.RegisterCommand(new DelegateCommand<Task>(TaskDeleteStart));
            InternalCommands.ProjectDeleteStart.RegisterCommand(new DelegateCommand<Project>(ProjectDeleteStart));
            InternalCommands.ProjectAddStart.RegisterCommand(new DelegateCommand<Customer>(ProjectAddStart));
            InternalCommands.TimeEntryDeleteStart.RegisterCommand(new DelegateCommand<TimeEntry>(TimeEntryDeleteStart));
            InternalCommands.TimeEntryEditStart.RegisterCommand(new DelegateCommand<TimeEntry>(TimeEntryEditStart));
            InternalCommands.TimeEntryAddStart.RegisterCommand(new DelegateCommand<Task>(TimeEntryAddStart));
            InternalCommands.TaskEditStart.RegisterCommand(new DelegateCommand<Task>(TaskEditStart));
            InternalCommands.TaskAddStart.RegisterCommand(new DelegateCommand<Project>(TaskAddStart));
            InternalCommands.ProjectEditStart.RegisterCommand(new DelegateCommand<Project>(ProjectEditStart));
            InternalCommands.CustomerEditStart.RegisterCommand(new DelegateCommand<Customer>(CustomerEditStart));
            InternalCommands.CustomerDeleteStart.RegisterCommand(new DelegateCommand<Customer>(CustomerDeleteStart));
            InternalCommands.CustomerAddStart.RegisterCommand(new DelegateCommand<object>(CustomerAddStart));
            InternalCommands.CreateTimeEntryTypeStart.RegisterCommand(
                new DelegateCommand<Customer>(CreateTimeEntryTypeStart));
            InternalCommands.EditTimeEntryTypeStart.RegisterCommand(
                new DelegateCommand<TimeEntryType>(EditTimeEntryTypeStart));
            InternalCommands.EditCustomerTimeEntryTypesStart.RegisterCommand(
                new DelegateCommand<Customer>(EditCustomerTimeEntryTypesStart));
            InternalCommands.CustomerInvoiceGroupComplete.RegisterCommand(
                new DelegateCommand(CustomerInvoiceGroupAddCompleted));

            InternalCommands.CustomerInvoiceGroupAddStart.RegisterCommand(
                new DelegateCommand<Customer>(CustomerInvoiceGroupAddStart));
        }

        private void TimeEntryAddStart(Task obj)
        {

            var newTimeEntry = new TimeEntry
                                   {
                                       TimeEntryID = 0,
                                       Task = obj,
                                       TaskID = obj.TaskID,
                                       UserID = _userSession.CurrentUser.UserID,
                                       User = _userSession.CurrentUser,
                                       ServiceClient = ServiceClients.AdministrationClient,
                                       Price = _userSession.CurrentUser.Price
                                   };

            _dataService.GetTimeEntryByTimeEntry(newTimeEntry)
                .Subscribe(t =>
                {
                    var timeEntryEditViewModel = new EditTimeEntryViewModel(t, t.Task, _dataService);
                    var editView = new EditTimeEntryViewWindow { ViewModel = (timeEntryEditViewModel) };
                    editView.Show();

                });

        }

        private void TimeEntryEditStart(TimeEntry timeEntry)
        {
            _dataService.GetTimeEntryByTimeEntry(timeEntry).Subscribe(t =>
            {
                var timeEntryEditViewModel = new EditTimeEntryViewModel(t, t.Task, _dataService);
                var editView = new EditTimeEntryViewWindow();
                editView.ViewModel = (timeEntryEditViewModel);
                editView.Show();

            });


        }

        private void EditCustomerTimeEntryTypesStart(Customer customer)
        {
            var editCustomerTimeEntryTypesView = new EditCustomerTimeEntryTypesView();
            var editCustomerTimeEntryTypesViewModel = new EditCustomerTimeEntryTypesViewModel(_dataService, customer);
            editCustomerTimeEntryTypesView.ViewModel = (editCustomerTimeEntryTypesViewModel);
            editCustomerTimeEntryTypesView.Show();
        }

        private void TimeEntryDeleteStart(TimeEntry obj)
        {
            var confirmBox = MessageBox.Show(TextResources.ConfirmDeleteTimeEntryLabel,
                                             TextResources.ConfirmDeleteBoxTitle,
                                             MessageBoxButton.OKCancel);

            if (confirmBox == MessageBoxResult.OK)
            {
                obj.MarkAsDeleted();
                obj.ChangeTracker.ChangeTrackingEnabled = false;
                obj.ChangeTracker.OriginalValues.Clear();
                obj.Task = null;
                _dataService.DeleteTimeEntry(obj).Subscribe(
                    timeEntryId =>
                    {
                        obj.AcceptChanges();
                        InternalCommands.TimeEntryDeleteCompleted.Execute(obj.TimeEntryID);
                    }
                    );
            }
        }

        private void ProjectDeleteStart(Project obj)
        {
            var confirmBox = MessageBox.Show(TextResources.ConfirmDeleteProjectLabel,
                                             TextResources.ConfirmDeleteBoxTitle,
                                             MessageBoxButton.OKCancel);

            if (confirmBox == MessageBoxResult.OK)
            {
                obj.MarkAsDeleted();
                obj.Customer = null;
                obj.ChangeTracker.OriginalValues.Clear();
                _dataService.DeleteProject(obj).Subscribe(
                    projectId =>
                    {
                        obj.AcceptChanges();
                        InternalCommands.ProjectDeleteCompleted.Execute(obj.ProjectID);
                    }
                    );
            }
        }

        private void TaskDeleteStart(Task obj)
        {
            var confirmBox = MessageBox.Show(TextResources.ConfirmDeleteTaskLabel, TextResources.ConfirmDeleteBoxTitle,
                                             MessageBoxButton.OKCancel);

            if (confirmBox == MessageBoxResult.OK)
            {
                obj.MarkAsDeleted();
                obj.Project = null;
                obj.ChangeTracker.OriginalValues.Clear();
                _dataService.DeleteTask(obj).Subscribe(
                    taskId =>
                    {
                        obj.AcceptChanges();
                        InternalCommands.TaskDeleteCompleted.Execute(obj.TaskID);
                    }
                    );
            }
        }

        private void ProjectAddStart(Customer customer)
        {
            var project = new Project
                              {
                                  CreatedBy = _userSession.CurrentUser.UserID,
                                  User = _userSession.CurrentUser,
                                  CreateDate = DateTime.Now,
                                  CustomerID = customer.Id,
                                  Customer = customer,
                                  CustomerInvoiceGroupID = customer.CustomerInvoiceGroups.First(x => x.DefaultCig).CustomerInvoiceGroupID
                              };

            var projectEditView = new EditProjectViewWindow();

            var projectEditViewModel = new EditProjectViewModel(project, _dataService);
            projectEditView.ViewModel = (projectEditViewModel);
            projectEditView.Show();
        }

        private void ProjectEditStart(Project obj)
        {
            obj.ChangeTracker.ChangeTrackingEnabled = false;
            obj.Tasks = null;
            obj.ChangeTracker.ChangeTrackingEnabled = true;
            var projectEditViewModel = new EditProjectViewModel(obj, _dataService);
            var projectEditView = new EditProjectViewWindow();
            projectEditView.ViewModel = (projectEditViewModel);
            projectEditView.Show();
        }

        private void TaskAddStart(Project obj)
        {
            var task = new Task
                           {
                               ProjectID = obj.ProjectID,
                               CreatedBy = _userSession.CurrentUser.UserID,
                               Project = obj,
                               CreateDate = DateTime.Now
                           };

            var taskEditViewModel = new EditTaskViewModel(task, _dataService, _estimateService);
            var taskEditView = new EditTaskViewWindow();
            taskEditView.ViewModel = (taskEditViewModel);

            taskEditView.Show();
        }

        private void CustomerAddStart(object obj)
        {
            var customer = new Customer
                               {
                                   CreatedBy = _userSession.CurrentUser.UserID,
                                   CreateDate = DateTime.Now,
                                   InheritsTimeEntryTypes = true,
                                   User = _userSession.CurrentUser,
                               };
            var customerEditViewModel = new EditCustomerViewModel(customer, _dataService, _users);
            var customerEditView = new EditCustomerViewWindow();
            customerEditView.ViewModel = (customerEditViewModel);

            customerEditView.Show();
        }

        private void CustomerEditStart(Customer obj)
        {
            var customerEditViewModel = new EditCustomerViewModel(obj, _dataService, _users);
            var customerEditView = new EditCustomerViewWindow();
            customerEditView.ViewModel = (customerEditViewModel);

            customerEditView.Show();
        }

        private void CustomerDeleteStart(Customer obj)
        {
            var confirmBox = MessageBox.Show(TextResources.ConfirmDeleteCustomerLabel,
                                             TextResources.ConfirmDeleteBoxTitle,
                                             MessageBoxButton.OKCancel);

            if (confirmBox == MessageBoxResult.OK)
            {
                obj.MarkAsDeleted();
                _dataService.DeleteCustomer(obj).Subscribe(
                    success =>
                    {
                        if (success)
                        {
                            obj.AcceptChanges();
                            InternalCommands.CustomerDeleteCompleted.Execute(obj.CustomerID);
                        }
                        else
                        {
                            obj.CancelChanges();
                        }
                    });
            }
        }



        private void TaskEditStart(Task obj)
        {
            var taskEditViewModel = new EditTaskViewModel(obj, _dataService, _estimateService);
            var taskEditView = new EditTaskViewWindow();
            taskEditView.ViewModel = (taskEditViewModel);
            taskEditView.Show();
        }



        private void EditTimeEntryTypeStart(TimeEntryType timeEntryType)
        {
            var editTimeEntryTypeView = new EditTimeEntryTypeView();
            var editTimeEntryTypeViewModel = new EditTimeEntryTypeViewModel(_dataService, timeEntryType);
            editTimeEntryTypeView.ViewModel = (editTimeEntryTypeViewModel);
            editTimeEntryTypeView.Show();
        }

        private void CreateTimeEntryTypeStart(Customer customer)
        {
            var timeEntryType = new TimeEntryType();
            if (customer != null)
            {
                timeEntryType.Customer = customer;
                customer.TimeEntryTypes.Add(timeEntryType);
                timeEntryType.CustomerId = customer.Id;
            }

            var editTimeEntryTypeView = new EditTimeEntryTypeView();
            var editTimeEntryTypeViewModel = new EditTimeEntryTypeViewModel(_dataService, timeEntryType);
            editTimeEntryTypeView.ViewModel = (editTimeEntryTypeViewModel);
            editTimeEntryTypeView.Show();
        }

        private void CustomerInvoiceGroupAddStart(Customer customer)
        {
            _civ = new EditCustomerInvoiceGroupView();
            var customerInvoiceGroupViewModel = new EditCustomerInvoiceGroupViewModel(customer, _dataService);
            _civ.ViewModel = (customerInvoiceGroupViewModel);
            _civ.Show();
        }

        private void CustomerInvoiceGroupAddCompleted(object obj)
        {
            if (_civ != null)
                _civ.Close();
        }
    }
}