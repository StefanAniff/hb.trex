using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web;
using D60.Toolkit.LogUtils;
using StructureMap;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.Implemented;
using TrexSL.Web.DataContracts;
using Project = TrexSL.Web.DataContracts.Project;
using Task = TrexSL.Web.DataContracts.Task;
using TimeEntry = TrexSL.Web.DataContracts.TimeEntry;
using TimeEntryType = TrexSL.Web.DataContracts.TimeEntryType;
using User = TrexSL.Web.DataContracts.User;
using UserCustomerInfo = TrexSL.Web.DataContracts.UserCustomerInfo;
using UserCreationResponse = TrexSL.Web.DataContracts.UserCreationResponse;

namespace TrexSL.Web
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class TrexSLService
    {

        [OperationContract]
        public List<Company> GetAllCompanies(bool includeInactive, bool includeParents, bool includeProjects, bool includeTasks, bool includeTimeEntries)
        {
            try
            {
                var customerRepository = ObjectFactory.GetInstance<ICustomerRepository>();
                var customers = customerRepository.GetAll(includeInactive).ToList().ToDtoObjects(includeParents, includeProjects, includeTasks,
                                                                                        includeTimeEntries);
                return customers.ToList();

            }
            catch (Exception ex)
            {

                OnError(ex);
                throw;

            }

        }
        [OperationContract]
        public List<Company> EntityCompanyRequest(List<int> companyIds, bool includeParents, bool includeInactive, bool includeProjects, bool includeTasks, bool includeTimeEntries)
        {
            try
            {
                var customerRepository = ObjectFactory.GetInstance<ICustomerRepository>();

                var companyList = new List<Company>();

                foreach (var companyId in companyIds)
                {
                    var customer = customerRepository.GetByID(companyId);
                    companyList.Add(customer.ToDtoObject(includeParents, includeProjects, includeTasks, includeTimeEntries));
                }
                return companyList.OrderBy(c => c.Name).ToList();


            }
            catch (Exception ex)
            {
                OnError(ex);
                throw;
            }
        }

        [OperationContract]
        public List<Project> EntityProjectRequest(List<int> projectIds, bool includeParents, bool includeInactive, bool includeTasks, bool includeTimeEntries)
        {
            try
            {
                var projectRepository = ObjectFactory.GetInstance<IProjectRepository>();

                var projectList = new List<Project>();

                foreach (var projectId in projectIds)
                {
                    var project = projectRepository.GetByID(projectId);
                    projectList.Add(project.ToDtoObject(includeParents, includeTasks, includeTimeEntries));
                }
                return projectList.OrderBy(p => p.Name).ToList();
            }
            catch (Exception ex)
            {
                OnError(ex);
                throw;
            }

        }

        [OperationContract]
        public List<Task> EntityTaskRequest(List<int> taskIds, bool includeParents, bool includeSubTasks, bool includeTimeEntries)
        {

            try
            {
                var taskRepository = ObjectFactory.GetInstance<ITaskRepository>();

                var taskList = new List<Task>();


                foreach (var taskId in taskIds)
                {
                    var task = taskRepository.GetById(taskId);

                    taskList.Add(task.ToDtoObject(includeParents, includeSubTasks, includeTimeEntries));
                }
                return taskList.OrderBy(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
                OnError(ex);
                throw;
            }
        }

        [OperationContract]
        public List<TimeEntry> GetTimeEntriesByPeriodAndUser(User user, DateTime startDate, DateTime endDate)
        {
            try
            {
                var timeEntryRepository = ObjectFactory.GetInstance<ITimeEntryRepository>();
                return timeEntryRepository.GetTimeEntriesByPeriodAndUser(user.UserId, startDate, endDate).ToDtoObjects(false);

            }
            catch (Exception ex)
            {
                OnError(ex);
                throw;
            }
        }

        [OperationContract]
        public List<TimeEntry> GetTimeEntriesByPeriod(DateTime startDate, DateTime endDate, bool includeParents)
        {
            try
            {
                var timeEntryRepository = ObjectFactory.GetInstance<ITimeEntryRepository>();
                return timeEntryRepository.GetTimeEntriesByPeriod(startDate, endDate).ToDtoObjects(includeParents);

            }
            catch (Exception ex)
            {
                OnError(ex);
                throw;
            }
        }



        [OperationContract]
        public List<User> GetAllUsers()
        {
            try
            {
                var userRepository = ObjectFactory.GetInstance<IUserRepository>();
                var userManagementService = ObjectFactory.GetInstance<IUserManagementService>();

                var users = userRepository.GetAllUsers();

                var dtoUsers = users.ToList().ToDtoObjects();
                foreach (var dtoUser in dtoUsers)
                {
                    dtoUser.Roles = userManagementService.GetRolesForUser(dtoUser.UserName);
                }
                return dtoUsers;

            }
            catch (Exception ex)
            {
                OnError(ex);
                throw;
            }
        }

        [OperationContract]
        public User GetUserByUserName(string userName)
        {
            try
            {
                var userRepository = ObjectFactory.GetInstance<IUserRepository>();
                var userManagementService = ObjectFactory.GetInstance<IUserManagementService>();
                var permissionService = ObjectFactory.GetInstance<IPermissionService>();
                var user = userRepository.GetByUserName(userName);
                var dtoUser = user.ToDtoObject(true);
                dtoUser.Roles = userManagementService.GetRolesForUser(dtoUser.UserName);

                var permissionFile = ConfigurationManager.AppSettings["adminPermissionConfigFile"];

                permissionFile = HttpContext.Current.Server.MapPath(permissionFile);

                dtoUser.Permissions = permissionService.GetPermissionsForRoles(dtoUser.Roles,permissionFile);

                return dtoUser;
            }
            catch (Exception ex)
            {

                OnError(ex);
                throw;
            }

        }

        [OperationContract]
        public UserCreationResponse CreateUser(UserCreationParameters userData)
        {
            var userManagementService = ObjectFactory.GetInstance<IUserManagementService>();
            var newUser = new Trex.Server.Core.Model.User()
                              {
                                  UserName = userData.User.UserName,
                                  Name = userData.User.FullName,
                                  Email = userData.User.Email,
                                  Price = userData.User.Price
                              };

            var userCreationResponse = userManagementService.CreateUser(newUser, userData.Password, userData.PasswordQuestion,
                                             userData.PasswordAnswer);

            if (userCreationResponse.Success)
                UpdateUserRoles(userData.User.UserName, userData.User.Roles);

            return userCreationResponse.ToDtoObject();



        }

        [OperationContract]
        public void SaveUser(User user)
        {
            try
            {
                var userRepository = ObjectFactory.GetInstance<IUserRepository>();
                var oldUser = userRepository.GetByUserName(user.UserName);
                SaveCustomerInfo(user.CustomerInfo, oldUser, userRepository);

                oldUser.Name = user.FullName;
                oldUser.Email = user.Email;
                oldUser.Inactive = user.Inactive;
                oldUser.Price = user.Price;


                userRepository.Update(oldUser);

                UpdateUserRoles(oldUser.UserName, user.Roles);



            }
            catch (Exception ex)
            {

                OnError(ex);
                throw;
            }
        }

        [OperationContract]
        public void DeleteUser(User user)
        {
            try
            {
                var userRepository = ObjectFactory.GetInstance<IUserRepository>();
                var userManagementService = ObjectFactory.GetInstance<IUserManagementService>();
                var userToDelete = userRepository.GetByUserName(user.UserName);
                userManagementService.DeleteUser(userToDelete);

            }
            catch (Exception ex)
            {

                OnError(ex);
            }
        }

        [OperationContract]
        public void DeactivateUser(User user)
        {
            try
            {
                var userRepository = ObjectFactory.GetInstance<IUserRepository>();
                var userManagementService = ObjectFactory.GetInstance<IUserManagementService>();
                var userToDeactivate = userRepository.GetByUserName(user.UserName);
                userManagementService.DeactivateUser(userToDeactivate);
            }
            catch (Exception ex)
            {
                OnError(ex);
                throw;
            }
        }

        [OperationContract]
        public void ActivateUser(User user)
        {
            try
            {
                var userRepository = ObjectFactory.GetInstance<IUserRepository>();
                var userManagementService = ObjectFactory.GetInstance<IUserManagementService>();
                var userToActivate = userRepository.GetByUserName(user.UserName);
                userManagementService.ActivateUser(userToActivate);
            }
            catch (Exception ex)
            {
                OnError(ex);
                throw;
            }
        }

        private void SaveCustomerInfo(IEnumerable<UserCustomerInfo> customerInfos, Trex.Server.Core.Model.User user, IUserRepository userRepository)
        {

            if (customerInfos == null)
                return;
            var customerInfoFactory = ObjectFactory.GetInstance<IUserCustomerInfoFactory>();
            var customerRepository = ObjectFactory.GetInstance<ICustomerRepository>();

            var newCustomerInfos = new List<Trex.Server.Core.Model.UserCustomerInfo>();

            foreach (var customerInfo in customerInfos)
            {
                var customer = customerRepository.GetByID(customerInfo.CustomerId);
                newCustomerInfos.Add(customerInfoFactory.Create(user, customer, customerInfo.PricePrHour));
            }

            var oldCustomerInfos = new List<Trex.Server.Core.Model.UserCustomerInfo>();

            foreach (var userCustomerInfo in user.CustomerInfo)
            {
                oldCustomerInfos.Add(userCustomerInfo);
            }

            //remove any old customerinfo, not in the new list
            foreach (var oldInfo in oldCustomerInfos)
            {
                if (newCustomerInfos.SingleOrDefault(nci => nci.Equals(oldInfo)) == null)
                {
                    user.RemoveCustomerInfo(oldInfo);
                    userRepository.DeleteCustomerInfo(oldInfo);
                }

            }




            foreach (var userCustomerInfo in newCustomerInfos)
            {
                user.AddCustomerInfo(userCustomerInfo);
            }



        }

        [OperationContract]
        public TimeEntry SaveTimeEntry(TimeEntry timeEntry)
        {
            try
            {
                var timeEntryRepository = ObjectFactory.GetInstance<ITimeEntryRepository>();
                var taskRepository = ObjectFactory.GetInstance<ITaskRepository>();
                var timeEntryFactory = ObjectFactory.GetInstance<ITimeEntryFactory>();
                var priceService = ObjectFactory.GetInstance<IPriceService>();
                var userRepository = ObjectFactory.GetInstance<IUserRepository>();
                var timeEntryTypeRepository = ObjectFactory.GetInstance<ITimeEntryTypeRepository>();
                var user = userRepository.GetByUserID(timeEntry.User.UserId);
                var task = taskRepository.GetByGuid(timeEntry.TaskGuid);
                var timeEntryType = timeEntryTypeRepository.GetById(timeEntry.TimeEntryType.Id);
                var pricePrHour = priceService.GetPrice(timeEntry.PricePrHour, task.Id, timeEntry.User.UserId);

                if (!timeEntryRepository.Exists(timeEntry.Guid))
                {
                    //TODO: Do logic that splits the timeentry in two, if a dateshift has occurred
                    //    if(timeEntry.StartTime.Date != timeEntry.EndTime.Date)


                    var newTimeEntry = timeEntryFactory.Create(
                        timeEntry.Guid,
                         user,
                         task,
                         timeEntryType,
                         timeEntry.StartTime,
                         timeEntry.EndTime,
                         timeEntry.Description,
                         timeEntry.TimeSpent,
                         0,
                         timeEntry.BillableTime,
                         timeEntry.Billable,
                         pricePrHour
                         );

                    timeEntryRepository.Save(newTimeEntry);

                    timeEntry.Id = newTimeEntry.Id;

                }

                else
                {
                    var changedTimeEntry = timeEntryRepository.GetByGuid(timeEntry.Guid);
                    changedTimeEntry.User = user;
                    changedTimeEntry.Task = task;
                    changedTimeEntry.TimeEntryType = timeEntryType;
                    changedTimeEntry.StartTime = timeEntry.StartTime;
                    changedTimeEntry.EndTime = timeEntry.EndTime;
                    changedTimeEntry.Description = timeEntry.Description;
                    changedTimeEntry.TimeSpent = timeEntry.TimeSpent;
                    changedTimeEntry.BillableTime = timeEntry.BillableTime;
                    changedTimeEntry.Billable = timeEntry.Billable;
                    changedTimeEntry.Price = pricePrHour;

                    timeEntryRepository.Save(changedTimeEntry);


                }

                return timeEntry;
            }
            catch (Exception ex)
            {
                OnError(ex);
                throw;
            }

        }

        [OperationContract]
        public Task SaveTask(Task task)
        {
            try
            {
                var taskRepository = ObjectFactory.GetInstance<ITaskRepository>();
                var taskFactory = ObjectFactory.GetInstance<ITaskFactory>();
                var userRepository = ObjectFactory.GetInstance<IUserRepository>();
                var projectRepository = ObjectFactory.GetInstance<IProjectRepository>();
                var user = userRepository.GetByUserID(task.CreatedBy.UserId);
                var project = projectRepository.GetByID(task.ProjectId);

                Trex.Server.Core.Model.Task parentTask = null;

                if (task.ParentTaskId.HasValue)
                    parentTask = taskRepository.GetById(task.ParentTaskId.Value);

                if (!taskRepository.ExistsByGuid(task.Guid))
                {
                    var newTask = taskFactory.Create(task.Guid,
                                        task.CreateDate,
                                        task.ChangeDate,
                                        task.Name,
                                       task.Description,
                                       user,
                                       project,
                                       null,
                                       parentTask,
                                       task.WorstCaseEstimate,
                                       task.BestCaseEstimate,
                                       task.RealisticEstimate,
                                       task.TimeEstimated,
                                       task.TimeLeft
                        );

                    taskRepository.Update(newTask);
                    task.Id = newTask.Id;
                    task.Guid = newTask.Guid;




                }
                else
                {
                    var originalTask = taskRepository.GetByGuid(task.Guid);

                    originalTask.Name = task.Name;
                    originalTask.Description = task.Description;
                    originalTask.ParentTask = parentTask;
                    originalTask.Project = project;
                    originalTask.RealisticEstimate = task.RealisticEstimate;
                    originalTask.BestCaseEstimate = task.BestCaseEstimate;
                    originalTask.WorstCaseEstimate = task.WorstCaseEstimate;
                    originalTask.TimeEstimated = task.TimeEstimated;
                    originalTask.TimeLeft = task.TimeLeft;
                    originalTask.Closed = task.Closed;

                    taskRepository.Update(originalTask);

                }
                return task;
            }
            catch (Exception ex)
            {
                OnError(ex);
                throw;
            }
        }

        [OperationContract]
        public Project SaveProject(Project project)
        {
            try
            {
                var projectFactory = ObjectFactory.GetInstance<IProjectFactory>();
                var companyRepository = ObjectFactory.GetInstance<ICustomerRepository>();
                var userRepository = ObjectFactory.GetInstance<IUserRepository>();
                var projectRepository = ObjectFactory.GetInstance<IProjectRepository>();
                var customer = companyRepository.GetByID(project.CompanyId);
                var user = userRepository.GetByUserID(project.CreatedBy.UserId);

                if (project.Id == 0)
                {
                    var newProject = projectFactory.Create(project.Name, customer, user, project.IsEstimatesEnabled);
                    projectRepository.Update(newProject);
                    project.Id = newProject.Id;
                    project.Guid = newProject.Guid;
                }
                else
                {
                    var originalProject = projectRepository.GetByID(project.Id);
                    originalProject.Name = project.Name;
                    originalProject.Customer = customer;
                    originalProject.Inactive = project.Inactive;
                    originalProject.CreatedBy = user;
                    originalProject.CreateDate = project.CreateDate;
                    originalProject.IsEstimatesEnabled = project.IsEstimatesEnabled;

                    projectRepository.Update(originalProject);


                }

                return project;
            }
            catch (Exception ex)
            {
                OnError(ex);
                throw;
            }

        }

        [OperationContract]
        public Company SaveCustomer(Company customer)
        {
            try
            {
                var customerFactory = ObjectFactory.GetInstance<ICustomerFactory>();
                var customerRepository = ObjectFactory.GetInstance<ICustomerRepository>();
                var userRepository = ObjectFactory.GetInstance<IUserRepository>();

                var user = userRepository.GetByUserID(customer.CreatedBy.UserId);

                if (customer.Id == 0)
                {
                    var newCustomer = customerFactory.Create(customer.Name, user, customer.StreetAddress, customer.ZipCode,
                                                             customer.Country, customer.ContactName, customer.ContactPhone,
                                                             customer.InheritsTimeEntryTypes, customer.PaymentTermNumberOfDays, customer.PaymentTermIncludeCurrentMonth, customer.Address2);

                    customerRepository.Update(newCustomer);
                    customer.Id = newCustomer.Id;
                    customer.Guid = newCustomer.Guid;
                }
                else
                {
                    var originalCustomer = customerRepository.GetByID(customer.Id);
                    originalCustomer.Name = customer.Name;
                    originalCustomer.CreatedBy = user;
                    originalCustomer.StreetAddress = customer.StreetAddress;
                    originalCustomer.ZipCode = customer.ZipCode;
                    originalCustomer.Country = customer.Country;
                    originalCustomer.ContactName = customer.ContactName;
                    originalCustomer.ContactPhone = customer.ContactPhone;
                    originalCustomer.InheritsTimeEntryTypes = customer.InheritsTimeEntryTypes;
                    originalCustomer.Inactive = customer.Inactive;
                    originalCustomer.Email = customer.Email;
                    originalCustomer.City = customer.City;
                    originalCustomer.PaymentTermIncludeCurrentMonth = customer.PaymentTermIncludeCurrentMonth;
                    originalCustomer.PaymentTermNumberOfDays = customer.PaymentTermNumberOfDays;
                    originalCustomer.Address2 = customer.Address2;

                    customerRepository.Update(originalCustomer);
                }
                return customer;
            }
            catch (Exception ex)
            {
                OnError(ex);
                throw;
            }

        }

        [OperationContract]
        public int DeleteCustomer(int customerId)
        {
            try
            {
                var customerRepository = ObjectFactory.GetInstance<ICustomerRepository>();
                var customer = customerRepository.GetByID(customerId);
                if (customer.Projects.Count == 0)
                    customerRepository.Delete(customer);
                return customerId;
            }
            catch (Exception ex)
            {
                OnError(ex);
                throw;
            }
        }

        [OperationContract]
        public int DeleteTask(int taskId)
        {
            try
            {
                var taskRepository = ObjectFactory.GetInstance<ITaskRepository>();
                var task = taskRepository.GetById(taskId);
                if (task.TimeEntries.Count == 0)
                    taskRepository.Delete(taskId);
                return taskId;
            }
            catch (Exception ex)
            {
                OnError(ex);
                throw;
            }
        }

        [OperationContract]
        public int DeleteProject(int projectId)
        {
            try
            {
                var projectRepository = ObjectFactory.GetInstance<IProjectRepository>();
                var project = projectRepository.GetByID(projectId);

                if (project.Tasks.Count == 0)
                    projectRepository.Delete(projectId);

                return projectId;
            }
            catch (Exception ex)
            {
                OnError(ex);
                throw;
            }
        }

        [OperationContract]
        public int DeleteTimeEntry(int timeEntryId)
        {
            try
            {
                var timeEntryRepository = ObjectFactory.GetInstance<ITimeEntryRepository>();
                var timeEntry = timeEntryRepository.GetById(timeEntryId);

                timeEntryRepository.Delete(timeEntry);

                return timeEntryId;
            }
            catch (Exception ex)
            {
                OnError(ex);
                throw;
            }
        }

        [OperationContract]
        public Guid CreateExcelSheet(SearchFilterTransferObject data)
        {
            try
            {
                var excelExportService = ObjectFactory.GetInstance<IExcelExportService>();
                return excelExportService.CreateWorkSheet(CreateFilter(data));
            }
            catch (Exception ex)
            {
                OnError(ex);
                throw;
            }
        }

        [OperationContract]
        public List<TimeEntryType> GetGlobalTimeEntryTypes()
        {
            try
            {
                var timeEntryTypeRepository = ObjectFactory.GetInstance<ITimeEntryTypeRepository>();
                return timeEntryTypeRepository.GetAllGlobal().ToDtoObjects();
            }
            catch (Exception ex)
            {
                OnError(ex);
                throw;
            }
        }

        [OperationContract]
        public List<TimeEntryType> GetAllTimeEntryTypes()
        {
            try
            {
                var timeEntryTypeRepository = ObjectFactory.GetInstance<ITimeEntryTypeRepository>();
                return timeEntryTypeRepository.GetAll().ToDtoObjects();
            }
            catch (Exception ex)
            {
                OnError(ex);
                throw;
            }
        }

        [OperationContract]
        public List<TimeEntryType> GetTimeEntryTypesByCustomer(int customerId)
        {
            try
            {
                var customerRepository = ObjectFactory.GetInstance<ICustomerRepository>();
                var customer = customerRepository.GetByID(customerId);
                return customer.TimeEntryTypes.ToDtoObjects();
            }
            catch (Exception ex)
            {
                OnError(ex);
                throw;
            }

        }

        [OperationContract]
        public TimeEntryType SaveTimeEntryType(TimeEntryType timeEntryType)
        {
            try
            {
                var timeEntryTypeRepository = ObjectFactory.GetInstance<ITimeEntryTypeRepository>();
                var timeEntryTypeFactory = ObjectFactory.GetInstance<ITimeEntryTypeFactory>();
                var customerRepository = ObjectFactory.GetInstance<ICustomerRepository>();
                Customer customer = null;

                if (timeEntryType.CustomerId.HasValue)
                    customer = customerRepository.GetByID(timeEntryType.CustomerId.Value);

                if (timeEntryType.Id == 0)
                {
                    var newTimeEntryType = timeEntryTypeFactory.Create(timeEntryType.Name, timeEntryType.IsDefault,
                                                                       timeEntryType.IsBillableByDefault, customer);


                    timeEntryTypeRepository.Update(newTimeEntryType);
                    timeEntryType.Id = newTimeEntryType.Id;


                }
                else
                {
                    var originalObject = timeEntryTypeRepository.GetById(timeEntryType.Id);
                    originalObject.Customer = customer;
                    originalObject.IsBillableByDefault = timeEntryType.IsBillableByDefault;
                    originalObject.IsDefault = timeEntryType.IsDefault;
                    originalObject.Name = timeEntryType.Name;
                    timeEntryTypeRepository.Update(originalObject);
                }

                return timeEntryType;
            }
            catch (Exception ex)
            {
                OnError(ex);
                throw;
            }

        }

        private SelectionFilter CreateFilter(SearchFilterTransferObject data)
        {

            var customerRepository = ObjectFactory.GetInstance<ICustomerRepository>();
            var projectRepository = ObjectFactory.GetInstance<IProjectRepository>();
            var taskRepository = ObjectFactory.GetInstance<ITaskRepository>();
            var userRepository = ObjectFactory.GetInstance<IUserRepository>();
            var filter = new SelectionFilter();

            foreach (var customerId in data.Customers)
            {
                filter.AddCustomer(customerRepository.GetByID(customerId));

            }

            foreach (var projectId in data.Projects)
            {
                filter.AddProject(projectRepository.GetByID(projectId));

            }

            foreach (var taskId in data.Tasks)
            {
                filter.AddTask(taskRepository.GetById(taskId));
            }

            if (data.Users != null)
                filter.Users = data.Users.Select(u => userRepository.GetByUserID(u)).ToList();

            filter.DateFrom = data.DateFrom;
            filter.DateTo = data.DateTo;


            return filter;

        }

        [OperationContract]
        public List<Task> SearchTasks(string searchString)
        {
            var taskRepository = ObjectFactory.GetInstance<ITaskRepository>();
            return taskRepository.SearchTasksBySearchString(searchString).ToDtoObjects(true, false, false);
        }

        [OperationContract]
        public List<Project> SearchProjects(string searchString)
        {
            var projectRepository = ObjectFactory.GetInstance<IProjectRepository>();
            return projectRepository.SearchProject(searchString).ToDtoObjects(true, false, false);
        }

        #region TimeTracker Client Methods




        [OperationContract]
        public bool ValidateUser(string userName, string password)
        {
            try
            {
                var userSession = ObjectFactory.GetInstance<IUserSession>();
                return userSession.ValidateCredentials(userName, password);
            }
            catch (Exception ex)
            {
                OnError(ex);
                throw;
            }
        }
        [OperationContract]
        public User GetUser(string userName, string password)
        {
            try
            {
                var userSession = ObjectFactory.GetInstance<IUserSession>();
                var userRepository = ObjectFactory.GetInstance<IUserRepository>();
                var userManagementService = ObjectFactory.GetInstance<IUserManagementService>();
                var permissionService = ObjectFactory.GetInstance<IPermissionService>();

                if (userSession.ValidateCredentials(userName, password))
                {
                    var user = userRepository.GetByUserName(userName);
                    var dtoUser = user.ToDtoObject(true);
                    dtoUser.Roles = userManagementService.GetRolesForUser(dtoUser.UserName);

                    var permissionFile = ConfigurationManager.AppSettings["clientPermissionConfigFile"];

                    permissionFile = HttpContext.Current.Server.MapPath(permissionFile);

                    dtoUser.Permissions = permissionService.GetPermissionsForRoles(dtoUser.Roles, permissionFile);
                    return dtoUser;
                }

                return null;
            }
            catch (Exception ex)
            {
                OnError(ex);
                throw;
            }
        }
        [OperationContract]
        public bool PingService()
        {
            return true;
        }
        [OperationContract]
        public UserStatistics GetUserStatistics(int userId, int numOfDaysBack)
        {
            try
            {
                var timeEntryRepository = ObjectFactory.GetInstance<ITimeEntryRepository>();
                var info = new UserStatistics();
                var firstDayOfWeek = DateTime.Now;
                var firstDayOfMonth = DateTime.Now;

                while (firstDayOfWeek.DayOfWeek != DayOfWeek.Monday)
                {
                    firstDayOfWeek = firstDayOfWeek.AddDays(-1);
                }

                while (firstDayOfMonth.Day != 1)
                {
                    firstDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                }

                info.RegisteredHoursThisWeek = timeEntryRepository.GetRegisteredHours(firstDayOfWeek.Date, DateTime.Now, userId);
                info.RegisteredHoursToday = timeEntryRepository.GetRegisteredHours(DateTime.Now.Date, DateTime.Now, userId);
                info.RegisteredHoursThisMonth = timeEntryRepository.GetRegisteredHours(firstDayOfMonth.Date, DateTime.Now, userId);
                info.EarningsToday = timeEntryRepository.GetEarningsByUser(DateTime.Now.Date, DateTime.Now, userId);
                info.EarningsThisWeek = timeEntryRepository.GetEarningsByUser(firstDayOfWeek.Date, DateTime.Now, userId);
                info.EarningsThisMonth = timeEntryRepository.GetEarningsByUser(firstDayOfMonth.Date, DateTime.Now, userId);
                info.BillableHoursToday = timeEntryRepository.GetBillableHours(DateTime.Now.Date, DateTime.Now, userId);
                info.BillableHoursThisWeek = timeEntryRepository.GetBillableHours(firstDayOfWeek.Date, DateTime.Now, userId);
                info.BillableHoursThisMonth = timeEntryRepository.GetBillableHours(firstDayOfMonth.Date, DateTime.Now, userId);

                var thisWeeksEntries = timeEntryRepository.GetTimeEntriesByPeriodAndUser(userId, DateTime.Now.Date.AddDays(-numOfDaysBack),
                                                                                              DateTime.Now);

                info.ThisWeeksTimeEntries = thisWeeksEntries.ToDtoObjects(true);

                return info;
            }
            catch (Exception ex)
            {
                OnError(ex);
                throw;
            }
        }

        [OperationContract]
        public List<Company> GetUnsyncedCompanies(DateTime lastSyncDate)
        {
            try
            {
                var customerRepository = ObjectFactory.GetInstance<ICustomerRepository>();
                var customers = customerRepository.GetByChangeDate(lastSyncDate).ToList().ToDtoObjects(false, false, false, false);
                return customers;
            }
            catch (Exception ex)
            {
                OnError(ex);
                throw;
            }
        }

        [OperationContract]
        public List<Project> GetUnsyncedProjects(DateTime lastSyncDate)
        {
            try
            {
                var projectRepository = ObjectFactory.GetInstance<IProjectRepository>();
                return projectRepository.GetByChangeDate(lastSyncDate).ToList().ToDtoObjects(false, false, false);
            }
            catch (Exception ex)
            {
                OnError(ex);
                throw;
            }
        }

        [OperationContract]
        public List<Task> GetUnsyncedTasks(DateTime lastSyncDate)
        {
            try
            {

                var taskRepository = ObjectFactory.GetInstance<ITaskRepository>();

                return taskRepository.GetByChangeDate(lastSyncDate).ToList().ToDtoObjects(false, true, false);
            }
            catch (Exception ex)
            {

                OnError(ex);
                throw;
            }
        }

        [OperationContract]
        public List<Task> UploadNewTasks(List<Task> tasks, User user)
        {

            try
            {
                foreach (var task in tasks)
                {
                    task.CreatedBy = user;
                    SaveTask(task);
                }

                return tasks;
            }
            catch (Exception ex)
            {
                OnError(ex);
                throw;
            }
        }

        [OperationContract]
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

                OnError(ex);
                throw;
            }

        }


        [OperationContract]
        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            var userManagementService = ObjectFactory.GetInstance<IUserManagementService>();
            var userRepository = ObjectFactory.GetInstance<IUserRepository>();

            var modelUser = userRepository.GetByUserName(userName);

            return userManagementService.ChangePassword(modelUser, oldPassword, newPassword);
        }

        [OperationContract]
        public List<string> GetRoles()
        {
            var userManagementService = ObjectFactory.GetInstance<IUserManagementService>();
            return userManagementService.GetRoles();
        }

        [OperationContract]
        public void UpdateUserRoles(string userName, List<string> roles)
        {
            var userRepository = ObjectFactory.GetInstance<IUserRepository>();

            var modelUser = userRepository.GetByUserName(userName);
            var userManagementService = ObjectFactory.GetInstance<IUserManagementService>();
            userManagementService.UpdateUserRoles(modelUser, roles);
        }



        [OperationContract]
        public bool ResetPassword(string userName)
        {
            var userManagementService = ObjectFactory.GetInstance<IUserManagementService>();
            var userRepository = ObjectFactory.GetInstance<IUserRepository>();
            var mailComposer = ObjectFactory.GetInstance<IEmailComposer>();

            var modelUser = userRepository.GetByUserName(userName);
            var newPassword = userManagementService.ResetPassword(modelUser);
            if (newPassword != null)
            {
                mailComposer.SendForgotPasswordEmail(modelUser.Name, newPassword, modelUser.Email);
                return true;
            }
            return false;
        }


        private void OnError(Exception ex)
        {
            var logger = new Logger();
            logger.LogError(ex);
        }

        #endregion

    }



}
