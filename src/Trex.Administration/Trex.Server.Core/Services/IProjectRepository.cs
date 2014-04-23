using System;
using System.Collections.Generic;
using Trex.Server.Core.Model;

namespace Trex.Server.Core.Services
{
    public interface IProjectRepository
    {
        List<Project> GetAll(bool includeInactive);
        List<Project> GetAllPreloaded();
        List<Project> GetByCustomer(Customer customer, bool includeInactive);
        Project GetByID(int projectID);
        Project GetByGuid(Guid guid);
        void Update(Project project);
        IList<Project> GetProjectsFilteredByUserPreferences(IUserPreferences userPreferences, User user);
        IList<Project> GetByChangeDate(DateTime startDate);
        void Delete(Project project);
        void Delete(int projectId);
        List<Project> SearchProject(string searchString);
    }
}