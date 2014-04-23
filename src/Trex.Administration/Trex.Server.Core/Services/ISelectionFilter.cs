using System.Collections.ObjectModel;
using Trex.Server.Core.Model;

namespace Trex.Server.Core.Services
{
    public interface ISelectionFilter
    {
        ReadOnlyCollection<Customer> Customers { get; }
        ReadOnlyCollection<Project> Projects { get; }
        ReadOnlyCollection<Task> Tasks { get; }
        void AddCustomer(Customer customer);
        void AddProject(Project project);
        void AddTask(Task task);
        void RemoveCustomer(Customer customer);
        void RemoveProject(Project project);
        void RemoveTask(Task task);
        bool HasCustomer(Customer customer);
        bool HasProject(Project project);
        bool HasTask(Task task);
        bool HasTimeEntry(TimeEntry timeEntry);
    }
}