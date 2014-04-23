#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using System.Windows.Printing;
using Trex.ServiceContracts;

#endregion

namespace Trex.Core.Services
{
    public interface IDataService
    {
        //IObservable<Unit> = void

        //IObservable<Unit> = void

        IObservable<ObservableCollection<InvoiceComment>> LoadInvoiceComments(int invoiceId);

        IObservable<ServerResponse> SaveInvoiceComment(string comment, int invoiceID, int userID);

        /// <summary>
        /// Return a list of all the invoices that have exceeded its due date
        /// </summary>
        /// <returns></returns>>
        IObservable<ObservableCollection<InvoiceListItemView>> GetDebitList();

        /// <summary>
        /// Finalize a draft, giving it a invoiceId, and if it set to be mailed, it will be mailed
        /// </summary>
        /// <param name="invoiceIds"></param>
        /// <param name="isPreview">If in preview mode it will not be mailed, only the files will be generated</param>
        /// <returns></returns>
        IObservable<ObservableCollection<ServerResponse>> FinalizeInvoices(ObservableCollection<int> invoiceIds, bool isPreview);

        /// <summary>
        /// Used to preview the invoice
        /// </summary>
        /// <param name="invoiceGuid"></param>
        /// <param name="format"></param>
        void PreviewInvoice(Guid invoiceGuid, int format);

        /// <summary>
        /// Recalulate an invoice draft, this is to get new timeentries to an allready created draft, or when you change the invoice period
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="customerInvoiceGroupId"></param>
        /// <returns></returns>
        IObservable<Unit> RecalculateInvoice(int invoiceId, DateTime startDate, DateTime endDate,
                                             int customerInvoiceGroupId);

        /// <summary>
        /// Delete an invoiceline
        /// </summary>
        /// <param name="invoiceLineId"></param>
        /// <returns></returns>
        IObservable<Unit> DeleteInvoiceLine(int invoiceLineId);

        /// <summary>
        /// Save the changes there have been made to an invoice
        /// </summary>
        /// <param name="invoiceData"></param>
        /// <returns></returns>
        IObservable<ServerResponse> SaveInvoiceChanges(InvoiceListItemView invoiceData);

        /// <summary>
        /// return an invoice form its ID
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        IObservable<Invoice> GetInvoiceById(int invoiceId);

        /// <summary>
        /// Recalculate the excVAT of an invoice
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        IObservable<double?> RecalculateexclVat(int invoiceId);

        /// <summary>
        /// Generate invoicelines for an invoice, from its time entries
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        IObservable<Unit> GenerateInvoiceLines(int invoiceId);

        /// <summary>
        /// returns the data from credit note, for a finalized invoice
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        IObservable<ObservableCollection<CreditNote>> GetFinalizedInvoiceDataByInvoiceId(int invoiceId);

        /// <summary>
        /// Generate a creditnote from an finalized invoice
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        IObservable<ServerResponse> GenerateCreditnote(int invoiceId, int userId);

        /// <summary>
        /// Returns the timeentries needed for an invoice in the given period
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        IObservable<ObservableCollection<TimeEntry>> GetTimeEntriesForInvoicing(DateTime startDate, DateTime endDate, int customerId);

        /// <summary>
        /// Ovwewrite a customerInvoiceGroup
        /// </summary>
        /// <param name="cig"></param>
        /// <returns></returns>
        IObservable<ServerResponse> OverWriteCig(CustomerInvoiceGroup cig);

        /// <summary>
        /// Get a list of customers, with drafts
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        IObservable<ObservableCollection<CustomersInvoiceView>> GetCustomerInvoiceViews(DateTime startDate, DateTime endDate);

        IObservable<ServerResponse> UpdateTimeEntryPrice(int projectId);

        void AutoInvoice(int customerId, int userId, DateTime endDate);

        /// <summary>
        /// Saves an invoice
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        IObservable<Invoice> SaveInvoice(Invoice invoice);

        /// <summary>
        /// Get ALL the invoices IDs
        /// </summary>
        /// <returns></returns>
        IObservable<ObservableCollection<int?>> GetAllInvoiceIDs();

        /// <summary>
        /// Get a list of all the invoices for a given customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        IObservable<ObservableCollection<InvoiceListItemView>> GetInvoicesByCustomerId(ObservableCollection<int> customerId);

        /// <summary>
        /// Get an invoice from invoiceId, NOT ID
        /// </summary>
        /// <param name="InvoiceId"></param>
        /// <returns></returns>
        IObservable<InvoiceListItemView> GetInvoicebyInvoiceID(int InvoiceId);

        /// <summary>
        /// Get a list of all customers
        /// </summary>
        /// <param name="includeParents"></param>
        /// <param name="includeInactive"></param>
        /// <param name="includeProjects"></param>
        /// <param name="includeTasks"></param>
        /// <param name="includeTimeEntries"></param>
        /// <returns></returns>
        IObservable<ObservableCollection<Customer>> GetAllCustomers(bool includeParents, bool includeInactive,
                                                                    bool includeProjects, bool includeTasks,
                                                                    bool includeTimeEntries);

        /// <summary>
        /// Get a customer from its ID
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="includeInactive"></param>
        /// <param name="includeParents"></param>
        /// <param name="includeProjects"></param>
        /// <param name="includeTasks"></param>
        /// <param name="includeTimeEntries"></param>
        /// <returns></returns>
        IObservable<Customer> GetCustomerById(int customerId, bool includeInactive, bool includeParents,
                                              bool includeProjects, bool includeTasks, bool includeTimeEntries);

        /// <summary>
        /// Generates a new empty invoice line
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="vat"></param>
        /// <returns></returns>
        IObservable<Unit> AddNewInvoiceLine(int invoiceId, double vat);

        /// <summary>
        /// Gets a customerInvoiceGroup from its ID
        /// </summary>
        /// <param name="customerInvoiceGroupId"></param>
        /// <returns></returns>
        IObservable<CustomerInvoiceGroup> GetCustomerInvoiceGroupByCustomerInvoiceGroupId(int customerInvoiceGroupId);

        /// <summary>
        /// Get a list of all users
        /// </summary>
        /// <returns></returns>
        IObservable<ObservableCollection<User>> GetAllUsers();

        /// <summary>
        /// Get a list of all roles
        /// </summary>
        /// <returns></returns>
        IObservable<ObservableCollection<Role>> GetRoles();

        /// <summary>
        /// Get a user from name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        IObservable<User> GetUserByUserName(string userName);

        /// <summary>
        /// Get all time entry types
        /// </summary>
        /// <returns></returns>
        IObservable<ObservableCollection<TimeEntryType>> GetGlobalTimeEntryTypes();

        /// <summary>
        /// Get all timeentries in a given period
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        IObservable<ObservableCollection<TimeEntryView>> GetTimeEntriesByPeriod(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Save a new timeentry type
        /// </summary>
        /// <param name="timeEntryType"></param>
        /// <returns></returns>
        IObservable<TimeEntryType> SaveTimeEntryType(TimeEntryType timeEntryType);

        /// <summary>
        /// Saves a time entry
        /// </summary>
        /// <param name="timeEntry"></param>
        /// <returns></returns>
        IObservable<TimeEntry> SaveTimeEntry(TimeEntry timeEntry);

        /// <summary>
        /// Excludes a time entry
        /// </summary>
        /// <param name="timeEntry"></param>
        /// <returns></returns>
        IObservable<Unit> ExcludeTimeEntry(TimeEntry timeEntry);

        /// <summary>
        /// saves a task
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        IObservable<Task> SaveTask(Task task);

        /// <summary>
        /// Saves a CustomerInvoiceGroups
        /// </summary>
        /// <param name="cig"></param>
        /// <returns></returns>
        IObservable<ServerResponse> SaveCustomerInvoiceGroup(CustomerInvoiceGroup cig);

        /// <summary>
        /// Saves a project
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        IObservable<Project> SaveProject(Project project);

        /// <summary>
        /// Saves a Customer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        IObservable<Customer> SaveCustomer(Customer customer);

        /// <summary>
        /// Saves a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        IObservable<Unit> SaveUser(User user);

        /// <summary>
        /// Saves an invoiceline
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        IObservable<InvoiceLine> SaveInvoiceLine(InvoiceLine line);

        /// <summary>
        /// Reset the password for a given user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        IObservable<bool> ResetPassword(User user, string language);

        /// <summary>
        /// Return a list of tasks, matching the search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        IObservable<ObservableCollection<Task>> SearchTasks(string searchString);

        /// <summary>
        /// Return a list of projects, matching the search string
        /// </summary>
        /// <param name="searchProjects"></param>
        /// <returns></returns>
        IObservable<ObservableCollection<Project>> SearchProjects(string searchProjects);

        /// <summary>
        /// Deletes a customer permanently
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        IObservable<bool> DeleteCustomer(Customer customer);

        /// <summary>
        /// Deletes a draft
        /// </summary>
        /// <param name="invoiceDraftId"></param>
        /// <returns></returns>
        IObservable<Unit> DeleteInvoiceDraft(int invoiceDraftId);

        /// <summary>
        /// Delete a task permanently
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        IObservable<bool> DeleteTask(Task task);

        /// <summary>
        /// Deletes a project permanently
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        IObservable<bool> DeleteProject(Project project);

        /// <summary>
        /// Delete a time entry permanently
        /// </summary>
        /// <param name="timeEntry"></param>
        /// <returns></returns>
        IObservable<bool> DeleteTimeEntry(TimeEntry timeEntry);

        /// <summary>
        /// Deletes an user permanently
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        IObservable<Unit> DeleteUser(User user);

        /// <summary>
        /// Deactivates an user, setting it to inactive
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        IObservable<Unit> DeactivateUser(User user);

        /// <summary>
        /// Reactives an user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        IObservable<Unit> ActivateUser(User user);

        /// <summary>
        /// Update all permissions
        /// </summary>
        /// <param name="permissions"></param>
        /// <param name="role"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IObservable<Unit> UpdatePermissions(ObservableCollection<UserPermission> permissions, Role role, int clientId);

        /// <summary>
        /// Get all the permissions for a given role
        /// </summary>
        /// <param name="role"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        IObservable<ObservableCollection<UserPermission>> GetPermissionsForRole(string role, int applicationId);

        /// <summary>
        /// Creates a new role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        IObservable<Unit> CreateRole(string role);

        /// <summary>
        /// Delete a role permanently
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        IObservable<Unit> DeleteRole(string role);

        /// <summary>
        /// Mail a new Invitation mail
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="role"></param>
        /// <param name="emailAddresses"></param>
        /// <returns></returns>
        IObservable<Unit> SendInvitationEmail(string customerId, string role, string emailAddresses);

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="sendEmail"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        IObservable<UserCreationResponse> CreateUser(User user, bool sendEmail, string language);

        /// <summary>
        /// Get a list of all templates used to generate invoice files
        /// </summary>
        /// <returns></returns>
        IObservable<ObservableCollection<InvoiceTemplate>> GetAllInvoiceTemplates();

        /// <summary>
        /// Get a time entry by a time entry, updating it with the newest data, and price
        /// </summary>
        /// <param name="timeentry"></param>
        /// <returns></returns>
        IObservable<TimeEntry> GetTimeEntryByTimeEntry(TimeEntry timeentry);

        /// <summary>
        /// Save an invoice template to the database
        /// </summary>
        /// <param name="invoiceTemplate"></param>
        /// <returns></returns>
        IObservable<Unit> SaveInvoiceTemplate(InvoiceTemplate invoiceTemplate);

        /// <summary>
        /// Saves an invoice template to the database
        /// </summary>
        /// <param name="binaryFile"></param>
        /// <param name="invoiceTemplateId"></param>
        /// <returns></returns>
        IObservable<Unit> SaveInvoiceTemplateFile(byte[] binaryFile, int invoiceTemplateId);

        /// <summary>
        /// Download a given template by its ID
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        IObservable<byte[]> Downloadtemplate(int templateId);

        /// <summary>
        /// Init the system
        /// </summary>
        /// <param name="loginSettings"></param>
        void Initialize(ILoginSettings loginSettings);

        /// <summary>
        /// generate a new draft, with time entries in the given period
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="customerID"></param>
        /// <param name="userId"></param>
        /// <param name="vat"></param>
        /// <returns></returns>
        IObservable<ServerResponse> GenerateInvoice(DateTime startDate, DateTime endDate, int customerID, int userId, float vat);

        /// <summary>
        /// Get all permissions for a given client
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IObservable<ObservableCollection<UserPermission>> GetAllPermissionsByClientId(int clientId);

        /// <summary>
        /// Get all customerinvoicegroups for a given customer, by customer ID
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        IObservable<ObservableCollection<CustomerInvoiceGroup>> GetCustomerInvoiceGroupByCustomerId(int customerId);

        /// <summary>
        /// Inserts a list of customerInvoiceGroups into the database
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        IObservable<ServerResponse> InsertCustomerInvoiceGroup(ObservableCollection<CustomerInvoiceGroup> group);

        /// <summary>
        /// Deletes a customerInvoiceGroup permanently
        /// </summary>
        /// <param name="customerInvoiceGroupId"></param>
        /// <returns></returns>
        IObservable<ServerResponse> DeleteCustomerInvoiceGroup(int customerInvoiceGroupId);

        /// <summary>
        /// Get all the invoicelines for a given invoice by invoice ID
        /// </summary>
        /// <param name="invoiceID"></param>
        /// <returns></returns>
        IObservable<ObservableCollection<InvoiceLine>> GetInvoiceLinesByInvoiceID(int invoiceID);

        /// <summary>
        /// Get a list of all timeentries for a given invoice by invoice ID
        /// </summary>
        /// <param name="invoiceID"></param>
        /// <returns></returns>
        IObservable<ObservableCollection<TimeEntry>> GetInvoiceDataByInvoiceId(int invoiceID);

        /// <summary>
        /// Mail the invoice and spec file to a customer
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        IObservable<ServerResponse> SendInvoiceMail(Invoice invoice);

        //IObservable<ServerResponse> FinalizeDraft(ObservableCollection<int> invoiceId);

        /// <summary>
        /// Deletes an invoicetemplate from the database permanently
        /// </summary>
        /// <param name="selectedTemplate"></param>
        /// <returns></returns>
        IObservable<Unit> DeleteInvoiceTemplate(InvoiceTemplate selectedTemplate);

        /// <summary>
        /// Deletes all the InvoiceFiles for a given invoice
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        IObservable<ServerResponse> DeleteInvoiceFiles(int invoiceId);

        /// <summary>
        /// Validates that the template contains all the right bookmarks
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        IObservable<ServerResponse> ValidateTemplate(Byte[] data, int TemplateType);

        IObservable<Customer> GetCustomerByCriteria(GetCustomerByIdCriterias criterias);
    }
}