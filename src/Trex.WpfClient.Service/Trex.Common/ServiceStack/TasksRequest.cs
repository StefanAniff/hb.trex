using System;
using System.Collections.Generic;
using ServiceStack.ServiceHost;
using Trex.Common.DataTransferObjects;

namespace Trex.Common.ServiceStack
{
    public class TasksRequest : ReadonlyRequest, IReturn<TasksResponse>
    {
        public DateTime? LastSyncDate { get; set; }
    }


    public class GetTasksByIdRequest : ReadonlyRequest, IReturn<GetTasksByIdResponse>
    {
        public List<int> TaskIds { get; set; }

        public GetTasksByIdRequest()
        {
            TaskIds = new List<int>();
        }
    }


    public class TasksResponse
    {
        public TasksResponse()
        {
            Tasks = new List<TaskDto>();
        }

        public List<TaskDto> Tasks { get; set; }
    }

    public class GetTasksByIdResponse
    {
        public GetTasksByIdResponse()
        {
            Tasks = new List<FullTaskDto>();
        }

        public List<FullTaskDto> Tasks { get; set; }
    }
}