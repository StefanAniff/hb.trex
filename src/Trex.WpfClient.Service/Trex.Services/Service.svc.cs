using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using AutoMapper;
using Trex.Server.Core;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;
using Trex.Server.Core.Unity;
using Trex.Server.Infrastructure.UnitOfWork;
using TrexSL.Web.DataContracts;
using TrexSL.Web.Exceptions;
using TrexSL.Web.Intercepts;
using TrexSL.Web.ServiceInterfaces;

namespace TrexSL.Web
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, IncludeExceptionDetailInFaults = true, ConcurrencyMode = ConcurrencyMode.Single)]
    [UnityAndErrorBehavior]
    [CustomBehavior(true)]
    public class TrexSLService : ITrexSlService
    {
        private readonly IUserSession _userSession;
        private readonly IUserRepository _userRepository;
        private readonly IUserManagementService _userManagementService;
        private readonly IPermissionService _permissionService;
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly ITimeEntryFactory _timeEntryFactory;
        private readonly ITimeEntryTypeRepository _timeEntryTypeRepository;
        private readonly IPriceService _priceService;
        private readonly ITaskFactory _taskFactory;
        private readonly IProjectRepository _projectRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IRoleManagementService _roleManagementService;
        private readonly IMembershipProvider _membershipProvider;

        public TrexSLService(IUserSession userSession, IUserRepository userRepository, IUserManagementService userManagementService,
                             IPermissionService permissionService, ITimeEntryRepository timeEntryRepository, ITaskRepository taskRepository, ITimeEntryFactory timeEntryFactory,
                             ITimeEntryTypeRepository timeEntryTypeRepository, IPriceService priceService, ITaskFactory taskFactory, IProjectRepository projectRepository,
                             ICustomerRepository customerRepository, IRoleManagementService roleManagementService, IMembershipProvider membershipProvider)
        {
            _userSession = userSession;
            _userRepository = userRepository;
            _userManagementService = userManagementService;
            _permissionService = permissionService;
            _timeEntryRepository = timeEntryRepository;
            _taskRepository = taskRepository;
            _timeEntryFactory = timeEntryFactory;
            _timeEntryTypeRepository = timeEntryTypeRepository;
            _priceService = priceService;
            _taskFactory = taskFactory;
            _projectRepository = projectRepository;
            _customerRepository = customerRepository;
            _roleManagementService = roleManagementService;
            _membershipProvider = membershipProvider;
        }

        [UnitOfWork(true)]
        public List<TimeEntryDto> GetTimeEntriesByPeriodAndUser(int userId, DateTime startDate, DateTime endDate)
        {
            var timeEntriesByPeriodAndUser = _timeEntryRepository.GetTimeEntriesByPeriodAndUser(userId, startDate, endDate).ToList();
            return Mapper.Map<List<TimeEntry>, List<TimeEntryDto>>(timeEntriesByPeriodAndUser);
        }

        [UnitOfWork(true)]
        public List<GeneralTimeEntryDto> GetTimeEntriesByPeriod(DateTime startDate, DateTime endDate)
        {
            var timeEntriesByPeriod = _timeEntryRepository.GetTimeEntriesByPeriod(startDate, endDate).ToList();
            return Mapper.Map<List<TimeEntry>, List<GeneralTimeEntryDto>>(timeEntriesByPeriod);
        }

        [UnitOfWork(true)]
        public List<GeneralTimeEntryDto> GetAllTimeEntriesByPeriod(DateTime startDate, DateTime endDate)
        {
            var timeEntriesByPeriod = _timeEntryRepository.GetAllTimeEntriesByPeriod(startDate, endDate).ToList();
            return Mapper.Map<List<TimeEntry>, List<GeneralTimeEntryDto>>(timeEntriesByPeriod);
        }

        [UnitOfWork]
        public void ChangeTaskActiveState(int taskId, bool isInactive)
        {
            var task = _taskRepository.GetById(taskId);
            task.Inactive = isInactive;
            _taskRepository.SaveOrUpdate(task);
        }

        [UnitOfWork]
        public TimeEntryDto SaveTimeEntry(TimeEntryDto timeEntryDto, int userId)
        {
            var user = _userRepository.GetByUserID(userId);
            var task = _taskRepository.GetByGuid(timeEntryDto.TaskGuid);
            var timeEntryType = _timeEntryTypeRepository.GetById(timeEntryDto.TimeEntryTypeId);
            var pricePrHour = _priceService.GetPrice(timeEntryDto.PricePrHour, user, task);

            if (!_timeEntryRepository.Exists(timeEntryDto.Guid))
            {
                //TODO: Do logic that splits the timeentry in two, if a dateshift has occurred
                //    if(timeEntry.StartTime.Date != timeEntry.EndTime.Date)

                var newTimeEntry = _timeEntryFactory.Create(
                    timeEntryDto.Guid,
                    user,
                    task,
                    timeEntryType,
                    timeEntryDto.StartTime,
                    timeEntryDto.EndTime,
                    timeEntryDto.Description,
                    timeEntryDto.TimeSpent,
                    0,
                    timeEntryDto.BillableTime,
                    timeEntryDto.Billable,
                    pricePrHour,
                    timeEntryDto.ClientSourceId
                    );

                _timeEntryRepository.SaveOrUpdate(newTimeEntry);
                timeEntryDto.Id = newTimeEntry.Id;
            }
            else
            {
                var changedTimeEntry = _timeEntryRepository.GetByGuid(timeEntryDto.Guid);
                changedTimeEntry.User = user;
                changedTimeEntry.Task = task;
                changedTimeEntry.TimeEntryType = timeEntryType;
                changedTimeEntry.StartTime = timeEntryDto.StartTime;
                changedTimeEntry.EndTime = timeEntryDto.EndTime;
                changedTimeEntry.Description = timeEntryDto.Description;
                changedTimeEntry.TimeSpent = timeEntryDto.TimeSpent;
                changedTimeEntry.BillableTime = timeEntryDto.BillableTime;
                changedTimeEntry.Billable = timeEntryDto.Billable;
                changedTimeEntry.Price = pricePrHour;

                _timeEntryRepository.SaveOrUpdate(changedTimeEntry);
            }

            return timeEntryDto;
        }



        private void SaveTask(TaskDto task, int userId)
        {
            var user = _userRepository.GetByUserID(userId);
            var project = _projectRepository.GetById(task.ProjectId);

            Trex.Server.Core.Model.Task parentTask = null;

            if (task.ParentTaskId.HasValue)
                parentTask = _taskRepository.GetById(task.ParentTaskId.Value);

            if (!_taskRepository.ExistsByGuid(task.Guid))
            {
                var newTask = _taskFactory.Create(task.Guid,
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

                _taskRepository.SaveOrUpdate(newTask);
                task.Id = newTask.TaskID;
                task.Guid = newTask.Guid;
            }
            else
            {
                var originalTask = _taskRepository.GetByGuid(task.Guid);

                originalTask.TaskName = task.Name;
                originalTask.Description = task.Description;
                originalTask.ParentTask = parentTask;
                originalTask.Project = project;
                originalTask.RealisticEstimate = task.RealisticEstimate;
                originalTask.BestCaseEstimate = task.BestCaseEstimate;
                originalTask.WorstCaseEstimate = task.WorstCaseEstimate;
                originalTask.TimeEstimated = task.TimeEstimated;
                originalTask.TimeLeft = task.TimeLeft;

                _taskRepository.SaveOrUpdate(originalTask);
            }
        }


        [UnitOfWork(true)]
        public List<TimeEntryTypeDto> GetAllTimeEntryTypes()
        {
            var allTimeEntryTypes = _timeEntryTypeRepository.GetAll().ToList();
            return Mapper.Map<List<TimeEntryType>, List<TimeEntryTypeDto>>(allTimeEntryTypes);
        }

        [UnitOfWork]
        public bool ValidateUser(string userName, string password)
        {
            return _userSession.ValidateCredentials(userName, password);
        }

        [UnitOfWork(true)]
        public UserDto GetUser(string userName, string password)
        {
            if (_userSession.ValidateCredentials(userName, password))
            {
                var user = _userRepository.GetByUserName(userName);
                var dtoUser = Mapper.Map<User, UserDto>(user);

                dtoUser.Roles = _userManagementService.GetRolesForUser(dtoUser.UserName);

                // User must have at least one role
                if (!dtoUser.Roles.Any())
                    throw new RoleException(string.Format("User {0} has no membership-roles", userName));

                var objectPermissions = _permissionService.GetPermissions(dtoUser.Roles,
                                                                            TenantConnectionProvider.DynamicString,
                                                                            TenantConnectionProvider.ApplicationType);
                dtoUser.Permissions = objectPermissions.Where(permissionItem => permissionItem.IsEnabled)
                                                       .Select(permissionItem => permissionItem.PermissionName)
                                                       .ToList();

                return dtoUser;
            }
            return null;
        }

        public bool PingService()
        {
            return true;
        }

        [UnitOfWork(true)]
        public UserStatistics GetUserStatistics(int userId, int numOfDaysBack)
        {
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

            info.RegisteredHoursThisWeek = _timeEntryRepository.GetRegisteredHours(firstDayOfWeek.Date, DateTime.Now, userId);
            info.RegisteredHoursToday = _timeEntryRepository.GetRegisteredHours(DateTime.Now.Date, DateTime.Now, userId);
            info.RegisteredHoursThisMonth = _timeEntryRepository.GetRegisteredHours(firstDayOfMonth.Date, DateTime.Now, userId);
            info.EarningsToday = _timeEntryRepository.GetEarningsByUser(DateTime.Now.Date, DateTime.Now, userId);
            info.EarningsThisWeek = _timeEntryRepository.GetEarningsByUser(firstDayOfWeek.Date, DateTime.Now, userId);
            info.EarningsThisMonth = _timeEntryRepository.GetEarningsByUser(firstDayOfMonth.Date, DateTime.Now, userId);
            info.BillableHoursToday = _timeEntryRepository.GetBillableHours(DateTime.Now.Date, DateTime.Now, userId);
            info.BillableHoursThisWeek = _timeEntryRepository.GetBillableHours(firstDayOfWeek.Date, DateTime.Now, userId);
            info.BillableHoursThisMonth = _timeEntryRepository.GetBillableHours(firstDayOfMonth.Date, DateTime.Now, userId);

            return info;
        }

        #region Companies

        [UnitOfWork(true)]
        public List<CompanyDto> GetUnsyncedCompanies(DateTime lastSyncDate)
        {
            var customers = _customerRepository.GetByChangeDate(lastSyncDate).ToList();
            return Mapper.Map<List<Company>, List<CompanyDto>>(customers);

        }

        [UnitOfWork(true)]
        public CompressedObject GetUnsyncedCompaniesCompressed(DateTime lastSyncDate)
        {
            var customers = GetUnsyncedCompanies(lastSyncDate);

            var helper = new SerializationHelper();
            var serializedResponse = helper.Serialize(customers);
            var compressedResponse = helper.TryCompress(serializedResponse, customers.GetType());

            return compressedResponse;
        }

        [UnitOfWork(true)]
        public List<CompanyDto> GetCompaniesByNameSearchString(string searchString)
        {
            var companies = _customerRepository
                .GetByNameSearchString(searchString)
                .ToList();

            return Mapper.Map<List<Company>, List<CompanyDto>>(companies);            
        }

        # endregion

        [UnitOfWork(true)]
        public List<ProjectDto> GetUnsyncedProjects(DateTime lastSyncDate)
        {
            var unsyncedProjects = _projectRepository.GetByChangeDate(lastSyncDate).ToList();
            return Mapper.Map<List<Project>, List<ProjectDto>>(unsyncedProjects);
        }

        [UnitOfWork(true)]
        public List<TaskDto> GetUnsyncedTasks(DateTime? lastSyncDate)
        {
            var unsyncedTasks = _taskRepository.GetByChangeDate(lastSyncDate).ToList();
            return Mapper.Map<List<Task>, List<TaskDto>>(unsyncedTasks);
        }


        [UnitOfWork(true)]
        public CompressedObject GetUnsyncedTasksCompressed(DateTime? lastSyncDate)
        {
            var tasks = _taskRepository.GetByChangeDate(lastSyncDate).ToList();

            var helper = new SerializationHelper();
            var serializedResponse = helper.Serialize(tasks);
            var compressedResponse = helper.TryCompress(serializedResponse, tasks.GetType());

            return compressedResponse;
        }


        [UnitOfWork]
        public List<TaskDto> UploadNewTasks(List<TaskDto> tasks, int userId)
        {
            foreach (var task in tasks)
            {
                SaveTask(task, userId);
            }

            return tasks;
        }

        [UnitOfWork]
        public void UploadNewTimeEntries(List<TimeEntryDto> timeEntries, int userId)
        {
            foreach (var timeentry in timeEntries)
            {
                SaveTimeEntry(timeentry, userId);
            }
        }

        [UnitOfWork]
        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            var modelUser = _userRepository.GetByUserName(userName);
            return _userManagementService.ChangePassword(modelUser, oldPassword, newPassword);
        }


        [UnitOfWork(true)]
        public List<string> GetRoles()
        {
            return _userManagementService.GetRoles();
        }

        [UnitOfWork]
        public void UpdateUserRoles(string userName, List<string> roles)
        {
            var modelUser = _userRepository.GetByUserName(userName);
            _userManagementService.UpdateUserRoles(modelUser, roles);
        }


        #region ROLES AND PERMISSIONS


        [UnitOfWork]
        [FaultContract(typeof(ExceptionInfo))]
        public void CreateRole(string roleName)
        {
            try
            {
                if (!ExistsRole(roleName))
                {
                    _membershipProvider.CreateRole(roleName);
                    _roleManagementService.CreateRole(roleName, TenantConnectionProvider.DynamicString);
                }
                else
                {
                    throw new RoleException("Role name is already in use");
                }
            }
            catch (RoleException exception)
            {
                throw new FaultException<ExceptionInfo>(new ExceptionInfo("Service returned an error", "Service error details"),
                                                        exception.Message);
            }
        }


        [UnitOfWork]
        [FaultContract(typeof(ExceptionInfo))]
        public void DeleteRole(string roleName)
        {
            try
            {
                if (ExistsRole(roleName) && (!_membershipProvider.GetUsersInRole(roleName).Any()))
                {
                    _membershipProvider.DeleteRole(roleName);
                    _roleManagementService.DeleteRole(roleName, TenantConnectionProvider.DynamicString);
                }
                else
                {
                    throw new RoleException("Unable to delete role because it has users assigned to it");
                }
            }
            catch (RoleException exception)
            {
                throw new FaultException<ExceptionInfo>(new ExceptionInfo("Service returned an error", "Service error details"),
                                                        exception.Message);
            }
        }


        [UnitOfWork]
        public bool ExistsRole(string name)
        {
            return _membershipProvider.RoleExists(name);
        }


        [UnitOfWork]
        public List<PermissionItemDto> GetPermissionsForSingleRole(string roleName, int applicationId)
        {
            var rolesList = new List<string>
                {
                    roleName
                };
            var permissions = _permissionService.GetPermissions(rolesList, TenantConnectionProvider.DynamicString, applicationId).ToList();
            return Mapper.Map<List<PermissionItem>, List<PermissionItemDto>>(permissions);
        }


        [UnitOfWork]
        public void UpdatePermissions(List<PermissionItemDto> updatedPermissions, string roleName, int applicationId)
        {
            var newPermissions = updatedPermissions;
            var oldPermissions = GetPermissionsForSingleRole(roleName, applicationId);

            for (var x = 0; x < updatedPermissions.Count; x++)
            {
                if (newPermissions[x].IsEnabled && !oldPermissions[x].IsEnabled)
                    _permissionService.AddPermission(newPermissions[x].Id, roleName, TenantConnectionProvider.DynamicString);

                if (!newPermissions[x].IsEnabled && oldPermissions[x].IsEnabled)
                    _permissionService.RemovePermission(newPermissions[x].Id, roleName, TenantConnectionProvider.DynamicString);
            }
        }

        #endregion
    }
}
