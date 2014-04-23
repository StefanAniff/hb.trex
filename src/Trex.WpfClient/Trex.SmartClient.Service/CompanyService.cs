using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Trex.SmartClient.Core.Exceptions;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Service.Helpers;
using Trex.SmartClient.Service.TrexPortalService;

namespace Trex.SmartClient.Service
{
    public class CompanyService : ClientServiceBase, ICompanyService
    {
        private readonly IUserSession _userSession;
        private readonly ServiceFactory _serviceFactory;

        public CompanyService(IUserSession userSession, ServiceFactory serviceFactory)
        {
            _userSession = userSession;
            _serviceFactory = serviceFactory;
        }

        public async Task<List<Company>> GetUnsynced(DateTime lastSyncDate)
        {
            using (var client = _serviceFactory.GetServiceClient(_userSession.LoginSettings))
            {
                try
                {
                    var companyList = await client.GetUnsyncedCompaniesAsync(lastSyncDate.AddDays(-1));
                    return companyList.Select(company => Company.Create(company.Name, company.Id, company.InheritsTimeEntryTypes, company.Inactive)).ToList();
                }
                catch (Exception ex)
                {
                    throw new ServiceAccessException("Error contacting service", ex);
                }
            }
        }		        
    }
}
