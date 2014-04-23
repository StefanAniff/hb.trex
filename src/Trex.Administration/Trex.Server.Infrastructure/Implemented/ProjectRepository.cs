using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using Trex.Server.Core.Exceptions;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented
{
    public class ProjectRepository : RepositoryBase, IProjectRepository
    {
        #region IProjectRepository Members

        public List<Project> GetAll(bool includeInactive)
        {
            var session = GetSession();

            var allProjects = from projects in session.Linq<Project>()
                              orderby projects.Customer.Name , projects.Name
                              select projects;

            if (!includeInactive)
            {
                var filteredProjects = from projects in allProjects
                                       where projects.Inactive == false
                                       select projects;
                return filteredProjects.ToList();
            }
            else
            {
                return allProjects.ToList();
            }
        }

        public List<Project> GetAllPreloaded()
        {
            var session = GetSession();
            var allProjects = session.CreateQuery(
                "select projects"
                + " from Project prj"
                + " join fetch prj.Customer cst"
                + " join fetch prj.Tasks tsk"
                + " join fetch tsk.TimeEntries te"
                ).List<Project>();
            return allProjects.ToList();
        }

        public List<Project> GetByCustomer(Customer customer, bool includeInactive)
        {
            throw new NotImplementedException();
        }

        public Project GetByID(int projectID)
        {
            var session = GetSession();

            return session.Get<Project>(projectID);
        }

        public Project GetByGuid(Guid guid)
        {
            var session = GetSession();

            var project = from projects in session.Linq<Project>()
                          where projects.Guid == guid
                          select projects;

            return project.First();
        }

        public IList<Project> GetProjectsFilteredByUserPreferences(IUserPreferences userPreferences, User user)
        {
            //if (userPreferences.ShowAllTasks)
            //    return GetAll(false);
            //else
            return user.Projects;
        }

        /// <summary>
        /// Gets projects changed after a given date
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <returns></returns>
        public IList<Project> GetByChangeDate(DateTime startDate)
        {
            var session = GetSession();

            var project = from projects in session.Linq<Project>()
                          where projects.CreateDate >= startDate || (projects.ChangeDate != null && projects.ChangeDate >= startDate)
                          select projects;

            return project.ToList();
        }

        /// <summary>
        /// Updates the specified project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <exception cref="EntityUpdateException"></exception>
        public void Update(Project project)
        {
            var session = GetSession();

            try
            {
                project.ChangeDate = DateTime.Now;
                session.Transaction.Begin();
                session.SaveOrUpdate(project);
                session.Flush();
                session.Transaction.Commit();
            }
            catch (HibernateException ex)
            {
                session.Transaction.Rollback();
                throw new EntityUpdateException("Error updating project", ex);
            }
        }

        public void Delete(Project project)
        {
            var session = GetSession();
            try
            {
                session.Transaction.Begin();
                session.Delete(project);
                session.Flush();
                session.Transaction.Commit();
            }
            catch (HibernateException ex)
            {
                session.Transaction.Rollback();
                throw new EntityDeleteException("Error deleting Project", ex);
            }
        }

        public void Delete(int projectId)
        {
            Delete(GetByID(projectId));
        }

        public List<Project> SearchProject(string searchString)
        {
            var allProjects = GetAll(false);

            var searchProjects =
                from project in allProjects
                select
                    new
                        {
                            Project = project,
                            SearchString = string.Concat(project.Name, " ", project.Customer.Name)
                        };

            var searchArray = searchString.Split(" ".ToCharArray());
            searchArray = searchArray.Where(s => !String.IsNullOrEmpty(s)).ToArray();
            var result = new List<Project>();
            var query = from t in searchProjects select t;

            searchArray.ToList().ForEach(s =>
                                         query = query.Where(st => st.SearchString.ToLower().Contains(s.ToLower())));

            result.AddRange(query.ToList().Select(t => t.Project));

            return result;
        }

        #endregion
    }
}