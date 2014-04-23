using System.Collections.Generic;
using System.Linq;
using Trex.ServiceContracts;
using Trex.TaskAdministration.TaskManagementScreen.TaskTreeView;

namespace Trex.TaskAdministration.TaskManagementScreen.RightPanelView
{
    public class TreeItemSelectionFilter
    {
        private readonly List<TreeCustomerViewModel> _selectedCustomers = new List<TreeCustomerViewModel>();
        private readonly List<TreeProjectViewModel> _selectedProjects = new List<TreeProjectViewModel>();
        private readonly List<TreeTaskViewModel> _selectedTasks = new List<TreeTaskViewModel>();

        public List<TreeCustomerViewModel> SelectedCustomers
        {
            get { return _selectedCustomers; }
        }

        public List<TreeProjectViewModel> SelectedProjects
        {
            get { return _selectedProjects; }
        }

        public List<TreeTaskViewModel> SelectedTasks
        {
            get { return _selectedTasks; }
        }

        public void AddCustomer(TreeCustomerViewModel customer)
        {
            if (!HasCustomer(customer.Customer))
            {
                _selectedCustomers.Add(customer);
            }
        }

        public void AddProject(TreeProjectViewModel project)
        {
            if (_selectedProjects.Count(p => p.Project.Id == project.Project.Id) == 0)
            {
                _selectedProjects.Add(project);
            }

            AddCustomer((TreeCustomerViewModel)project.Parent);
        }

        public void AddTask(TreeTaskViewModel task)
        {
            if (_selectedTasks.Count(t => t.Task.Id == task.Task.Id) == 0)
            {
                _selectedTasks.Add(task);
            }

            AddProject((TreeProjectViewModel)task.Parent);
        }

        public void RemoveCustomer(TreeCustomerViewModel customer)
        {
            var customerToRemove = _selectedCustomers.SingleOrDefault(c => c.Customer.Id == customer.Customer.Id);
            if (customerToRemove != null)
            {
                _selectedCustomers.Remove(customerToRemove);
            }

            foreach (var project in customer.Children.Cast<TreeProjectViewModel>())
            {
                RemoveProject(project);
            }
        }

        public void RemoveProject(TreeProjectViewModel project)
        {
            var projectToRemove = _selectedProjects.SingleOrDefault(p => p.Project.Id == project.Project.Id);
            if (projectToRemove != null)
            {
                _selectedProjects.Remove(projectToRemove);
            }
        }

        public void RemoveTask(TreeTaskViewModel task)
        {
            var taskToRemove = _selectedTasks.SingleOrDefault(t => t.Task.Id == task.Task.Id);
            if (taskToRemove != null)
            {
                _selectedTasks.Remove(taskToRemove);
            }
        }

        public bool HasCustomer(Customer customer)
        {
            return _selectedCustomers.Count(c => c.Customer.Id == customer.Id) > 0;
        }

        public bool HasProject(Project project)
        {
            if (_selectedProjects.Any(x => x.Project.Id == project.Id))
                return true;

            if (ProjectFilterIsOn(project))
                return false;

            return HasCustomer(project.Customer);
        }

        public bool HasTask(Task task)
        {
            if (_selectedTasks.Count(t => t.Task.Id == task.Id) > 0)
            {
                return true;
            }

            if (TaskFilterIsOn(task))
            {
                return false;
            }

            return HasProject(task.Project);
        }

        private bool ProjectFilterIsOn(Project project)
        {
            return _selectedProjects.Count(p => p.CustomerId == project.CustomerID) > 0;
        }

        private bool TaskFilterIsOn(Task task)
        {
            return _selectedTasks.Count(t => t.ParentProject.Project.Id == task.ProjectID) > 0;
        }
    }
}