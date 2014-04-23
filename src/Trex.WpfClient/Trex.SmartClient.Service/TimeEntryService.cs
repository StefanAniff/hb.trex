using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using Trex.Common.DataTransferObjects;
using Trex.Common.ServiceStack;
using Trex.SmartClient.Core.Exceptions;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Service.Helpers;
using Trex.SmartClient.Service.TrexPortalService;
using Task = System.Threading.Tasks.Task;
using TimeEntryDto = Trex.SmartClient.Service.TrexPortalService.TimeEntryDto;
using TimeRegistrationTypeEnum = Trex.SmartClient.Core.Model.TimeRegistrationTypeEnum;


namespace Trex.SmartClient.Service
{
    public class TimeEntryService : ClientServiceBase, ITimeEntryService
    {
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly IUserSession _userSession;
        private readonly ITaskRepository _taskRepository;
        private readonly ITimeEntryTypeRepository _timeEntryTypeRepository;
        private readonly ServiceFactory _serviceFactory;
        private readonly IServiceStackClient _serviceStackClient;

        public TimeEntryService(IUserSession userSession, ITimeEntryRepository timeEntryRepository,
                                ITaskRepository taskRepository, ITimeEntryTypeRepository timeEntryTypeRepository,
                                IServiceStackClient serviceStackClient, ServiceFactory serviceFactory)
        {
            _userSession = userSession;
            _timeEntryRepository = timeEntryRepository;
            _taskRepository = taskRepository;
            _timeEntryTypeRepository = timeEntryTypeRepository;
            _serviceFactory = serviceFactory;
            _serviceStackClient = serviceStackClient;
        }

        private async Task<List<TimeEntry>> ConvertTimeEntriesFromServiceToModel(List<Common.DataTransferObjects.TimeEntryDto> timeentriesDtos,
                                                                                 ITaskRepository taskRepository, ITimeEntryTypeRepository timeEntryTypeRepository)
        {
            var timeEntries = new List<TimeEntry>();

            var inactiveTasksWithTimeEntriesInPeriod = await _serviceStackClient.PostAsync(new GetTasksByIdRequest
                {
                    TaskIds = timeentriesDtos.Where(x => x.TaskIsInactive).Select(x => x.TaskId).ToList()
                });

            foreach (var timeEntryDtos in timeentriesDtos.GroupBy(x => x.TaskGuid))
            {
                Core.Model.Task localTask;

                var taskIsInactive = timeEntryDtos.Select(x => x.TaskIsInactive).First();
                if (taskIsInactive)
                {
                    var fullTaskDto = inactiveTasksWithTimeEntriesInPeriod.Tasks.Single(x => x.Guid == timeEntryDtos.Key);
                    localTask = CreateLocalTask(fullTaskDto);
                }
                else
                {
                    localTask = taskRepository.GetByGuid(timeEntryDtos.Key);
                }

                foreach (var timeEntryDto in timeEntryDtos)
                {
                    var timeEntryType = timeEntryTypeRepository.GetById(timeEntryDto.TimeEntryTypeId);
                    if (localTask != null)
                    {
                        timeEntries.Add(Create(timeEntryDto, localTask, timeEntryType));
                    }
                    else
                    {
                        throw new MissingHieracleDataException(string.Format("Could not find task {0} for timeEntry '{1} ({2})'",
                                                                             timeEntryDto.TaskGuid, timeEntryDto.TaskName, timeEntryDto.Id));
                    }
                }
            }
            return timeEntries;
        }

        /// <summary>
        /// As this is an inactive task, we need to create the whole datagraph, as the parent properties does not exist locally
        /// </summary>
        private static Core.Model.Task CreateLocalTask(FullTaskDto task)
        {
            var timeRegistrationTypeEnum = (TimeRegistrationTypeEnum)(int)task.TimeRegistrationType; //now thats casting!

            var company = task.Project.CompanyDto;
            var customer = Company.Create(company.Name, company.Id, company.InheritsTimeEntryTypes, company.Inactive);
            var project = Project.Create(task.Project.Id, task.Project.Name, customer, task.Project.Inactive);

            var localTask = Core.Model.Task.Create(task.Guid, task.Id, task.Name, task.Description, project, task.CreateDate, true, string.Empty,
                                                   task.Inactive, timeRegistrationTypeEnum);
            return localTask;
        }

        private static TimeEntry Create(Common.DataTransferObjects.TimeEntryDto timeEntryDto, Core.Model.Task task, TimeEntryType timeEntryType)
        {
            return TimeEntry.Create(timeEntryDto.Guid,
                                    task,
                                    timeEntryType,
                                    TimeSpan.FromHours(timeEntryDto.TimeSpent),
                                    TimeSpan.FromHours(timeEntryDto.BillableTime),
                                    timeEntryDto.Description,
                                    timeEntryDto.StartTime,
                                    timeEntryDto.EndTime,
                                    timeEntryDto.PricePrHour,
                                    true,
                                    string.Empty,
                                    timeEntryDto.Billable,
                                    new TimeEntryHistory(),
                                    true,
                                    timeEntryDto.CreateDate,
                                    timeEntryDto.ClientSourceId,
                                    timeEntryDto.Invoiced);
        }


        public async Task SaveNewTimeEntries(DateTime lastSyncDate)
        {
            var unsyncedTasks = _timeEntryRepository.GetUnsyncedTimeEntries();
            var unsyncedTasksCount = unsyncedTasks.Count;

            if (unsyncedTasksCount == 0)
                return;

            var user = new UserDto
            {
                UserId = _userSession.CurrentUser.Id,
                UserName = _userSession.CurrentUser.UserName,
                FullName = _userSession.CurrentUser.Name
            };

            var serviceEntries = new List<Common.DataTransferObjects.TimeEntryDto>();
            foreach (var timeEntry in unsyncedTasks)
            {
                var timeEntryDto = new Common.DataTransferObjects.TimeEntryDto();
                timeEntryDto.Description = timeEntry.Description;
                timeEntryDto.Guid = timeEntry.Guid;
                timeEntryDto.BillableTime = timeEntry.BillableTime.TotalHours;
                timeEntryDto.Billable = timeEntry.Billable;
                timeEntryDto.TimeSpent = timeEntry.TimeSpent.TotalHours;
                timeEntryDto.TimeEntryTypeId = timeEntry.TimeEntryType.Id;
                timeEntryDto.StartTime = timeEntry.StartTime;
                timeEntryDto.EndTime = timeEntry.EndTime;

                timeEntryDto.TaskId = timeEntry.Task.Id;
                timeEntryDto.TaskGuid = timeEntry.Task.Guid;
                timeEntryDto.PricePrHour = timeEntry.PricePrHour;
                timeEntryDto.CreateDate = timeEntry.CreateDate;
                timeEntryDto.ClientSourceId = timeEntry.ClientSourceId;
                serviceEntries.Add(timeEntryDto);
            }


            try
            {
                var response = await _serviceStackClient.PostAsync(new SaveOrUpdateTimeEntriesRequest
                   {
                       TimeEntries = serviceEntries.ToList(),
                       UserId = user.UserId
                   });

                foreach (var responseTimeEntryStatus in response.TimeEntryStatus)
                {
                    var unsyncedTask = unsyncedTasks.SingleOrDefault(x => x.Guid == responseTimeEntryStatus.Guid);
                    if (unsyncedTask != null)
                    {
                        unsyncedTask.IsSynced = responseTimeEntryStatus.IsOK;
                        unsyncedTask.SyncResponse = responseTimeEntryStatus.ReasonText;
                    }
                }
                _timeEntryRepository.AddOrUpdateRange(unsyncedTasks);
            }
            catch (CommunicationException ex)
            {
                throw new ServiceAccessException("Error when uploading timeentries", ex);
            }
        }

        public async Task<List<TimeEntry>> GetTimeEntriesByDateIgnoreEmptyTimeEntries(DateTime startTime, DateTime endTime)
        {
            return await GetTimeEntries(startTime, endTime, true);
        }

        public async Task<List<TimeEntry>> GetTimeEntriesByDate(DateTime startTime, DateTime endTime)
        {
            return await GetTimeEntries(startTime, endTime, false);
        }


        private async Task<List<TimeEntry>> GetTimeEntries(DateTime startTime, DateTime endTime, bool ignoreEmptyTimeEntries)
        {
            try
            {
                var request = new GetTimeEntryByPeriodAndUserRequest
                    {
                        UserId = _userSession.CurrentUser.Id,
                        StartDate = startTime,
                        EndDate = endTime,
                    };
                var response = await _serviceStackClient.PostAsync(request);


                var timeEntries = await ConvertTimeEntriesFromServiceToModel(response.TimeEntries, _taskRepository, _timeEntryTypeRepository);
                if (ignoreEmptyTimeEntries)
                {
                    return timeEntries.Where(x => x.TimeSpent.Ticks != 0).ToList();
                }
                return timeEntries;
            }
            catch (CommunicationException ex)
            {
                throw new ServiceAccessException("Error when loading timeentries", ex);
            }
        }
    }
}
