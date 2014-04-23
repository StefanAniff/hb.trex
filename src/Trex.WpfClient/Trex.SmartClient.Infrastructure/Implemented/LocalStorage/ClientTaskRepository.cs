using System;
using System.Collections.Generic;
using System.Linq;
using Trex.SmartClient.Core.Exceptions;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;

namespace Trex.SmartClient.Infrastructure.Implemented.LocalStorage
{
    public class ClientTaskRepository : ITaskRepository
    {
        private readonly DataSetWrapper _dataWrapper;
        private readonly IProjectRepository _projectRepository;

        public ClientTaskRepository(IProjectRepository projectRepository, DataSetWrapper dataSetWrapper)
        {
            _dataWrapper = dataSetWrapper;
            _projectRepository = projectRepository;
        }

        public List<Task> GetAll()
        {
            var tasks = new List<Task>();

            foreach (var taskRow in _dataWrapper.Tasks)
            {
                var task = Task.Create(taskRow.Guid,
                                       taskRow.Id,
                                       taskRow.Name,
                                       taskRow.Description,
                                       _projectRepository.GetById(taskRow.ProjectId),
                                       taskRow.CreateDate,
                                       taskRow.Synced,
                                       taskRow.SyncInfo,
                                       taskRow.Inactive, 
                                       (TimeRegistrationTypeEnum) taskRow.TimeRegistionTypeId);
                tasks.Add(task);

            }
            return tasks;
        }

        public void AddOrUpdate(Task task)
        {
            AddOrUpdateInternal(task);
            _dataWrapper.Save();
        }

        public void AddOrUpdate(List<Task> tasks)
        {
            if (!tasks.Any())
            {
                return;
            }
            _dataWrapper.SaveTask(tasks);
            _dataWrapper.Save();
        }

        private void AddOrUpdateInternal(Task task)
        {
            _dataWrapper.SaveTask(task);
        }

        public Task GetByGuid(Guid guid)
        {
            var row = _dataWrapper.GetTaskByGuid(guid);
            if (row == null)
            {
                throw new NotFoundByGuidException("Task not found by guid", guid, null);
            }
            var timeRegistrationTypeEnum = (TimeRegistrationTypeEnum) row.TimeRegistionTypeId;
            return Task.Create(row.Guid, row.Id, row.Name, row.Description, _projectRepository.GetById(row.ProjectId), row.CreateDate, row.Synced, row.SyncInfo, row.Inactive, timeRegistrationTypeEnum);
        }

        public List<Task> GetNewTasks()
        {
            return _dataWrapper.Tasks.Where(c => c.Synced == false)
                               .ToList().ConvertAll(
                                   row => Task.Create(row.Guid,
                                                      row.Id,
                                                      row.Name,
                                                      row.Description,
                                                      _projectRepository.GetById(row.ProjectId),
                                                      row.CreateDate,
                                                      row.Synced,
                                                      row.SyncInfo,
                                                      row.Inactive, (TimeRegistrationTypeEnum) row.TimeRegistionTypeId));
        }

        public bool Exists(Guid guid)
        {
            return _dataWrapper.GetTaskByGuid(guid) != null;

        }
    }
}