using System.Collections.Generic;
using System.Linq;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Data;

namespace Trex.SmartClient.Infrastructure.Implemented.LocalStorage
{
    public class ClientProjectRepository : IProjectRepository
    {
        private readonly DataSetWrapper _dataWrapper;
        private readonly ICompanyRepository _companyRepository;

        public ClientProjectRepository(ICompanyRepository companyRepository, DataSetWrapper dataSetWrapper)
        {
            _dataWrapper = dataSetWrapper;
            _companyRepository = companyRepository;
        }

        public Project GetById(int id)
        {
            TimeTrackerDataSet.ProjectsRow projectsRow = _dataWrapper.Projects.SingleOrDefault(row => row.Id == id);
            if (projectsRow != null)
            {
                return Project.Create(projectsRow.Id, projectsRow.Name, _companyRepository.GetById(projectsRow.CustomerId),projectsRow.Inactive);
            }

            return null;
        }

        public List<Project> GetAll()
        {
            List<Project> returnList = new List<Project>();
            foreach (TimeTrackerDataSet.ProjectsRow row in _dataWrapper.Projects)
            {
                returnList.Add(Project.Create(row.Id, row.Name, _companyRepository.GetById(row.CustomerId),row.Inactive));

            }
            return returnList;
        }

        public List<Project> GetByCompany(Company company)
        {
            if (company == null)
                return new List<Project>();

            List<TimeTrackerDataSet.ProjectsRow> projectsRows = _dataWrapper.Projects.Where(row => row.CustomerId == company.Id).ToList();
            List<Project> returnList = new List<Project>();

            foreach (TimeTrackerDataSet.ProjectsRow row in projectsRows)
            {
                returnList.Add(Project.Create(row.Id, row.Name, company,row.Inactive));

            }

            return returnList;
        }

        public List<Project> GetBySearchStringAndCompany(string searchString, Company company)
        {

            if (string.IsNullOrEmpty(searchString))
                return GetByCompany(company);

            if (company == null)
                return new List<Project>();


            return _dataWrapper.Projects.Where(row => row.CustomerId == company.Id && row.Name.ToLower().Contains(searchString.ToLower())).ToList()
                .ConvertAll(projectRow =>
                    Project.Create(projectRow.Id, projectRow.Name, _companyRepository.GetById(projectRow.CustomerId),projectRow.Inactive)
                );




        }

        /// <summary>
        /// Returns active projects by client- and projectname
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public List<Project> GetBySearchString(string searchString)
        {
            var result =
                from proj in _dataWrapper.Projects
                where
                    ((_companyRepository.GetById(proj.CustomerId).Name.ToLower().Contains(searchString.ToLower())
                    || proj.Name.ToLower().Contains(searchString.ToLower()))
                    && !proj.Inactive) 
                select proj;

            return result
                    .Select(x => Project.Create(x.Id, x.Name, _companyRepository.GetById(x.CustomerId), x.Inactive))
                    .OrderBy(x => x.Company.Name)
                    .ThenBy(x => x.Name)
                    .ToList();
        }

        public List<Project> GetAllActive()
        {
            return GetAll()
                    .Where(x => !x.Inactive)
                    .ToList();
        }

        public void AddOrUpdate(Project project)
        {
            AddOrUpdateInternal(project);
            _dataWrapper.Save();
        }

        public void AddOrUpdate(List<Project> projects)
        {
            if (!projects.Any())
            {
                return;
            }
            foreach (var project in projects)
            {
                AddOrUpdateInternal(project);

            }
            _dataWrapper.Save();
        }

        private void AddOrUpdateInternal(Project project)
        {
            _dataWrapper.SaveProject(project);

        }
    }
}
