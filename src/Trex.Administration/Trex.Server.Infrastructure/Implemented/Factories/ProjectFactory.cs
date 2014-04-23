using System;
using Trex.Server.Core.Exceptions;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented.Factories
{
    public class ProjectFactory : IProjectFactory
    {
        #region IProjectFactory Members

        public Project Create(string projectName, Customer customer, User createdBy, bool isEstimatesEnabled)
        {
            if (string.IsNullOrEmpty(projectName))
            {
                throw new ParameterNullOrEmptyException("Project name cannot be null or empty");
            }

            if (customer == null)
            {
                throw new ParameterNullOrEmptyException("Customer cannot be null");
            }

            if (createdBy == null)
            {
                throw new ParameterNullOrEmptyException("User cannot be null");
            }

            return new Project(projectName, customer, createdBy, DateTime.Now, DateTime.Now, false, isEstimatesEnabled);
        }

        #endregion
    }
}