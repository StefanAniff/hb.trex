#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.Objects;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web.Security;
using System.Windows;
using System.Windows.Controls;
using StructureMap;
using Telerik.Reporting.Service;
using Trex.Server.Core.Interfaces;
using Trex.Server.Core.Services;
using Trex.Server.DataAccess;
using Trex.Server.Infrastructure.BaseClasses;
using Trex.Server.Infrastructure.Extensions;
using Trex.Server.Infrastructure.Implemented;
using Trex.Server.Infrastructure.ServiceBehavior;
using Trex.ServiceContracts;
using Trex.ServiceContracts.Model;
using TrexSL.Web.Exceptions;
using System.Linq;

#endregion

namespace TrexSL.Web
{
    [CustomBehavior]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class TrexSLService : LogableBase, ITrexService
    {
        #region ITrexService Members
        private readonly IInvoiceService _invoiceService;
        private readonly ITemplateService _templateService;
        private readonly ICustomerInvoiceGroupService _customerInvoiceGroupService;
        private readonly ICustomerService _customerService;
        private readonly IProjectService _projectService;
        private readonly ITaskService _taskService;
        private readonly ITimeEntryService _timeEntryService;
        private readonly IUserManagementService _userManagementService;
        private readonly IMembershipService _membershipService;
        private readonly IInvoiceSender _invoiceSender;

        public TrexSLService()
        {
            _templateService = ObjectFactory.GetInstance<ITemplateService>();
            _invoiceService = ObjectFactory.GetInstance<IInvoiceService>();
            _customerInvoiceGroupService = ObjectFactory.GetInstance<ICustomerInvoiceGroupService>();
            _customerService = ObjectFactory.GetInstance<ICustomerService>();
            _projectService = ObjectFactory.GetInstance<IProjectService>();
            _taskService = ObjectFactory.GetInstance<ITaskService>();
            _timeEntryService = ObjectFactory.GetInstance<ITimeEntryService>();
            _userManagementService = ObjectFactory.GetInstance<IUserManagementService>();
            _membershipService = ObjectFactory.GetInstance<IMembershipService>();
            _invoiceSender = ObjectFactory.GetInstance<IInvoiceSender>();
        }

        public TrexSLService(IInvoiceService invoiceService,
                             ITemplateService templateService,
                             ICustomerInvoiceGroupService customerInvoiceGroupService,
                             ICustomerService customerService,
                             IProjectService projectService,
                             ITaskService taskService,
                             ITimeEntryService timeEntryService,
                             IUserManagementService userManagementService,
                             IMembershipService membershipService,
                             IInvoiceSender invoiceSender)
        {
            _invoiceService = invoiceService;
            _templateService = templateService;
            _customerInvoiceGroupService = customerInvoiceGroupService;
            _customerService = customerService;
            _projectService = projectService;
            _taskService = taskService;
            _timeEntryService = timeEntryService;
            _userManagementService = userManagementService;
            _membershipService = membershipService;
            _invoiceSender = invoiceSender;

        }

        public double? UpdateExclVAT(int invoiceId)
        {
            try
            {
                return _invoiceService.UpdateExclVAT(invoiceId);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public List<InvoiceComment> LoadComments(int invoiceId)
        {
            try
            {
                return _invoiceService.LoadInvoiceComment(invoiceId);
            }
            catch (Exception e)
            {
                LogError(e);
                return null;
            }
        }

        public ServerResponse SaveComment(string comment, int invoiceID, int userID)
        {
            try
            {
                return _invoiceService.SaveInvoiceComment(comment, invoiceID, userID);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return new ServerResponse("Failed to save comment", false);
            }
        }

        public List<InvoiceTemplate> GetInvoiceTemplates()
        {
            try
            {
                return _templateService.GetInvoiceTemplates();
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public void SetStandardInvoiceMailTemplate(int templateId)
        {
            try
            {
                _templateService.SetStandardInvoiceMailTemplate(templateId);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        public void SetStandardInvoicePrintTemplate(int templateId)
        {
            try
            {
                _templateService.SetStandardInvoicePrintTemplate(templateId);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        public void SetStandardSpecificationTemplate(int templateId)
        {
            try
            {
                _templateService.SetStandardSpecificationTemplate(templateId);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        public void CreateNewInvoiceLine(int invoiceId, double vat)
        {
            try
            {
                _invoiceService.CreateNewInvoiceLine(invoiceId, vat);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        public void GenerateInvoiceLines(int invoiceid)
        {
            try
            {
                _invoiceService.GenerateInvoiceLines(invoiceid);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        public ServerResponse SendInvoiceEmailToCustomerInvoiceGroup(Invoice invoice)
        {
            try
            {
                _invoiceSender.SendInvoiceEmail(invoice.ID);
                _invoiceService.UpdateDeliveredDate(invoice.ID);

                return new ServerResponse("Email send", true);

            }
            catch (InvalidOperationException iex)
            {
                LogError(iex);
                return new ServerResponse("Failed to send email\nPlease check if all templates are uploaded and try again", false);

            }
            catch (Exception ex)
            {
                LogError(ex);
                return new ServerResponse("Failed to send email", false);
            }
        }

        public List<CustomerInvoiceGroup> GetCustomerInvoiceGroupByCustomerId(int customerId)
        {
            try
            {
                return _customerInvoiceGroupService.GetCustomerInvoiceGroupById(customerId);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public CustomerInvoiceGroup GetCustomerInvoiceGroupByCustomerInvoiceFGroupId(int customerInvoiceGroupId)
        {
            try
            {
                return _customerInvoiceGroupService.GetCustomerInvoiceGroupByCustomerInvoiceGroupId(customerInvoiceGroupId);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public ServerResponse InsertCustomerInvoiceGroup(ObservableCollection<CustomerInvoiceGroup> group)
        {
            try
            {
                foreach (var customerInvoiceGroup in group)
                {
                    _customerInvoiceGroupService.InsertCustomerInvoiceGroupIntoDatabase(customerInvoiceGroup);
                }

                return new ServerResponse("Data inserted succesfully", true);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return new ServerResponse("Data inserted failed", false);
            }
        }

        public ServerResponse SaveCIG(CustomerInvoiceGroup cig)
        {
            try
            {
                _customerInvoiceGroupService.InsertCustomerInvoiceGroupIntoDatabase(cig);

                return new ServerResponse("Data inserted succesfully", true);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return new ServerResponse("Data inserted failed", false);
            }
        }

        public ServerResponse OverWriteCig(CustomerInvoiceGroup cig)
        {
            try
            {
                _customerInvoiceGroupService.OverwriteCig(cig);

                return new ServerResponse("Data inserted succesfully", true);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return new ServerResponse("Data inserted failed", false);
            }
        }

        public ServerResponse DeleteCustomerInvoiceGroup(int customerInvoiceGroupId)
        {
            try
            {
                return _customerInvoiceGroupService.DeleteCustomerInvoiceGroup(customerInvoiceGroupId);
            }
            catch (Exception)
            {
                return new ServerResponse("Could not delete CustomerInvoiceGroup", false);
            }
        }

        public Invoice ReCalculateInvoice(Invoice invoice)
        {
            try
            {
                return _invoiceService.ReCalculateInvoice(invoice);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public Invoice GetInvoiceById(int invoiceId)
        {
            try
            {
                return _invoiceService.GetInvoiceById(invoiceId);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }

        }

        public InvoiceListItemView GetInvoiceByInvoiceId(int invoiceId)
        {
            try
            {
                return _invoiceService.GetInvoiceByInvoiceId(invoiceId);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public List<int?> GetAllInvoiceIds()
        {
            try
            {
                return _invoiceService.GetAllInvoiceIDs();
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public List<TimeEntry> GetInvoiceDataByInvoiceId(int invoiceId)
        {
            try
            {
                return _invoiceService.GetInvoiceDataByInvoiceId(invoiceId);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public List<CreditNote> GetFinalizedInvoiceDataByInvoiceId(int invoiceId)
        {
            try
            {
                return _invoiceService.GetFinalizedInvoiceDataByInvoiceId(invoiceId);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public List<InvoiceLine> GetInvoiceLinesByInvoiceId(int invoiceId)
        {
            try
            {
                return _invoiceService.GetInvoiceLinesByInvoiceId(invoiceId);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public ObservableCollection<InvoiceListItemView> GetInvoicesByCustomerId(ObservableCollection<int> customerIds)
        {
            try
            {
                return _invoiceService.GetInvoicesByCustomerId(customerIds);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public List<TimeEntry> GetTimeEntriesForInvoicing(DateTime startDate, DateTime endDate, int custumerInvoiceGroupId)
        {
            try
            {
                return _invoiceService.GetTimeEntriesForInvoicing(startDate, endDate, custumerInvoiceGroupId);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public ServerResponse GenerateInvoicesFromCustomerID(DateTime startDate, DateTime endDate, int customerId, int userId, float VAT)
        {
            try
            {
                _invoiceService.GenerateInvoiceDraft(startDate, endDate, customerId, userId, VAT);

                return new ServerResponse("Invoice successfully generated for customer: " + customerId, true);

            }
            catch (Exception ex)
            {
                LogError(ex);
                return new ServerResponse("Invoice generation failed for customer: " + customerId, false);
            }
        }

        public ServerResponse GenerateCreditnote(int invoiceId, int currentUserId)
        {
            try
            {
                var newId = _invoiceService.GenerateCreditNote(invoiceId, currentUserId);

                return new ServerResponse("Credit note created", true, newId);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return new ServerResponse("Credit note is not created", false);
            }
        }

        public List<CustomersInvoiceView> GetCustomerInvoiceViews(DateTime startDate, DateTime endDate)
        {
            try
            {
                return _customerService.GetCustomerInvoiceViews(startDate, endDate);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public Customer GetCustomerById(int customerId, bool includeInactive, bool includeParents, bool includeProjects, bool includeTasks, bool includeTimeEntries)
        {
            try
            {
                return _customerService.GetCustomerById(customerId, includeInactive, includeParents, includeProjects,
                                                    includeTasks, includeTimeEntries);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public Customer GetCustomerByCriteria(GetCustomerByIdCriterias criterias)
        {
            try
            {
                return _customerService.GetCustomerByCriteria(criterias);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public void SaveInvoiceTemplate(InvoiceTemplate template)
        {
            try
            {
                _templateService.SaveTemplate(template);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        public void RecalculateInvoice(int invoiceId, DateTime startDate, DateTime endDate, int customerInvoiceGroupId)
        {
            try
            {
                _invoiceService.UpdateInvoiceAttention(invoiceId, customerInvoiceGroupId);
                _invoiceService.ReleaseTimeEntries(invoiceId);
                _invoiceService.UpdateTimeEntries(invoiceId, startDate, endDate, customerInvoiceGroupId);
                var fixedProjects = _invoiceService.GenerateInvoiceLines(invoiceId);
                _invoiceService.UpdateTimeEntriesPricePrHour(fixedProjects, invoiceId);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        public void DeleteInvoiceTemplate(InvoiceTemplate template)
        {
            try
            {
                _templateService.DeleteTemplate(template);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        public void SaveInvoiceTemplateFile(byte[] binaryFile, int invoiceTemplateId)
        {
            try
            {
                _templateService.SaveTemplateFile(invoiceTemplateId, binaryFile);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        public List<InvoiceTemplate> GetAllInvoiceTemplates()
        {
            try
            {
                return _templateService.GetAllInvoiceTemplates();

            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public Project GetProjectById(int projectId, bool includeParents, bool includeInactive, bool includeTasks, bool includeTimeEntries)
        {
            try
            {
                return _projectService.GetProjectById(projectId, includeParents, includeInactive, includeTasks,
                                               includeTimeEntries);

            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public Task GetTaskById(int taskId, bool includeParents, bool includeSubTasks, bool includeTimeEntries)
        {
            try
            {
                return _taskService.GetTaskById(taskId, includeParents, includeSubTasks, includeTimeEntries);

            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public List<Customer> GetAllCustomers(bool includeInactive, bool includeParents, bool includeProjects, bool includeTasks, bool includeTimeEntries)
        {
            try
            {
                return _customerService.GetAllCustomers(includeInactive, includeParents, includeProjects, includeTasks,
                                                 includeTimeEntries);

            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public List<Customer> EntityCompanyRequest(List<int> customerIds, bool includeParents, bool includeInactive, bool includeProjects, bool includeTasks, bool includeTimeEntries)
        {
            try
            {
                return _customerService.EntityCompanyRequest(customerIds, includeParents, includeInactive,
                                                             includeProjects, includeTasks, includeTimeEntries);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public List<Project> EntityProjectRequest(List<int> projectIds, bool includeParents, bool includeInactive, bool includeTasks, bool includeTimeEntries)
        {
            try
            {
                return _projectService.EntityProjectRequest(projectIds, includeParents, includeInactive, includeTasks,
                                                            includeTimeEntries);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public List<Task> EntityTaskRequest(List<int> taskIds, bool includeParents, bool includeSubTasks, bool includeTimeEntries)
        {
            try
            {
                return _taskService.EntityTaskRequest(taskIds, includeParents, includeSubTasks, includeTimeEntries);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public List<TimeEntry> GetTimeEntriesByPeriodAndUser(User user, DateTime startDate, DateTime endDate)
        {
            try
            {
                return _timeEntryService.GetTimeEntriesByPeriodAndUser(user, startDate, endDate);

            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public List<TimeEntryView> GetTimeEntriesByPeriod(DateTime startDate, DateTime endDate)
        {
            try
            {
                return _timeEntryService.GetTimeEntriesByPeriod(startDate, endDate);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public List<User> GetAllUsers()
        {
            try
            {
                return _userManagementService.GetAllUsers();
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public User GetUserByUserName(string userName)
        {
            try
            {
                var requestContext = OperationContext.Current.Extensions.Find<ConnectionContext>();
                return _userManagementService.GetUserByUserName(userName, requestContext.ClientApplicationType);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public UserCreationResponse CreateUser(User user, bool sendEmail, string language)
        {
            var requestContext = OperationContext.Current.Extensions.Find<ConnectionContext>();
            return _userManagementService.CreateUser(user, sendEmail, language, requestContext.CustomerId);
        }

        public void SaveUser(User user)
        {
            try
            {
                _userManagementService.SaveUser(user);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        public void DeleteUser(User user)
        {
            try
            {
                _userManagementService.DeleteUser(user);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        public void DeactivateUser(User user)
        {
            try
            {
                _membershipService.DeactivateUser(user);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        public void ActivateUser(User user)
        {
            try
            {
                _membershipService.ActivateUser(user);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        public List<InvoiceListItemView> GetDebitList()
        {
            try
            {
                return _invoiceService.GetDebitList();
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }


        }

        public TimeEntry GetTimeEntryByTimeEntry(TimeEntry timeEntry)
        {
            try
            {
                return _timeEntryService.GetTimeEntryByTimeEntry(timeEntry);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public TimeEntry SaveTimeEntry(TimeEntry timeEntry)
        {
            try
            {
                return _timeEntryService.SaveTimeEntry(timeEntry);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public void ExcludeTimeEntry(TimeEntry timeEntry)
        {
            try
            {
                var invoiceId = timeEntry.InvoiceId.GetValueOrDefault();
                _timeEntryService.ExcludeTimeEntry(timeEntry);
                var invoice = _invoiceService.GetInvoiceById(invoiceId);
                _invoiceService.ReCalculateInvoice(invoice);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        public InvoiceLine SaveInvoiceLine(InvoiceLine invoiceLine)
        {
            try
            {
                return _invoiceService.SaveInvoiceLine(invoiceLine);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public Invoice SaveInvoice(Invoice invoice)
        {
            try
            {
                return _invoiceService.SaveInvoice(invoice);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }
        
        public ServerResponse SaveInvoiceChanges(InvoiceListItemView invoicedata)
        {
            try
            {
                return _invoiceService.SaveInvoiceChanges(invoicedata); ;
            }
            catch (InvalidOperationException ex)
            {
                LogError(ex);
                return new ServerResponse("tried to save, a deleted draft", false);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return new ServerResponse("Invoice was not saved", false);
            }
        }

        public Task SaveTask(Task task)
        {
            try
            {
                return _taskService.SaveTask(task);

            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public Project SaveProject(Project project)
        {
            try
            {
                return _projectService.SaveProject(project);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public Customer SaveCustomer(Customer customer)
        {
            try
            {
                return _customerService.SaveCustomer(customer);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public void DeleteInvoiceLine(int invoiceLineId)
        {
            try
            {
                _invoiceService.DeleteInvoiceLine(invoiceLineId);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        public void DeleteInvoiceDraft(int invoiceDraftId)
        {
            try
            {
                _invoiceService.DeleteInvoiceById(invoiceDraftId);
            }
            catch (InvalidOperationException ex)
            {
                LogError(ex);
            }
            catch (UpdateException ex)
            {
                LogError(ex);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        public bool DeleteCustomer(Customer customer)
        {
            try
            {
                return _customerService.DeleteCustomer(customer);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return false;
            }
        }

        public bool DeleteTask(Task task)
        {
            try
            {
                return _taskService.DeleteTask(task);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return false;
            }
        }

        public bool DeleteProject(Project project)
        {
            try
            {
                return _projectService.DeleteProject(project);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return false;
            }
        }

        public bool DeleteTimeEntry(TimeEntry timeEntry)
        {
            try
            {
                return _timeEntryService.DeleteTimeEntry(timeEntry);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return false;
            }
        }

        public List<TimeEntryType> GetGlobalTimeEntryTypes()
        {
            try
            {
                return _timeEntryService.GetGlobalTimeEntryTypes();
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public List<TimeEntryType> GetAllTimeEntryTypes()
        {
            try
            {
                return _timeEntryService.GetAllTimeEntryTypes();
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public TimeEntryType SaveTimeEntryType(TimeEntryType timeEntryType)
        {
            try
            {
                return _timeEntryService.SaveTimeEntryType(timeEntryType);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public List<Task> SearchTasks(string searchString)
        {
            try
            {
                return _taskService.SearchTasks(searchString);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public List<Project> SearchProjects(string searchString)
        {
            try
            {
                return _projectService.SearchProjects(searchString);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public ServerResponse UpdateTimeEntryPrice(int projectId)
        {
            return _timeEntryService.UpdateTimeEntryPrice(projectId);
        }

        #endregion

        #region TimeTracker Client Methods

        public bool ValidateUser(string userName, string password)
        {
            try
            {
                return Membership.ValidateUser(userName, password);
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        public bool ValidateUserWithCustomerId(string userName, string password, string customerId)
        {
            try
            {
                Membership.ApplicationName = customerId;

                var customerEntities = new TrexBaseEntities();
                var customer =
                    customerEntities.TrexCustomers.SingleOrDefault(c => c.CustomerId.ToLower() == customerId.ToLower());

                if (customer == null)
                    return false;

                if (!customer.IsActivated)
                    return false;

                return Membership.ValidateUser(userName, password);
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        public User GetUser(string userName, string password)
        {
            try
            {
                if (Membership.ValidateUser(userName, password))
                {
                    var user = GetUserByUserName(userName);
                    return user;
                }
                return null;
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        public bool PingService()
        {
            return true;
        }

        public List<Customer> GetUnsyncedCompanies(DateTime lastSyncDate)
        {
            try
            {
                var context = ObjectFactory.GetInstance<ITrexContextProvider>();
                var entityContext = context.TrexEntityContext;
                var customerList = from customers in entityContext.Customers
                                   where
                                       customers.CreateDate >= lastSyncDate ||
                                       (customers.ChangeDate != null && customers.ChangeDate >= lastSyncDate)
                                   select customers;

                return customerList.ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        public List<Project> GetUnsyncedProjects(DateTime lastSyncDate)
        {
            try
            {
                var context = ObjectFactory.GetInstance<ITrexContextProvider>();
                var entityContext = context.TrexEntityContext;

                var project = from projects in entityContext.Projects
                              where
                                  projects.CreateDate >= lastSyncDate ||
                                  (projects.ChangeDate != null && projects.ChangeDate >= lastSyncDate)
                              select projects;

                return project.ToList();

                //var projectRepository = ObjectFactory.GetInstance<IProjectRepository>();
                //return projectRepository.GetByChangeDate(lastSyncDate).ToList().ToDtoObjects(false, false, false);
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        public List<Task> GetUnsyncedTasks(DateTime lastSyncDate)
        {
            try
            {
                var context = ObjectFactory.GetInstance<ITrexContextProvider>();
                var entityContext = context.TrexEntityContext;

                var tasks = (from t in entityContext.Tasks
                             where
                                 t.CreateDate >= lastSyncDate || (t.ChangeDate != null && t.ChangeDate >= lastSyncDate)
                             select t
                            );

                return tasks.ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        public List<Task> UploadNewTasks(List<Task> tasks, User user)
        {
            try
            {
                foreach (var task in tasks)
                {
                    task.CreatedBy = user.UserID;
                    SaveTask(task);
                }

                return tasks;
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        public void UploadNewTimeEntries(List<TimeEntry> timeEntries, User user)
        {
            try
            {
                foreach (var timeentry in timeEntries)
                {
                    timeentry.User = user;
                    SaveTimeEntry(timeentry);
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            try
            {
                var userManagementService = ObjectFactory.GetInstance<IMembershipService>();

                var modelUser = GetUserByUserName(userName);

                return userManagementService.ChangePassword(modelUser, oldPassword, newPassword);
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        public bool ResetPassword(User user, string language)
        {            
            try
            {
                var userManagementService = ObjectFactory.GetInstance<IUserManagementService>();
                return userManagementService.ResetPassword(user, language);
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        public List<Role> GetRoles()
        {
            try
            {
                var roleService = ObjectFactory.GetInstance<IRoleManagementService>();
                return roleService.GetAllRoles();
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        public void UpdateUserRoles(string userName, List<string> roles)
        {
            try
            {
                var modelUser = GetUserByUserName(userName);
                var userManagementService = ObjectFactory.GetInstance<IMembershipService>();
                userManagementService.UpdateUserRoles(modelUser, roles);
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        #endregion

        #region ROLES AND PERMISSIONS

        public void CreateRole(string roleName)
        {
            try
            {
                var roleManagementService = ObjectFactory.GetInstance<IRoleManagementService>();
                roleManagementService.CreateRole(roleName);
            }
            catch (RoleException exception)
            {
                throw new FaultException<ExceptionInfo>(
                    new ExceptionInfo("Service returned an error", "Service error details"),
                    exception.Message);
            }
            catch (Exception exception)
            {
                LogError(exception);
                throw;
            }
        }

        public void DeleteRole(string roleName)
        {
            try
            {
                var roleManagementService = ObjectFactory.GetInstance<IRoleManagementService>();
                roleManagementService.DeleteRole(roleName);
            }
            catch (RoleException exception)
            {
                throw new FaultException<ExceptionInfo>(
                    new ExceptionInfo("Service returned an error", "Service error details"),
                    exception.Message);
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        public bool ExistsRole(string name)
        {
            try
            {
                return Roles.RoleExists(name);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return true;
            }
        }

        public List<UserPermission> GetAllPermissionsByClientId(int clientId)
        {
            try
            {
                var permissionService = ObjectFactory.GetInstance<IPermissionService>();
                return permissionService.GetAllPermissions(clientId);
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        public List<UserPermission> GetPermissionsForSingleRole(string roleName, int applicationId)
        {
            try
            {
                var rolesList = new List<string> { roleName };

                var permissionService = ObjectFactory.GetInstance<IPermissionService>();
                var permissions = permissionService.GetPermissionsForRoles(rolesList, applicationId).ToList();

                return permissions;
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        public void UpdatePermissions(List<UserPermission> updatedPermissions, Role role, int applicationId)
        {
            try
            {
                var permissionService = ObjectFactory.GetInstance<IPermissionService>();
                permissionService.UpdatePermissionsForRole(updatedPermissions, role, applicationId);
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        public List<Customer> GetLatestWorkedOnCustomersByUser(User user, DateTime fromDate)
        {
            try
            {
                if (user == null)
                    throw new ArgumentException("User cannot be null", "user");

                var context = ObjectFactory.GetInstance<ITrexContextProvider>();
                var entityContext = context.TrexEntityContext;

                var timeentries = entityContext.TimeEntries
                    .Where(t => t.UserID == user.UserID && t.CreateDate >= fromDate).Select(
                        t => t.Task.Project.Customer.CustomerID).Distinct();
                var customers = entityContext.Customers.Where(c => timeentries.Contains(c.CustomerID));

                return customers.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Project> GetLatestWorkedOnProjectsByUser(User user, DateTime fromDate)
        {
            try
            {
                if (user == null)
                    throw new ArgumentException("User cannot be null", "user");

                var context = ObjectFactory.GetInstance<ITrexContextProvider>();
                var entityContext = context.TrexEntityContext;

                var projectIds = entityContext.TimeEntries
                    .Where(t => t.UserID == user.UserID && t.CreateDate >= fromDate).Select(
                        t => t.Task.Project.ProjectID).Distinct();
                var projects = entityContext.Projects.Where(c => projectIds.Contains(c.ProjectID));

                return projects.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Task> GetLatestWorkedOnTasksByUser(User user, DateTime fromDate)
        {
            try
            {
                if (user == null)
                    throw new ArgumentException("User cannot be null", "user");

                var context = ObjectFactory.GetInstance<ITrexContextProvider>();
                var entityContext = context.TrexEntityContext;

                var taskIds = entityContext.TimeEntries
                    .Where(t => t.UserID == user.UserID && t.CreateDate >= fromDate).Select(t => t.Task.TaskID).Distinct
                    ();
                var tasks = entityContext.Tasks.Where(c => taskIds.Contains(c.TaskID));

                return tasks.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        private void RemoveInactive(Customer customer)
        {
            try
            {
                for (int i = customer.Projects.Count() - 1; i >= 0; i--)
                {
                    if (customer.Projects[i].Inactive)
                        customer.Projects.RemoveAt(i);
                    else
                    {
                        for (int y = customer.Projects[i].Tasks.Count() - 1; y >= 0; y--)
                        {
                            if (customer.Projects[i].Tasks[y].Inactive)
                            {
                                customer.Projects[i].Tasks.RemoveAt(y);
                            }
                        }
                        customer.Projects[i].MarkAsUnchanged();
                        customer.Projects[i].AcceptChanges();
                    }
                }

                customer.MarkAsUnchanged();
                customer.AcceptChanges();
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        //#region Inviting

        //public void SendInvitationEmails(string customerId, string role, string emails)
        //{
        //    try
        //    {
        //        var emailsTrimmed = emails.Replace(" ", "");
        //        string[] emailsList = emailsTrimmed.Split(',');

        //        var mailValidator = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
        //           @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
        //           @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

        //        foreach (string inviteeEmail in emailsList)
        //        {
        //            if (mailValidator.IsMatch(inviteeEmail))
        //            {
        //                var invitationId = Guid.NewGuid();
        //                //if (!SendInvitation(inviteeEmail, invitationId))
        //                //    throw new Exception("Failed to send invitation email");

        //                if (!SaveInvitation(inviteeEmail, role, customerId, invitationId, DateTime.Now, false))
        //                    throw new Exception("Failed to save invitation");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError(ex);
        //    }
        //}

        //private bool SaveInvitation(string inviteeEmail, string role, string customerId, Guid invitationId, DateTime invitationDate, bool isUsed)
        //{
        //    //insert invitation into invitations table

        //    return false;

        //}

        //private bool SendInvitation(string inviteeEmail, Guid invitationId)
        //{
        //    //try
        //    //{
        //    //    var sender = ConfigurationManager.AppSettings["AdminSenderEmail"];
        //    //    var smtpServer = ConfigurationManager.AppSettings["smtpServer"];
        //    //    var smtpServerPort = ConfigurationManager.AppSettings["smtpServerPort"];
        //    //    var displayName = ConfigurationManager.AppSettings["displayName"];
        //    //    var mailSubject = ConfigurationManager.AppSettings["invitationMailSubject"];
        //    //    var invitationSite = ConfigurationManager.AppSettings["invitationSite"];
        //    //    var mailTemplate = ConfigurationManager.AppSettings["invitationMailTemplate"];

        //    //    var msg = new System.Net.Mail.MailMessage();
        //    //    msg.From = new System.Net.Mail.MailAddress(sender, displayName);
        //    //    msg.To.Add(new System.Net.Mail.MailAddress(inviteeEmail));

        //    //    //---

        //    //    var result = new StringBuilder();
        //    //    var buf = new byte[8192];
        //    //    var request = (HttpWebRequest)WebRequest.Create(mailTemplate);
        //    //    var response = (HttpWebResponse)request.GetResponse();
        //    //    var responseStream = response.GetResponseStream();

        //    //    string tempString = "";
        //    //    int count = 0;

        //    //    do
        //    //    {
        //    //        count = responseStream.Read(buf, 0, buf.Length);

        //    //        if (count != 0)
        //    //        {
        //    //            tempString = Encoding.ASCII.GetString(buf, 0, count);
        //    //            result.Append(tempString);
        //    //        }
        //    //    } while (count > 0);

        //    //    //---

        //    //    msg.Body = result.ToString().Replace("[INVITATIONLINK]", invitationSite + invitationId);
        //    //    msg.Subject = mailSubject;
        //    //    msg.IsBodyHtml = true;

        //    //    var mailClient = new System.Net.Mail.SmtpClient(smtpServer, Int32.Parse(smtpServerPort));

        //    //    mailClient.Send(msg);
        //    //    return true;
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    LogError(ex);
        //    //    return false;
        //    //}
        //    return false;
        //}

        //#endregion
    }
}

