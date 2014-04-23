using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Trex.ServiceContracts
{
     [ServiceContract(Namespace = "TrexService")]
    public interface ITrexService
    {
        [OperationContract]
        void BookTimeEntries(int invoiceId, Dictionary<int, double> timeEntries);


        [OperationContract]
        List<InvoiceLine> GetInvoiceLinesByInvoiceId(int invoiceId);

        [OperationContract]
        List<Invoice> GetInvoicesByCustomerId(int customerId);

        [OperationContract]
        List<TimeEntry> GetTimeEntries(int customerId, int invoiceId);

        [OperationContract]
        string AutoInvoice(int customerId, int userId, DateTime endDate);

        [OperationContract]
        List<Customer> CustomersEndDateFiltered(DateTime endDate);

        [OperationContract]
        CustomerStats GetCustomerStats(int customerId, DateTime lastEntry);

        [OperationContract]
        Customer GetCustomerById(int customerId, bool includeInactive, bool includeParents, bool includeProjects, bool includeTasks, bool includeTimeEntries);

        [OperationContract]
        Project GetProjectById(int projectId, bool includeParents, bool includeInactive, bool includeTasks, bool includeTimeEntries);

        [OperationContract]
        Task GetTaskById(int taskId, bool includeParents, bool includeSubTasks, bool includeTimeEntries);

        [OperationContract]
        List<Customer> GetAllCustomers(bool includeInactive, bool includeParents, bool includeProjects, bool includeTasks, bool includeTimeEntries);

        [OperationContract]
        List<Customer> EntityCompanyRequest(List<int> customerIds, bool includeParents, bool includeInactive, bool includeProjects, bool includeTasks, bool includeTimeEntries);

        [OperationContract]
        List<Project> EntityProjectRequest(List<int> projectIds, bool includeParents, bool includeInactive, bool includeTasks, bool includeTimeEntries);

        [OperationContract]
        List<Task> EntityTaskRequest(List<int> taskIds, bool includeParents, bool includeSubTasks, bool includeTimeEntries);

        [OperationContract]
        List<TimeEntry> GetTimeEntriesByPeriodAndUser(User user, DateTime startDate, DateTime endDate);

        [OperationContract]
        List<TimeEntry> GetTimeEntriesByPeriod(DateTime startDate, DateTime endDate, bool includeParents);

        [OperationContract]
        List<User> GetAllUsers();

        [OperationContract]
        User GetUserByUserName(string userName);

        [OperationContract]
        void SaveUser(User user);

        [OperationContract]
        void DeleteUser(User user);

        [OperationContract]
        void DeactivateUser(User user);

        [OperationContract]
        void ActivateUser(User user);

        [OperationContract]
        TimeEntry SaveTimeEntry(TimeEntry timeEntry);

        [OperationContract]
        Invoice SaveInvoiceLine(InvoiceLine invoiceLine);

        [OperationContract]
        Invoice SaveInvoice(Invoice invoice);

        [OperationContract]
        Task SaveTask(Task task);

        [OperationContract]
        Project SaveProject(Project project);

        [OperationContract]
        Customer SaveCustomer(Customer customer);

        [OperationContract]
        void DeleteInvoiceLine(int invoiceLineId);

        [OperationContract]
        int DeleteCustomer(int customerId);

        [OperationContract]
        int DeleteTask(int taskId);

        [OperationContract]
        int DeleteProject(int projectId);

        [OperationContract]
        int DeleteTimeEntry(int timeEntryId);

        [OperationContract]
        List<TimeEntryType> GetGlobalTimeEntryTypes();

        [OperationContract]
        List<TimeEntryType> GetAllTimeEntryTypes();

        [OperationContract]
        TimeEntryType SaveTimeEntryType(TimeEntryType timeEntryType);

        [OperationContract]
        List<Task> SearchTasks(string searchString);

        [OperationContract]
        List<Project> SearchProjects(string searchString);

        [OperationContract]
        bool ValidateUser(string userName, string password);

        [OperationContract]
        User GetUser(string userName, string password);

        [OperationContract]
        bool PingService();

        [OperationContract]
        List<Customer> GetUnsyncedCompanies(DateTime lastSyncDate);

        [OperationContract]
        List<Project> GetUnsyncedProjects(DateTime lastSyncDate);

        [OperationContract]
        List<Task> GetUnsyncedTasks(DateTime lastSyncDate);

        [OperationContract]
        List<Task> UploadNewTasks(List<Task> tasks, User user);

        [OperationContract]
        void UploadNewTimeEntries(List<TimeEntry> timeEntries, User user);

        [OperationContract]
        bool ChangePassword(string userName, string oldPassword, string newPassword);

        [OperationContract]
        bool ResetPassword(string userName);

        [OperationContract]
        List<string> GetRoles();

        [OperationContract]
        void UpdateUserRoles(string userName, List<string> roles);

        [OperationContract]
        [FaultContract(typeof(ExceptionInfo))]
        void CreateRole(string roleName);

        [OperationContract]
        [FaultContract(typeof(ExceptionInfo))]
        void DeleteRole(string roleName);

        [OperationContract]
        bool ExistsRole(string name);

        [OperationContract]
        List<PermissionItem> GetPermissionsForSingleRole(string roleName, int applicationId);

        [OperationContract]
        void UpdatePermissions(List<PermissionItem> updatedPermissions, string roleName, int applicationId);
    }
}