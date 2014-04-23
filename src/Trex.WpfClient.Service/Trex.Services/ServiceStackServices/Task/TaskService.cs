using ServiceStack.ServiceHost;
using System.Collections.Generic;
using Trex.Common.ServiceStack;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.ServiceStack;

namespace TrexSL.Web.ServiceStackServices.Task
{
    public class TaskService : NhServiceBase,
        IPost<TasksRequest>,
        IPost<GetTasksByIdRequest>,
        IGet<GetTasksByIdRequest>,
        IGet<TasksRequest>
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public object Post(GetTasksByIdRequest request)
        {
            var list = _taskRepository.GetByIds(request.TaskIds);
            var response = new GetTasksByIdResponse();
            response.Tasks = AutoMapper.Mapper.Map<List<Trex.Server.Core.Model.Task>, List<Trex.Common.DataTransferObjects.FullTaskDto>>(list);
            return response;
        }

        private TasksResponse TasksRequest(TasksRequest request)
        {
            var list = _taskRepository.GetByChangeDate(request.LastSyncDate);
            var response = new TasksResponse();
            response.Tasks = AutoMapper.Mapper.Map<List<Trex.Server.Core.Model.Task>, List<Trex.Common.DataTransferObjects.TaskDto>>(list);
            return response;
        }

        public object Post(TasksRequest request)
        {
            return TasksRequest(request);
        }

        public object Get(GetTasksByIdRequest request)
        {
            return Post(request);
        }

        public object Get(TasksRequest request)
        {
            return TasksRequest(request);
        }
    }
}