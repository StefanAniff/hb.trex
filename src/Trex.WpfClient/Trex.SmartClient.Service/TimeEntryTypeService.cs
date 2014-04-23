using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using Trex.SmartClient.Core.Exceptions;
using Trex.SmartClient.Core.Model;
using Trex.SmartClient.Core.Services;
using Trex.SmartClient.Service.Helpers;

namespace Trex.SmartClient.Service
{
    public class TimeEntryTypeService : ITimeEntryTypeService
    {
        private readonly IUserSession _userSession;
        private readonly ServiceFactory _serviceFactory;

        public TimeEntryTypeService(IUserSession userSession, ServiceFactory serviceFactory)
        {
            _userSession = userSession;
            _serviceFactory = serviceFactory;
        }

        public async Task<List<TimeEntryType>> GetAllTimeEntryTypes()
        {
            using (var client = _serviceFactory.GetServiceClient(_userSession.LoginSettings))
            {
                try
                {
                    var timeEntryTypes = await client.GetAllTimeEntryTypesAsync();

                    var returnList = new List<TimeEntryType>();
                    foreach (var timeEntryType in timeEntryTypes)
                    {
                        returnList.Add(TimeEntryType.Create(
                                           timeEntryType.Id,
                                           timeEntryType.IsDefault,
                                           timeEntryType.IsBillableByDefault,
                                           timeEntryType.Name,
                                           timeEntryType.CustomerId
                                           ));
                    }

                    return returnList;
                }
                catch (CommunicationException ex)
                {
                    throw new ServiceAccessException("Error when uploading timeentries", ex);
                }
            }
        }

        public static TrexPortalService.TimeEntryTypeDto ConvertFromModelToService(TimeEntryType timeEntryType)
        {
            return new TrexPortalService.TimeEntryTypeDto()
                                 {
                                     Name = timeEntryType.Name,
                                     CustomerId = timeEntryType.CustomerId,
                                     IsBillableByDefault = timeEntryType.IsBillableByDefault,
                                     IsDefault = timeEntryType.IsDefault,
                                     Id = timeEntryType.Id
                                 };
        }
    }
}
