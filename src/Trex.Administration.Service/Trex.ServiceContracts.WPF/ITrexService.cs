#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Windows.Controls;

#endregion

namespace Trex.ServiceContracts
{
    [ServiceContract(Namespace = "TrexService")]
    public interface ITrexService
    {
        [OperationContract]
        List<InvoiceComment> LoadComments(int invoiceId);

        [OperationContract]
        ServerResponse SaveComment(string comment, int invoiceID, int userID);

        [OperationContract]
        List<InvoiceListItemView> GetDebitList();

        [OperationContract]
        TimeEntry GetTimeEntryByTimeEntry(TimeEntry timeEntry);

        [OperationContract]
        InvoiceListItemView GetInvoiceByInvoiceId(int invoiceId);

        [OperationContract]
        List<int?> GetAllInvoiceIds();

        [OperationContract]
        void RecalculateInvoice(int invoiceId, DateTime startDate, DateTime endDate, int customerInvoiceGroupId);

        [OperationContract]
        void DeleteInvoiceTemplate(InvoiceTemplate template);

        [OperationContract]
        double? UpdateExclVAT(int invoiceId);

        [OperationContract]
        List<CreditNote> GetFinalizedInvoiceDataByInvoiceId(int invoiceId);

        [OperationContract]
        void SaveInvoiceTemplateFile(byte[] binaryFile, int invoiceTemplateId);

        [OperationContract]
        List<InvoiceTemplate> GetInvoiceTemplates();

        [OperationContract]
        ServerResponse OverWriteCig(CustomerInvoiceGroup cig);

        [OperationContract]
        ServerResponse SaveCIG(CustomerInvoiceGroup cig);

        [OperationContract]
        ServerResponse SaveInvoiceChanges(InvoiceListItemView invoicedata);

        [OperationContract]
        ObservableCollection<InvoiceListItemView> GetInvoicesByCustomerId(ObservableCollection<int> customerIds);

        [OperationContract]
        void GenerateInvoiceLines(int invoiceid);

        [OperationContract]
        void CreateNewInvoiceLine(int invoiceId, double vat);

        [OperationContract]
        void SetStandardInvoicePrintTemplate(int templateId);

        [OperationContract]
        void SetStandardInvoiceMailTemplate(int templateId);

        [OperationContract]
        void SetStandardSpecificationTemplate(int templateId);

        [OperationContract]
        ServerResponse SendInvoiceEmailToCustomerInvoiceGroup(Invoice invoice);

        [OperationContract]
        CustomerInvoiceGroup GetCustomerInvoiceGroupByCustomerInvoiceFGroupId(int customerInvoiceGroupId);

        [OperationContract]
        ServerResponse DeleteCustomerInvoiceGroup(int customerInvoiceGroupId);

        [OperationContract]
        List<CustomerInvoiceGroup> GetCustomerInvoiceGroupByCustomerId(int customerId);

        [OperationContract]
        ServerResponse InsertCustomerInvoiceGroup(ObservableCollection<CustomerInvoiceGroup> group);

        [OperationContract]
        Invoice ReCalculateInvoice(Invoice invoice);

        [OperationContract]
        List<TimeEntry> GetInvoiceDataByInvoiceId(int invoiceId);

        [OperationContract]
        List<InvoiceLine> GetInvoiceLinesByInvoiceId(int invoiceId);

        [OperationContract]
        List<TimeEntry> GetTimeEntriesForInvoicing(DateTime startDate, DateTime endDate, int customerId);

        [OperationContract]
        List<CustomersInvoiceView> GetCustomerInvoiceViews(DateTime startDate, DateTime endDate);

        [OperationContract]
        Customer GetCustomerById(int customerId, bool includeInactive, bool includeParents, bool includeProjects,
                                 bool includeTasks, bool includeTimeEntries);

        [OperationContract]
        Project GetProjectById(int projectId, bool includeParents, bool includeInactive, bool includeTasks,
                               bool includeTimeEntries);

        [OperationContract]
        Task GetTaskById(int taskId, bool includeParents, bool includeSubTasks, bool includeTimeEntries);

        [OperationContract]
        List<Customer> GetAllCustomers(bool includeInactive, bool includeParents, bool includeProjects,
                                       bool includeTasks, bool includeTimeEntries);

        [OperationContract]
        List<Customer> EntityCompanyRequest(List<int> customerIds, bool includeParents, bool includeInactive,
                                            bool includeProjects, bool includeTasks, bool includeTimeEntries);

        [OperationContract]
        List<Project> EntityProjectRequest(List<int> projectIds, bool includeParents, bool includeInactive,
                                           bool includeTasks, bool includeTimeEntries);

        [OperationContract]
        List<Task> EntityTaskRequest(List<int> taskIds, bool includeParents, bool includeSubTasks,
                                     bool includeTimeEntries);

        [OperationContract]
        List<TimeEntry> GetTimeEntriesByPeriodAndUser(User user, DateTime startDate, DateTime endDate);

        [OperationContract]
        List<TimeEntryView> GetTimeEntriesByPeriod(DateTime startDate, DateTime endDate);

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
        void ExcludeTimeEntry(TimeEntry timeEntry);

        [OperationContract]
        InvoiceLine SaveInvoiceLine(InvoiceLine invoiceLine);

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
        void DeleteInvoiceDraft(int invoiceDraftID);

        [OperationContract]
        bool DeleteCustomer(Customer customer);

        [OperationContract]
        bool DeleteTask(Task task);

        [OperationContract]
        bool DeleteProject(Project project);

        [OperationContract]
        bool DeleteTimeEntry(TimeEntry timeEntry);

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
        List<Role> GetRoles();

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
        List<UserPermission> GetPermissionsForSingleRole(string roleName, int applicationId);

        [OperationContract]
        void UpdatePermissions(List<UserPermission> updatedPermissions, Role role, int applicationId);

        [OperationContract]
        Invoice GetInvoiceById(int invoiceId);

        [OperationContract]
        bool ValidateUserWithCustomerId(string userName, string password, string customerId);

        [OperationContract]
        UserCreationResponse CreateUser(User user, bool sendEmail, string language);

        [OperationContract]
        bool ResetPassword(User user, string language);

        [OperationContract]
        List<InvoiceTemplate> GetAllInvoiceTemplates();

        [OperationContract]
        void SaveInvoiceTemplate(InvoiceTemplate template);

        [OperationContract]
        List<UserPermission> GetAllPermissionsByClientId(int clientId);

        [OperationContract]
        List<Customer> GetLatestWorkedOnCustomersByUser(User user, DateTime fromDate);

        [OperationContract]
        List<Project> GetLatestWorkedOnProjectsByUser(User user, DateTime fromDate);

        [OperationContract]
        List<Task> GetLatestWorkedOnTasksByUser(User user, DateTime fromDate);

        [OperationContract]
        ServerResponse GenerateInvoicesFromCustomerID(DateTime startDate, DateTime endDate, int customerId, int userId, float VAT);

        [OperationContract]
        ServerResponse GenerateCreditnote(int invoiceId, int currentUserId);

        [OperationContract]
        ServerResponse UpdateTimeEntryPrice(int projectId);

        [OperationContract]
        Customer GetCustomerByCriteria(GetCustomerByIdCriterias criterias);
    }
}
