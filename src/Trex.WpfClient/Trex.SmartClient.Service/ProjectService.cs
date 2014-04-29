using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trex.Common.DataTransferObjects.ProjectAdministration;
using Trex.Common.ServiceStack;
using Trex.SmartClient.Core.Exceptions;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Service.Helpers;
using Project = Trex.SmartClient.Core.Model.Project;


namespace Trex.SmartClient.Service
{
    public class ProjectService : ClientServiceBase, IProjectService
    {
        private readonly IUserSession _userSession;
        private readonly ICompanyRepository _companyRepository;
        private readonly ServiceFactory _serviceFactory;

        public ProjectService(IUserSession userSession, ICompanyRepository companyRepository, ServiceFactory serviceFactory)
        {
            _userSession = userSession;
            _companyRepository = companyRepository;
            _serviceFactory = serviceFactory;
        }

        public async Task<List<Project>> GetUnsynced(DateTime lastSyncDate)
        {
            using (var client = _serviceFactory.GetServiceClient(_userSession.LoginSettings))
            {
                try
                {
                    var projectList = await client.GetUnsyncedProjectsAsync(lastSyncDate.AddDays(-1));
                    var returnList = new List<Project>();

                    foreach (var project in projectList)
                    {
                        var company = _companyRepository.GetById(project.CompanyId);

                        if (company != null)
                        {
                            returnList.Add(Project.Create(project.Id, project.Name, company, project.Inactive));
                        }
                        else
                        {
                            throw new MissingHieracleDataException(string.Format("Could not find CompanyId {0} for project '{1} ({2})'", project.CompanyId, project.Name, project.Id));
                        }
                    }
                    return returnList;
                }
                catch (MissingHieracleDataException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new ServiceAccessException("Error contacting service", ex);
                }
            }
        }
    }
}
