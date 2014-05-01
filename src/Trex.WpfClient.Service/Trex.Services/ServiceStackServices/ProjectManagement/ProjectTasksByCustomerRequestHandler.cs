using System.Collections.Generic;
using Trex.Common.DataTransferObjects.ProjectAdministration;
using Trex.Common.ServiceStack;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.ServiceStack;
using System.Linq;
using TrexSL.Web.DataContracts;
using UserDto = Trex.Common.DataTransferObjects.ProjectAdministration.UserDto;

namespace TrexSL.Web.ServiceStackServices.ProjectManagement
{
    public class ProjectTasksByCustomerRequestHandler : NhServiceBasePost<ProjectTasksByCustomerRequest>
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectTasksByCustomerRequestHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        protected override object Send(ProjectTasksByCustomerRequest request)
        {
            return GetValue(request);
        }

        public ProjectTasksByCustomerResponse GetValue(ProjectTasksByCustomerRequest request)
        {
            var domProjects = _projectRepository.GetByCustomerId(request.CustomerId);
            var response = new ProjectTasksByCustomerResponse { Projects = new List<ProjectTaskDto>()};

            foreach (var domProject in domProjects)
            {
                var singleTask = GetAndEnsureSingleTask(domProject);

                var dto = new ProjectTaskDto
                    {
                        ProjectId = domProject.ProjectID,
                        Id = singleTask.TaskID,
                        Description = singleTask.Description,
                        Inactive = singleTask.Inactive,
                        Name = singleTask.TaskName,
                        Manager = new UserDto
                            {
                                Id = singleTask.ProjectManager.UserID
                                // IVA: DU ER KOMMET HERTIL
                            }
                    };
                response.Projects.Add(dto);
            }

            return response;
        }

        private static Trex.Server.Core.Model.Task GetAndEnsureSingleTask(Project domProject)
        {
            var task = domProject.Tasks.SingleOrDefault();

            // Assert only 1 task pr. project. H&B specification
            if (task == null)
                throw new DomainException(string.Format("Project with id {0} has NO or MULTIPLE tasks", domProject.ProjectID));

            return task;
        }
    }
}