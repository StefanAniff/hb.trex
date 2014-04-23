using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using StructureMap;
using Trex.Server.Core.Services;
using Trex.Server.DataAccess;
using Trex.Server.Infrastructure.BaseClasses;
using Trex.ServiceContracts;

namespace Trex.Server.Infrastructure.Implemented
{
    public class ProjectService : LogableBase, IProjectService
    {
        private readonly ITrexContextProvider _entityContext;

        public ProjectService(ITrexContextProvider contextProvider)
        {
            _entityContext = contextProvider;
        }

        public Project SaveProject(Project project)
        {
            project.ChangeTracker.ChangeTrackingEnabled = true;
            using (var db = _entityContext.TrexEntityContext)
            {
                project.ChangeDate = DateTime.Now;
                project.CustomerInvoiceGroup = null;
                if (project.ProjectID != 0)
                    db.Projects.ApplyChanges(project);
                else
                {
                    project.ChangeTracker.ChangeTrackingEnabled = false;

                    project.CreateDate = DateTime.Now;
                    project.User = null;
                    project.Tasks = null;
                    db.Projects.AddObject(project);
                    db.Projects.ApplyChanges(project);

                    project.ChangeTracker.ChangeTrackingEnabled = true;
                }
                db.SaveChanges(SaveOptions.AcceptAllChangesAfterSave |SaveOptions.DetectChangesBeforeSave);
            }

            return project;

        }

        public bool DeleteProject(Project project)
        {
            using (var db = _entityContext.TrexEntityContext)
            {       
                //var project = GetProjectById(projectId, false, true, true, false);
                db.Projects.Attach(project);

                if (project.Tasks.Count == 0)
                {
                    db.Projects.DeleteObject(project);
                    db.SaveChanges();
                    return true;
                }
                return false;
                
            }

        }

        public List<Project> SearchProjects(string searchString)
        {
            using (var db = _entityContext.TrexEntityContext)
            {
                var searchProjects = (from project in db.Projects
                                      select new
                                      {
                                          Project = project,
                                          SearchString = project.ProjectName + " " + project.Customer.CustomerName
                                      });

                var searchArray = searchString.Split(" ".ToCharArray());
                searchArray = searchArray.Where(s => !String.IsNullOrEmpty(s)).ToArray();

                var result = new List<Project>();
                var query = from p in searchProjects select p;
                searchArray.ToList().ForEach(s => query = query.Where(st => st.SearchString.ToLower().Contains(s.ToLower())));
                result.AddRange(query.Take(10).ToList().Select(t => t.Project));

                foreach (var project in result)
                {
                    db.LoadProperty(project, "Customer");
                }

                return result;
                
            }

        }

        public Project GetProjectById(int projectId, bool includeParents, bool includeInactive, bool includeTasks, bool includeTimeEntries)
        {
            using (var db = _entityContext.TrexEntityContext)
            {
                var project = db.Projects;

                if (includeTasks)
                    project.Include("Tasks");

                if (includeTimeEntries)
                    project.Include("Tasks.TimeEntries");

                return project.SingleOrDefault(p => p.ProjectID == projectId);
                
            }


        }

        public List<Project> EntityProjectRequest(List<int> projectIds, bool includeParents, bool includeInactive, bool includeTasks, bool includeTimeEntries)
        {
            var projectList = new List<Project>();

            foreach (var projectId in projectIds)
            {
                var project = GetProjectById(projectId, includeParents, includeInactive, includeTasks, includeTimeEntries);
                projectList.Add(project);
            }
            return projectList.OrderBy(p => p.ProjectName).ToList();
        }
    }
}
