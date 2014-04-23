using System;
using System.Collections.Generic;
using Trex.Common.ServiceStack;
using System.Threading.Tasks;
using Trex.SmartClient.Core.Exceptions;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Service.Helpers;
using ServiceTask = Trex.SmartClient.Service.TrexPortalService.TaskDto;
using Task = Trex.SmartClient.Core.Model.Task;
using User = Trex.SmartClient.Service.TrexPortalService.UserDto;

namespace Trex.SmartClient.Service
{
    public class TaskService : ClientServiceBase, ITaskService
    {
        private readonly IUserSession _userSession;
        private readonly IAppSettings _appSettings;
        private readonly IServiceStackClient _serviceStackClient;
        private readonly ServiceFactory _serviceFactory;
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;


        public TaskService(IProjectRepository projectRepository, ITaskRepository taskRepository, IUserSession userSession, IAppSettings appSettings,
            IServiceStackClient serviceStackClient, ServiceFactory serviceFactory)
        {
            _userSession = userSession;
            _appSettings = appSettings;
            _serviceStackClient = serviceStackClient;
            _serviceFactory = serviceFactory;
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
            _userSession = userSession;

        }

        public async Task<List<Task>> GetUnsynced(DateTime lastSyncDate)
        {
            try
            {
                var response = await _serviceStackClient.PostAsync(new TasksRequest { LastSyncDate = lastSyncDate });

                var returnList = new List<Task>();
                foreach (var task in response.Tasks)
                {
                    var project = _projectRepository.GetById(task.ProjectId);
                    if (project != null)
                    {
                        var timeRegistrationTypeEnum = (TimeRegistrationTypeEnum)(int)task.TimeRegistrationType; //now thats casting!
                        var item = Task.Create(task.Guid, task.Id, task.Name, task.Description, project, task.CreateDate, true, string.Empty,
                                               task.Inactive, timeRegistrationTypeEnum);
                        returnList.Add(item);
                    }
                    else
                    {
                        throw new MissingHieracleDataException(string.Format("Could not find projectId {0} for task '{1} ({2})'", task.ProjectId, task.Name, task.Id));
                    }
                }
                return returnList;
            }
            catch (MissingHieracleDataException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceAccessException("Error contacting service", ex);
            }
        }


        public async System.Threading.Tasks.Task SaveNewTasks()
        {
            var unsyncedTasks = _taskRepository.GetNewTasks();

            if (unsyncedTasks.Count == 0)
                return;
            var user = new User
                {
                    UserId = _userSession.CurrentUser.Id,
                    UserName = _userSession.CurrentUser.UserName,
                    FullName = _userSession.CurrentUser.Name
                };

            var serviceTasks = unsyncedTasks.ConvertAll(t =>
                                                        new ServiceTask
                                                            {
                                                                Description = t.Description,
                                                                Guid = t.Guid,
                                                                Id = t.Id,
                                                                Name = t.Name,
                                                                ProjectId = t.Project.Id,
                                                                CreateDate = t.CreateDate
                                                            }
                );

            using (var client = _serviceFactory.GetServiceClient(_userSession.LoginSettings))
            {
                try
                {
                    await client.UploadNewTasksAsync(serviceTasks, user.UserId);
                    unsyncedTasks.ForEach(t => t.IsSynced = true);
                    _taskRepository.AddOrUpdate(unsyncedTasks);

                }
                catch (Exception ex)
                {
                    throw new ServiceAccessException("Error contacting service", ex);
                }
            }
        }
    }
}
