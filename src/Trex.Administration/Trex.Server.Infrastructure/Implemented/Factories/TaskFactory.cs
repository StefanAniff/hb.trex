using System;
using Trex.Server.Core.Exceptions;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented.Factories
{
    public class TaskFactory : ITaskFactory
    {
        #region ITaskFactory Members

        /// <summary>
        /// Creates the specified Task.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="createdBy">The created by.</param>
        /// <param name="project">The project.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="parentTask">The parent task.</param>
        /// <returns></returns>
        /// <exception cref="ParameterNullOrEmptyException"></exception>
        public Task Create(string name, string description, User createdBy, Project project, Tag tag, Task parentTask)
        {
            return Create(name, description, createdBy, project, tag, parentTask, 0, 0, 0, 0, 0);
        }

        public Task Create(string name, string description, User createdBy, Project project, Tag tag, Task parentTask, double estimatePessimistic, double estimateOptimistic, double estimateRealistic,
                           double estimateCalculated, double timeLeft)
        {
            return Create(Guid.NewGuid(), DateTime.Now, DateTime.Now, name, description, createdBy, project, tag, parentTask);
        }

        /// <summary>
        /// Creates the specified Task.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <param name="createDate">The create date.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="createdBy">The created by.</param>
        /// <param name="project">The project.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="parentTask">The parent task.</param>
        /// <returns></returns>
        public Task Create(Guid guid, DateTime createDate, DateTime? changeDate, string name, string description, User createdBy, Project project, Tag tag, Task parentTask)
        {
            return Create(guid, createDate, changeDate, name, description, createdBy, project, tag, parentTask, 0, 0, 0, 0, 0);
        }

        /// <summary>
        /// Creates the specified Task.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <param name="createDate">The create date.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="createdBy">The created by.</param>
        /// <param name="project">The project.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="parentTask">The parent task.</param>
        /// <returns></returns>
        public Task Create(Guid guid, DateTime createDate, DateTime? changeDate, string name, string description, User createdBy, Project project, Tag tag, Task parentTask, double estimatePessimistic,
                           double estimateOptimistic, double estimateRealistic, double estimateCalculated, double timeLeft)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ParameterNullOrEmptyException("Name cannot by null or empty");
            }

            if (createdBy == null)
            {
                throw new ParameterNullOrEmptyException("User createdBy cannot be null");
            }

            if (project == null)
            {
                throw new ParameterNullOrEmptyException("Project cannot be null");
            }

            return new Task
                       {
                           Guid = guid,
                           CreateDate = createDate,
                           ChangeDate = changeDate,
                           Name = name,
                           Description = description,
                           CreatedBy = createdBy,
                           Project = project,
                           Tag = tag,
                           ParentTask = parentTask,
                           WorstCaseEstimate = estimatePessimistic,
                           BestCaseEstimate = estimateOptimistic,
                           RealisticEstimate = estimateRealistic,
                           TimeEstimated = estimateCalculated,
                           TimeLeft = timeLeft
                       };
        }

        #endregion
    }
}