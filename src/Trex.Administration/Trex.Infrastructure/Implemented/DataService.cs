#region

using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.ServiceModel;
using Trex.Core.Interfaces;
using Trex.Core.Services;
using Trex.ServiceContracts;
using IFileDownloadService = Trex.ServiceModel.FileDownloadService.IFileDownloadService;
using ITrexService = Trex.ServiceModel.Model.ITrexService;

#endregion

namespace Trex.Infrastructure.Implemented
{
    public class DataService : IDataService
    {
        private Func<int, IObservable<ObservableCollection<InvoiceComment>>> _loadInvoiceComment; 
        private Func<string, int, int, IObservable<ServerResponse>> _saveInvoiceComment; 
        private Func<IObservable<ObservableCollection<InvoiceListItemView>>> _getDebitList;
        private Func<int, IObservable<Unit>> _generateInvoiceLines;
        private Func<ObservableCollection<int>, IObservable<ObservableCollection<InvoiceListItemView>>> _getInvoicesByCustomerId;
        private Func<int, IObservable<Unit>> _deleteInvoiceLine;

        private Func<int, IObservable<Invoice>> _getInvoiceById;
        private Func<int, IObservable<ObservableCollection<InvoiceLine>>> _getInvoiceLinesByInvoiceID;

        private Func<int, IObservable<ObservableCollection<TimeEntry>>> _getInvoiceDataByInvoiceId;
        private Func<int, IObservable<ObservableCollection<CreditNote>>> _getFinalizedInvoiceDataByInvoiceId;
        private Func<IObservable<ObservableCollection<int?>>> _getAllInvoiceIDs;
        private Func<int, int, IObservable<ServerResponse>> _generateCreditnote;

        private Func<int, IObservable<InvoiceListItemView>> _getInvoicebyInvoiceID;

        private Func<InvoiceListItemView, IObservable<ServerResponse>> _saveInvoiceChanges;

        private Func<Invoice, IObservable<ServerResponse>> _sendInvoiceMail;

        private Func<int, IObservable<Double?>> _RecalculateexclVat;

        private readonly IUserSettingsService _userSettingsService;
        private readonly IDataLoadingNotifier _loadingNotifier;

        private Func<int, double, IObservable<Unit>> _addNewInvoiceLine;
        private Func<int, IObservable<CustomerInvoiceGroup>> _getCustomerInvoiceGroupByCustomerInvoiceGroupId;
        private Func<User, IObservable<Unit>> _activateUser;
        private Func<string, IObservable<Unit>> _createRole;
        private Func<User, bool, string, IObservable<UserCreationResponse>> _createUser;
        private Func<User, IObservable<Unit>> _deactivateUser;
        private Func<Customer, IObservable<bool>> _deleteCustomer;
        private Func<int, IObservable<ServerResponse>> _deleteCustomerInvoiceGroup;

        private Func<int, IObservable<Unit>> _deleteInvoiceDraft;

        private Func<Project, IObservable<bool>> _deleteProject;
        private Func<string, IObservable<Unit>> _deleteRole;
        private Func<Task, IObservable<bool>> _deleteTask;
        private Func<TimeEntry, IObservable<bool>> _deleteTimeEntry;
        private Func<User, IObservable<Unit>> _deleteUser;

        private Func<DateTime, DateTime, int, int, float, IObservable<ServerResponse>> _generateInvoice;

        private Func<bool, bool, bool, bool, bool, IObservable<ObservableCollection<Customer>>> _getAllCustomers;
        private Func<IObservable<ObservableCollection<InvoiceTemplate>>> _getAllInvoiceTemplates;
        private Func<int, IObservable<ObservableCollection<UserPermission>>> _getAllPermissionsByClientId;
        private Func<IObservable<ObservableCollection<User>>> _getAllUsers;
        private Func<int, bool, bool, bool, bool, bool, IObservable<Customer>> _getCustomerById;
        private Func<GetCustomerByIdCriterias, IObservable<Customer>> _getCustomerByCriterias;
        private Func<int, IObservable<ObservableCollection<CustomerInvoiceGroup>>> _getCustomerInvoiceGroupByCustomerId;
        private Func<DateTime, DateTime, IObservable<ObservableCollection<CustomersInvoiceView>>> _getCustomerInvoiceViews;
        private Func<IObservable<ObservableCollection<TimeEntryType>>> _getGlobalTimeEntryTypes;
        private Func<string, int, IObservable<ObservableCollection<UserPermission>>> _getPermissionsForRole;
        private Func<IObservable<ObservableCollection<Role>>> _getRoles;
        private Func<DateTime, DateTime, IObservable<ObservableCollection<TimeEntryView>>> _getTimeEntriesByPeriod;
        private Func<DateTime, DateTime, int, IObservable<ObservableCollection<TimeEntry>>> _getTimeEntriesForInvoicing;
        private Func<string, IObservable<User>> _getUserByUserName;

        private Func<ObservableCollection<CustomerInvoiceGroup>, IObservable<ServerResponse>>
            _insertCustomerInvoiceGroup;

        private ILoginSettings _loginSettings;
        private Func<User, string, IObservable<bool>> _resetPassword;
        private Func<Customer, IObservable<Customer>> _saveCustomer;
        private Func<Invoice, IObservable<Invoice>> _saveInvoice;

        private Func<CustomerInvoiceGroup, IObservable<ServerResponse>> _saveCustomerInvoiceGroup;
        private Func<CustomerInvoiceGroup, IObservable<ServerResponse>> _overWriteCig;

        private Func<InvoiceLine, IObservable<InvoiceLine>> _saveInvoiceLine;

        private Func<InvoiceTemplate, IObservable<Unit>> _saveInvoiceTemplate;
        private Func<byte[], int, IObservable<Unit>> _saveInvoiceTemplateFile;

        private Func<Project, IObservable<Project>> _saveProject;
        private Func<Task, IObservable<Task>> _saveTask;
        private Func<TimeEntry, IObservable<TimeEntry>> _saveTimeEntry;
        private Func<TimeEntry, IObservable<Unit>> _excludeTimeEntry;
        private Func<TimeEntryType, IObservable<TimeEntryType>> _saveTimeEntryType;
        private Func<User, IObservable<Unit>> _saveUser;
        private Func<string, IObservable<ObservableCollection<Project>>> _searchProjects;
        private Func<string, IObservable<ObservableCollection<Task>>> _searchTasks;
        private Func<string, string, string, IObservable<Unit>> _sendInvitationEmail;

        private ITrexService _service;

        private Func<ObservableCollection<UserPermission>, Role, int, IObservable<Unit>> _updatePermissions;
        private Func<InvoiceTemplate, IObservable<Unit>> _deleteInvoiceTemplate;

        private Func<int, DateTime, DateTime, int, IObservable<Unit>> _recalculateInvoice;

        private Func<TimeEntry, IObservable<TimeEntry>> _getTimeEntryByTimeEntry;

        private Func<int, IObservable<ServerResponse>> _UpdateTimeEntryPrice;

        public DataService(IUserSettingsService userSettingsService, IDataLoadingNotifier loadingNotifier)
        {
            _userSettingsService = userSettingsService;
            _loadingNotifier = loadingNotifier;
        }

        public IUserSession UserSession { get; set; }

        #region IDataService Members

        public void Initialize(ILoginSettings loginSettings)
        {
            _loginSettings = loginSettings;
            _service = ServiceFactory.GetServiceClient(loginSettings);

            _loadInvoiceComment = Observable.FromAsyncPattern<int, ObservableCollection<InvoiceComment>>(_service.BeginLoadComments, _service.EndLoadComments);
                _saveInvoiceComment = Observable.FromAsyncPattern<string, int, int, ServerResponse>(_service.BeginSaveComment, _service.EndSaveComment);

            _getDebitList = Observable.FromAsyncPattern<ObservableCollection<InvoiceListItemView>>(_service.BeginGetDebitList,
                                                                                                   _service.EndGetDebitList);

            _recalculateInvoice = Observable.FromAsyncPattern<int, DateTime, DateTime, int>(_service.BeginRecalculateInvoice, _service.EndRecalculateInvoice);

            _RecalculateexclVat = Observable.FromAsyncPattern<int, double?>(_service.BeginUpdateExclVAT,
                                                                            _service.EndUpdateExclVAT);

            _getInvoiceById = Observable.FromAsyncPattern<int, Invoice>(_service.BeginGetInvoiceById, _service.EndGetInvoiceById);

            _saveInvoiceChanges = Observable.FromAsyncPattern<InvoiceListItemView, ServerResponse>(_service.BeginSaveInvoiceChanges, _service.EndSaveInvoiceChanges);

            _generateInvoiceLines = Observable.FromAsyncPattern<int>(_service.BeginGenerateInvoiceLines, _service.EndGenerateInvoiceLines);

            _getInvoicebyInvoiceID = Observable.FromAsyncPattern<int, InvoiceListItemView>(_service.BeginGetInvoiceByInvoiceId,
                                                                                _service.EndGetInvoiceByInvoiceId);

            _sendInvoiceMail = Observable.FromAsyncPattern<Invoice, ServerResponse>(_service.BeginSendInvoiceEmailToCustomerInvoiceGroup, _service.EndSendInvoiceEmailToCustomerInvoiceGroup);

            _addNewInvoiceLine = Observable.FromAsyncPattern<int, double>(_service.BeginCreateNewInvoiceLine, _service.EndCreateNewInvoiceLine);

            _generateCreditnote = Observable.FromAsyncPattern<int, int, ServerResponse>(_service.BeginGenerateCreditnote,
                                                                        _service.EndGenerateCreditnote);

            _getAllInvoiceIDs = Observable.FromAsyncPattern<ObservableCollection<int?>>(_service.BeginGetAllInvoiceIds, _service.EndGetAllInvoiceIds);

            _getAllUsers = Observable.FromAsyncPattern<ObservableCollection<User>>(_service.BeginGetAllUsers, _service.EndGetAllUsers);

            _getCustomerInvoiceGroupByCustomerInvoiceGroupId =
                Observable.FromAsyncPattern<int, CustomerInvoiceGroup>(
                    _service.BeginGetCustomerInvoiceGroupByCustomerInvoiceFGroupId,
                    _service.EndGetCustomerInvoiceGroupByCustomerInvoiceFGroupId);

            _overWriteCig = Observable.FromAsyncPattern<CustomerInvoiceGroup, ServerResponse>(_service.BeginOverWriteCig, _service.EndOverWriteCig);

            _getInvoiceDataByInvoiceId = Observable.FromAsyncPattern<int, ObservableCollection<TimeEntry>>(_service.BeginGetInvoiceDataByInvoiceId, _service.EndGetInvoiceDataByInvoiceId);

            _getFinalizedInvoiceDataByInvoiceId =
                Observable.FromAsyncPattern<int, ObservableCollection<CreditNote>>(
                    _service.BeginGetFinalizedInvoiceDataByInvoiceId, _service.EndGetFinalizedInvoiceDataByInvoiceId);

            _deleteInvoiceDraft = Observable.FromAsyncPattern<int>(_service.BeginDeleteInvoiceDraft, _service.EndDeleteInvoiceDraft);

            _getInvoiceLinesByInvoiceID = Observable.FromAsyncPattern<int, ObservableCollection<InvoiceLine>>(
                    _service.BeginGetInvoiceLinesByInvoiceId, _service.EndGetInvoiceLinesByInvoiceId);

            _getCustomerInvoiceGroupByCustomerId = Observable.FromAsyncPattern<int, ObservableCollection<CustomerInvoiceGroup>>(
                    _service.BeginGetCustomerInvoiceGroupByCustomerId, _service.EndGetCustomerInvoiceGroupByCustomerId);

            _insertCustomerInvoiceGroup = Observable.FromAsyncPattern<ObservableCollection<CustomerInvoiceGroup>, ServerResponse>(
                    _service.BeginInsertCustomerInvoiceGroup, _service.EndInsertCustomerInvoiceGroup);

            _deleteCustomerInvoiceGroup = Observable.FromAsyncPattern<int, ServerResponse>(_service.BeginDeleteCustomerInvoiceGroup,
                                                                 _service.EndDeleteCustomerInvoiceGroup);

            _generateInvoice = Observable.FromAsyncPattern<DateTime, DateTime, int, int, float, ServerResponse>(
                    _service.BeginGenerateInvoicesFromCustomerID,
                    _service.EndGenerateInvoicesFromCustomerID);

            _deleteInvoiceLine = Observable.FromAsyncPattern<int>(_service.BeginDeleteInvoiceLine, _service.EndDeleteInvoiceLine);

            _getUserByUserName = Observable.FromAsyncPattern<string, User>(_service.BeginGetUserByUserName, _service.EndGetUserByUserName);

            _getAllCustomers = Observable.FromAsyncPattern<bool, bool, bool, bool, bool, ObservableCollection<Customer>>(
                    _service.BeginGetAllCustomers, _service.EndGetAllCustomers);

            _getRoles = Observable.FromAsyncPattern<ObservableCollection<Role>>(_service.BeginGetRoles, _service.EndGetRoles);

            _updatePermissions = Observable.FromAsyncPattern<ObservableCollection<UserPermission>, Role, int>(_service.BeginUpdatePermissions, _service.EndUpdatePermissions);

            _getPermissionsForRole = Observable.FromAsyncPattern<string, int, ObservableCollection<UserPermission>>(
                    _service.BeginGetPermissionsForSingleRole, _service.EndGetPermissionsForSingleRole);

            _createRole = Observable.FromAsyncPattern<string>(_service.BeginCreateRole, _service.EndCreateRole);

            _deleteRole = Observable.FromAsyncPattern<string>(_service.BeginDeleteRole, _service.EndDeleteRole);

            _createUser = Observable.FromAsyncPattern<User, bool, string, UserCreationResponse>(_service.BeginCreateUser, _service.EndCreateUser);

            _deactivateUser = Observable.FromAsyncPattern<User>(_service.BeginDeactivateUser, _service.EndDeactivateUser);

            _activateUser = Observable.FromAsyncPattern<User>(_service.BeginActivateUser, _service.EndActivateUser);

            _deleteUser = Observable.FromAsyncPattern<User>(_service.BeginDeleteUser, _service.EndDeleteUser);



            _saveCustomerInvoiceGroup = Observable.FromAsyncPattern<CustomerInvoiceGroup, ServerResponse>(_service.BeginSaveCIG,
                                                                                        _service.EndSaveCIG);
            _saveInvoiceLine = Observable.FromAsyncPattern<InvoiceLine, InvoiceLine>(_service.BeginSaveInvoiceLine,
                                                                                     _service.EndSaveInvoiceLine);

            _saveUser = Observable.FromAsyncPattern<User>(_service.BeginSaveUser, _service.EndSaveUser);

            _saveCustomer = Observable.FromAsyncPattern<Customer, Customer>(_service.BeginSaveCustomer, _service.EndSaveCustomer);

            _saveProject = Observable.FromAsyncPattern<Project, Project>(_service.BeginSaveProject, _service.EndSaveProject);

            _getGlobalTimeEntryTypes = Observable.FromAsyncPattern<ObservableCollection<TimeEntryType>>(_service.BeginGetGlobalTimeEntryTypes,
                                                                                 _service.EndGetGlobalTimeEntryTypes);

            _saveTimeEntry = Observable.FromAsyncPattern<TimeEntry, TimeEntry>(_service.BeginSaveTimeEntry, _service.EndSaveTimeEntry);

            _excludeTimeEntry = Observable.FromAsyncPattern<TimeEntry>(_service.BeginExcludeTimeEntry, _service.EndExcludeTimeEntry);

            _getCustomerById = Observable.FromAsyncPattern<int, bool, bool, bool, bool, bool, Customer>(_service.BeginGetCustomerById,
                                                                                         _service.EndGetCustomerById);

            _getCustomerByCriterias =
                Observable.FromAsyncPattern<GetCustomerByIdCriterias, Customer>(_service.BeginGetCustomerByCriteria, _service.EndGetCustomerByCriteria);

            _saveTask = Observable.FromAsyncPattern<Task, Task>(_service.BeginSaveTask, _service.EndSaveTask);

            _deleteTimeEntry = Observable.FromAsyncPattern<TimeEntry, bool>(_service.BeginDeleteTimeEntry, _service.EndDeleteTimeEntry);

            _deleteProject = Observable.FromAsyncPattern<Project, bool>(_service.BeginDeleteProject, _service.EndDeleteProject);

            _deleteTask = Observable.FromAsyncPattern<Task, bool>(_service.BeginDeleteTask, _service.EndDeleteTask);

            _deleteCustomer = Observable.FromAsyncPattern<Customer, bool>(_service.BeginDeleteCustomer, _service.EndDeleteCustomer);
            
            _saveTimeEntryType = Observable.FromAsyncPattern<TimeEntryType, TimeEntryType>(_service.BeginSaveTimeEntryType, _service.EndSaveTimeEntryType);

            _getInvoicesByCustomerId = Observable.FromAsyncPattern<ObservableCollection<int>, ObservableCollection<InvoiceListItemView>>(
                    _service.BeginGetInvoicesByCustomerId, _service.EndGetInvoicesByCustomerId);

            _saveInvoice = Observable.FromAsyncPattern<Invoice, Invoice>(_service.BeginSaveInvoice, _service.EndSaveInvoice);

            _resetPassword = Observable.FromAsyncPattern<User, string, bool>(_service.BeginResetPassword, _service.EndResetPassword);

            _searchTasks = Observable.FromAsyncPattern<string, ObservableCollection<Task>>(_service.BeginSearchTasks, _service.EndSearchTasks);

            _searchProjects = Observable.FromAsyncPattern<string, ObservableCollection<Project>>(_service.BeginSearchProjects,
                                                                                   _service.EndSearchProjects);

            _getTimeEntriesByPeriod = Observable.FromAsyncPattern<DateTime, DateTime, ObservableCollection<TimeEntryView>>(
                    _service.BeginGetTimeEntriesByPeriod, _service.EndGetTimeEntriesByPeriod);

            _getCustomerInvoiceViews = Observable.FromAsyncPattern<DateTime, DateTime, ObservableCollection<CustomersInvoiceView>>(
                    _service.BeginGetCustomerInvoiceViews, _service.EndGetCustomerInvoiceViews);

            _getTimeEntriesForInvoicing = Observable.FromAsyncPattern<DateTime, DateTime, int, ObservableCollection<TimeEntry>>(
                    _service.BeginGetTimeEntriesForInvoicing, _service.EndGetTimeEntriesForInvoicing);

            _getAllInvoiceTemplates = Observable.FromAsyncPattern<ObservableCollection<InvoiceTemplate>>(
                    _service.BeginGetAllInvoiceTemplates, _service.EndGetAllInvoiceTemplates);

            _saveInvoiceTemplate = Observable.FromAsyncPattern<InvoiceTemplate>(_service.BeginSaveInvoiceTemplate, _service.EndSaveInvoiceTemplate);

            _saveInvoiceTemplateFile = Observable.FromAsyncPattern<byte[], int>(_service.BeginSaveInvoiceTemplateFile, _service.EndSaveInvoiceTemplateFile);

            _deleteInvoiceTemplate = Observable.FromAsyncPattern<InvoiceTemplate>(_service.BeginDeleteInvoiceTemplate, _service.EndDeleteInvoiceTemplate);

            _getAllPermissionsByClientId = Observable.FromAsyncPattern<int, ObservableCollection<UserPermission>>(
                    _service.BeginGetAllPermissionsByClientId, _service.EndGetAllPermissionsByClientId);


            _getTimeEntryByTimeEntry = Observable.FromAsyncPattern<TimeEntry, TimeEntry>(_service.BeginGetTimeEntryByTimeEntry,
                                                                      _service.EndGetTimeEntryByTimeEntry);

            _UpdateTimeEntryPrice = Observable.FromAsyncPattern<int, ServerResponse>(
                _service.BeginUpdateTimeEntryPrice, _service.EndUpdateTimeEntryPrice);

        }

        public IObservable<ObservableCollection<InvoiceListItemView>> GetDebitList()
        {
            return _getDebitList().ObserveOnDispatcher();
        }

        public IObservable<ObservableCollection<int?>> GetAllInvoiceIDs()
        {
            return _getAllInvoiceIDs().ObserveOnDispatcher();
        }

        public IObservable<ObservableCollection<InvoiceComment>> LoadInvoiceComments(int invoiceId)
        {
            return _loadInvoiceComment(invoiceId).ObserveOnDispatcher();
        }
           

        public IObservable<ServerResponse> SaveInvoiceComment(string comment, int invoiceID, int userID)
        {
            return _saveInvoiceComment(comment, invoiceID, userID).ObserveOnDispatcher();
        }

        public IObservable<double?> RecalculateexclVat(int invoiceId)
        {
            return _RecalculateexclVat(invoiceId).ObserveOnDispatcher();
        }

        public IObservable<ServerResponse> OverWriteCig(CustomerInvoiceGroup cig)
        {
            return _overWriteCig(cig).ObserveOnDispatcher();
        }

        public IObservable<Invoice> GetInvoiceById(int invoiceId)
        {
            return _getInvoiceById(invoiceId).ObserveOnDispatcher();
        }

        public IObservable<Unit> RecalculateInvoice(int invoiceId, DateTime startDate, DateTime endDate, int customerInvoiceGroupId)
        {
            NotifyLoading();
            var observable = _recalculateInvoice(invoiceId, startDate, endDate, customerInvoiceGroupId);
            SubscribeForNotifaction(observable);
            return observable;
        }        

        public IObservable<ServerResponse> SaveInvoiceChanges(InvoiceListItemView invoiceData)
        {
            return _saveInvoiceChanges(invoiceData).ObserveOnDispatcher();
        }

        public IObservable<Unit> GenerateInvoiceLines(int invoiceId)
        {
            return _generateInvoiceLines(invoiceId).ObserveOnDispatcher();
        }

        public IObservable<Unit> AddNewInvoiceLine(int invoiceId, double vat)
        {
            return _addNewInvoiceLine(invoiceId, vat);
        }

        public IObservable<ServerResponse> SendInvoiceMail(Invoice invoice)
        {
            return _sendInvoiceMail(invoice);
        }

        public IObservable<InvoiceListItemView> GetInvoicebyInvoiceID(int InvoiceId)
        {
            return _getInvoicebyInvoiceID(InvoiceId).ObserveOnDispatcher();
        }

        public IObservable<CustomerInvoiceGroup> GetCustomerInvoiceGroupByCustomerInvoiceGroupId(int customerInvoiceGroupId)
        {
            return _getCustomerInvoiceGroupByCustomerInvoiceGroupId(customerInvoiceGroupId);
        }

        public IObservable<ObservableCollection<TimeEntry>> GetInvoiceDataByInvoiceId(int invoiceId)
        {
            NotifyLoading();
            var observable = _getInvoiceDataByInvoiceId(invoiceId).ObserveOnDispatcher();
            SubscribeForNotifaction(observable);
            return observable;
        }

        public IObservable<ObservableCollection<CreditNote>> GetFinalizedInvoiceDataByInvoiceId(int invoiceId)
        {
            NotifyLoading();
            var observable = _getFinalizedInvoiceDataByInvoiceId(invoiceId).ObserveOnDispatcher();
            SubscribeForNotifaction(observable);
            return observable;
        }

        public IObservable<ObservableCollection<InvoiceLine>> GetInvoiceLinesByInvoiceID(int invoiceId)
        {
            return _getInvoiceLinesByInvoiceID(invoiceId).ObserveOnDispatcher();
        }

        public IObservable<ServerResponse> DeleteCustomerInvoiceGroup(int customerInvoiceGroupId)
        {
            return _deleteCustomerInvoiceGroup(customerInvoiceGroupId).ObserveOnDispatcher();
        }

        public IObservable<ServerResponse> InsertCustomerInvoiceGroup(ObservableCollection<CustomerInvoiceGroup> group)
        {
            return _insertCustomerInvoiceGroup(group).ObserveOnDispatcher();
        }

        public IObservable<ServerResponse> GenerateCreditnote(int invoiceId, int userId)
        {
            return _generateCreditnote(invoiceId, userId).ObserveOnDispatcher();
        }

        public IObservable<ObservableCollection<CustomerInvoiceGroup>> GetCustomerInvoiceGroupByCustomerId(int customerId)
        {
            return _getCustomerInvoiceGroupByCustomerId(customerId).ObserveOnDispatcher();
        }

        public IObservable<ServerResponse> GenerateInvoice(DateTime startDate, DateTime endDate, int customerID, int userId, float vat)
        {
            return _generateInvoice(startDate, endDate, customerID, userId, vat).ObserveOnDispatcher();
        }

        public IObservable<Unit> DeleteInvoiceLine(int invoiceLineId)
        {
            return _deleteInvoiceLine(invoiceLineId).ObserveOnDispatcher();
        }

        public IObservable<Unit> DeleteInvoiceDraft(int invoiceDraftId)
        {
            NotifyLoading();
            var observable = _deleteInvoiceDraft(invoiceDraftId).ObserveOnDispatcher();
            SubscribeForNotifaction(observable);
            return observable;
        }

        public IObservable<Unit> CreateRole(string role)
        {
            return _createRole(role).ObserveOnDispatcher();
        }

        public IObservable<Unit> DeleteRole(string role)
        {
            return _deleteRole(role).ObserveOnDispatcher();
        }

        public IObservable<ObservableCollection<UserPermission>> GetPermissionsForRole(string role, int applicationId)
        {
            return _getPermissionsForRole(role, applicationId).ObserveOnDispatcher();
        }

        public IObservable<Unit> UpdatePermissions(ObservableCollection<UserPermission> permissions, Role role,
                                                   int clientId)
        {
            return _updatePermissions(permissions, role, clientId).ObserveOnDispatcher();
        }

        public IObservable<ObservableCollection<CustomersInvoiceView>> GetCustomerInvoiceViews(DateTime startDate, DateTime endDate)
        {
            NotifyLoading();
            var observable = _getCustomerInvoiceViews(startDate, endDate).ObserveOnDispatcher();
            SubscribeForNotifaction(observable);
            return observable;
        }

        public IObservable<ObservableCollection<TimeEntry>> GetTimeEntriesForInvoicing(DateTime startDate,
                                                                                       DateTime endDate, int customerId)
        {
            return _getTimeEntriesForInvoicing(startDate, endDate, customerId).ObserveOnDispatcher();
        }

        public void AutoInvoice(int customerId, int userId, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public IObservable<ServerResponse> SaveCustomerInvoiceGroup(CustomerInvoiceGroup cig)
        {
            return _saveCustomerInvoiceGroup(cig).ObserveOnDispatcher();
        }

        public IObservable<Invoice> SaveInvoice(Invoice invoice)
        {
            NotifyLoading();
            var observable = _saveInvoice(invoice).ObserveOnDispatcher();
            SubscribeForNotifaction(observable);
            return observable;
        }

        public IObservable<ObservableCollection<InvoiceListItemView>> GetInvoicesByCustomerId(ObservableCollection<int> customerId)
        {
            NotifyLoading();
            var observable = _getInvoicesByCustomerId(customerId).ObserveOnDispatcher();
            SubscribeForNotifaction(observable);
            return observable;
        }

        public IObservable<ObservableCollection<Customer>> GetAllCustomers(bool includeParents, bool includeInactive,
                                                                           bool includeProjects, bool includeTasks,
                                                                           bool includeTimeEntries)
        {
            return
                _getAllCustomers(includeInactive, includeParents, includeProjects, includeTasks, includeTimeEntries).
                    ObserveOnDispatcher();
        }

        public IObservable<Customer> GetCustomerById(int customerId, bool includeInactive, bool includeParents,
                                                     bool includeProjects, bool includeTasks, bool includeTimeEntries)
        {
            return _getCustomerById(customerId, includeInactive, includeParents, includeProjects, includeTasks,
                                 includeTimeEntries).ObserveOnDispatcher();
        }

        public IObservable<Customer> GetCustomerByCriteria(GetCustomerByIdCriterias criterias)
        {
            return _getCustomerByCriterias(criterias).ObserveOnDispatcher();
        }

        public IObservable<ObservableCollection<User>> GetAllUsers()
        {
            return _getAllUsers().ObserveOnDispatcher();
        }

        public IObservable<ObservableCollection<Role>> GetRoles()
        {
            return _getRoles().ObserveOnDispatcher();
        }

        public IObservable<User> GetUserByUserName(string userName)
        {
            _service = null;
            _service = ServiceFactory.GetServiceClient(_userSettingsService.GetSettings());

            return _getUserByUserName(userName).ObserveOnDispatcher();
        }

        public IObservable<ObservableCollection<TimeEntryType>> GetGlobalTimeEntryTypes()
        {
            return _getGlobalTimeEntryTypes().ObserveOnDispatcher();
        }

        public IObservable<ObservableCollection<TimeEntryView>> GetTimeEntriesByPeriod(DateTime startDate, DateTime endDate)
        {
            return _getTimeEntriesByPeriod(startDate, endDate).ObserveOnDispatcher();
        }

        public IObservable<TimeEntryType> SaveTimeEntryType(TimeEntryType timeEntryType)
        {
            return _saveTimeEntryType(timeEntryType).ObserveOnDispatcher();
        }

        public IObservable<TimeEntry> SaveTimeEntry(TimeEntry timeEntry)
        {
            return _saveTimeEntry(timeEntry).ObserveOnDispatcher();
        }

        public IObservable<Unit> ExcludeTimeEntry(TimeEntry timeEntry)
        {
            return _excludeTimeEntry(timeEntry).ObserveOnDispatcher();
        }

        public IObservable<InvoiceLine> SaveInvoiceLine(InvoiceLine line)
        {
            return _saveInvoiceLine(line).ObserveOnDispatcher();
        }

        public IObservable<Task> SaveTask(Task task)
        {
            return _saveTask(task).ObserveOnDispatcher();
        }

        public IObservable<Project> SaveProject(Project project)
        {
            return _saveProject(project).ObserveOnDispatcher();
        }

        public IObservable<Customer> SaveCustomer(Customer customer)
        {
            return _saveCustomer(customer).ObserveOnDispatcher();
        }

        public IObservable<Unit> SaveUser(User user)
        {
            return _saveUser(user).ObserveOnDispatcher();
        }

        public IObservable<bool> ResetPassword(User user, string language)
        {
            return _resetPassword(user, language).ObserveOnDispatcher();
        }

        public IObservable<UserCreationResponse> CreateUser(User user, bool sendEmail, string language)
        {
            return _createUser(user, sendEmail, language).ObserveOnDispatcher();
        }

        public IObservable<ObservableCollection<Task>> SearchTasks(string searchString)
        {
            return _searchTasks(searchString).ObserveOnDispatcher();
        }

        public IObservable<ObservableCollection<Project>> SearchProjects(string searchProjects)
        {
            return _searchProjects(searchProjects).ObserveOnDispatcher();
        }

        public IObservable<bool> DeleteCustomer(Customer customer)
        {
            return _deleteCustomer(customer).ObserveOnDispatcher();
        }

        public IObservable<bool> DeleteTask(Task task)
        {
            return _deleteTask(task).ObserveOnDispatcher();
        }

        public IObservable<bool> DeleteProject(Project project)
        {
            return _deleteProject(project).ObserveOnDispatcher();
        }

        public IObservable<bool> DeleteTimeEntry(TimeEntry timeEntry)
        {
            return _deleteTimeEntry(timeEntry).ObserveOnDispatcher();
        }

        public IObservable<Unit> DeleteUser(User user)
        {
            return _deleteUser(user).ObserveOnDispatcher();
        }

        public IObservable<Unit> DeactivateUser(User user)
        {
            return _deactivateUser(user).ObserveOnDispatcher();
        }

        public IObservable<Unit> ActivateUser(User user)
        {
            return _activateUser(user).ObserveOnDispatcher();
        }

        public IObservable<TimeEntry> GetTimeEntryByTimeEntry(TimeEntry timeentry)
        {
            return _getTimeEntryByTimeEntry(timeentry).ObserveOnDispatcher();
        }

        public IObservable<ObservableCollection<UserPermission>> GetAllPermissionsByClientId(int clientId)
        {
            return _getAllPermissionsByClientId(clientId).ObserveOnDispatcher();
        }

        public IObservable<Unit> SendInvitationEmail(string customerId, string role, string emailAddresses)
        {
            return _sendInvitationEmail(customerId, role, emailAddresses).ObserveOnDispatcher();
        }

        public IObservable<ObservableCollection<ServerResponse>> FinalizeInvoices(ObservableCollection<int> invoiceIds, bool isPreview)
        {
            try
            {
                var client = ServiceFactory.GetFileDownloadClient(_loginSettings);

                var method = Observable.FromAsyncPattern<ObservableCollection<int>, bool, ObservableCollection<ServerResponse>>(client.BeginFinalizeInvoices,
                                                                                            client.EndFinalizeInvoices);
                NotifyLoading();
                var observable = method(invoiceIds, isPreview).ObserveOnDispatcher();
                SubscribeForNotifaction(observable);
                return observable;
            }
            catch (Exception)
            {
                throw new ArgumentException("Failed in DataService.cs, at FinalizeInvoices(List<int> invoiceIds)");
            }
        }

        public IObservable<ServerResponse> UpdateTimeEntryPrice(int projectId)
        {
            return _UpdateTimeEntryPrice(projectId).ObserveOnDispatcher();
        }

        

        /// <summary>
        /// Preview an invoice
        /// </summary>
        /// <param name="invoiceGuid">Invoice's Guid</param>
        /// <param name="format">1 = invoice/credit note in mail, 2 = invoice/credit note in print, 3 = specification in mail</param>
        public void PreviewInvoice(Guid invoiceGuid, int format)
        {
            try
            {
                var customerId = _loginSettings.CustomerId;
                var channel = ServiceFactory.GetFileDownloadChannel();
                var t = channel.Endpoint.Address.ToString().Replace("FileDownloadService.svc", "");
                System.Windows.Browser.HtmlPage.Window.Navigate(
                    new Uri(t + "PreviewInvoice.aspx?customerId="
                    + customerId + "&Guid=" + invoiceGuid + "&format=" + format),
                    "_newWindow", "toolbar=1,menubar=1,resizable=1,scrollbars=1,top=0,left=0");

            }
            catch (Exception)
            {
                throw new ArgumentException("Failed in DataService.cs, at PreviewInvoice(Guid invoiceGuid, int format)");
            }
        }

        #endregion

        #region Templates

        public IObservable<byte[]> Downloadtemplate(int templateId)
        {
            try
            {
                var client = ServiceFactory.GetFileDownloadClient(_loginSettings);

                var method = Observable.FromAsyncPattern<int, byte[]>(client.BeginDownloadTemplateFile,
                                                                               client.EndDownloadTemplateFile);

                return method(templateId).ObserveOnDispatcher();
            }
            catch (Exception)
            {
                throw new ArgumentException("Failed in DataService.cs, at Downloadtemplate(int invoiceID)");
            }
        }

        public IObservable<Unit> DeleteInvoiceTemplate(InvoiceTemplate selectedTemplate)
        {
            return _deleteInvoiceTemplate(selectedTemplate);
        }

        public IObservable<ServerResponse> ValidateTemplate(Byte[] data, int TemplateType)
        {
            var client = ServiceFactory.GetFileDownloadClient(_loginSettings);
            var method = Observable.FromAsyncPattern<Byte[], int, ServerResponse>(client.BeginValidateTemplate,
                                                                          client.EndValidateTemplate);

            return method(data, TemplateType);
            
        }

        public IObservable<ServerResponse> DeleteInvoiceFiles(int invoiceId)
        {
            var client = ServiceFactory.GetFileDownloadClient(_loginSettings);
            var method = Observable.FromAsyncPattern<int, ServerResponse>(client.BeginDeleteInvoiceFiles,
                                                                          client.EndDeleteInvoiceFiles);

            return method(invoiceId);
        }

        public IObservable<ObservableCollection<InvoiceTemplate>> GetAllInvoiceTemplates()
        {
            return _getAllInvoiceTemplates().ObserveOnDispatcher();
        }

        public IObservable<Unit> SaveInvoiceTemplate(InvoiceTemplate invoiceTemplate)
        {
            return _saveInvoiceTemplate(invoiceTemplate).ObserveOnDispatcher();
        }

        public IObservable<Unit> SaveInvoiceTemplateFile(byte[] binaryFile, int invoiceTemplateId)
        {
            return _saveInvoiceTemplateFile(binaryFile, invoiceTemplateId).ObserveOnDispatcher();
        }

        #endregion

        #region Notification helpers

        private void NotifyLoading()
        {
            _loadingNotifier.NotifySystemLoadingData();
        }

        private void SubscribeForNotifaction<T>(IObservable<T> observable)
        {
            observable.Subscribe(response => { }, _loadingNotifier.HandleLoadFailed, () => _loadingNotifier.NotifySystemIdle());
        }

        #endregion
    }
}
