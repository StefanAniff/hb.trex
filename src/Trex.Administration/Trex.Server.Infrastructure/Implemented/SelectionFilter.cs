using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented
{
    public class SelectionFilter : ISelectionFilter
    {
        private readonly List<Customer> _selectedCustomers;
        private readonly List<Project> _selectedProjects;
        private readonly List<Task> _selectedTasks;

        public SelectionFilter()
        {
            _selectedCustomers = new List<Customer>();
            _selectedProjects = new List<Project>();
            _selectedTasks = new List<Task>();
        }

        public List<User> Users { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        #region ISelectionFilter Members

        public ReadOnlyCollection<Customer> Customers
        {
            get { return new ReadOnlyCollection<Customer>(_selectedCustomers); }
        }

        public ReadOnlyCollection<Project> Projects
        {
            get { return new ReadOnlyCollection<Project>(_selectedProjects); }
        }

        public ReadOnlyCollection<Task> Tasks
        {
            get { return new ReadOnlyCollection<Task>(_selectedTasks); }
        }

        public void AddCustomer(Customer customer)
        {
            if (!HasCustomer(customer))
            {
                _selectedCustomers.Add(customer);
            }
        }

        public void AddProject(Project project)
        {
            if (_selectedProjects.Count(p => p.Id == project.Id) == 0)
            {
                _selectedProjects.Add(project);
            }

            AddCustomer(project.Customer);
        }

        public void AddTask(Task task)
        {
            if (_selectedTasks.Count(t => t.Id == task.Id) == 0)
            {
                _selectedTasks.Add(task);
            }

            AddProject(task.Project);
        }

        public void RemoveCustomer(Customer customer)
        {
            var customerToRemove = _selectedCustomers.SingleOrDefault(c => c.Id == customer.Id);
            if (customerToRemove != null)
            {
                _selectedCustomers.Remove(customerToRemove);
            }

            foreach (var project in customer.Projects)
            {
                RemoveProject(project);
            }
        }

        public void RemoveProject(Project project)
        {
            var projectToRemove = _selectedProjects.SingleOrDefault(p => p.Id == project.Id);
            if (projectToRemove != null)
            {
                _selectedProjects.Remove(projectToRemove);
            }

            //if(_selectedProjects.Count(p => p.CustomerId == project.CustomerId) == 0)
            //    RemoveCustomer(project.Customer);
        }

        public void RemoveTask(Task task)
        {
            var taskToRemove = _selectedTasks.SingleOrDefault(t => t.Id == task.Id);
            if (taskToRemove != null)
            {
                _selectedTasks.Remove(taskToRemove);
            }
        }

        public bool HasCustomer(Customer customer)
        {
            return _selectedCustomers.Count(c => c.Id == customer.Id) > 0;
        }

        public bool HasProject(Project project)
        {
            if (_selectedProjects.Count(p => p.Id == project.Id) > 0)
            {
                return true;
            }

            if (ProjectFilterIsOn(project))
            {
                return false;
            }

            return HasCustomer(project.Customer);
        }

        public bool HasTask(Task task)
        {
            if (_selectedTasks.Count(t => t.Id == task.Id) > 0)
            {
                return true;
            }

            if (TaskFilterIsOn(task))
            {
                return false;
            }

            return HasProject(task.Project);
        }

        public bool HasTimeEntry(TimeEntry timeEntry)
        {
            if (Users != null && Users.Count > 0)
            {
                if (Users.Count(u => u.Id == timeEntry.User.Id) == 0)
                {
                    return false;
                }
            }

            if (DateFrom.HasValue)
            {
                if (timeEntry.EndTime < DateFrom.Value)
                {
                    return false;
                }
            }

            if (DateTo.HasValue)
            {
                if (timeEntry.EndTime > DateTo.Value.AddDays(1))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        private bool ProjectFilterIsOn(Project project)
        {
            return _selectedProjects.Count(p => p.Customer.Id == project.Customer.Id) > 0;
        }

        private bool TaskFilterIsOn(Task task)
        {
            return _selectedTasks.Count(t => t.Project.Id == task.Project.Id) > 0;
        }
    }
}