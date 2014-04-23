using System;
using ServiceStack.ServiceHost;
using Trex.Common.ServiceStack;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.ServiceStack;

namespace TrexSL.Web.ServiceStackServices.GeneralSettingss
{
    public class GeneralSettingsService : NhServiceBase, IGet<GetGeneralSettingsRequest>,
        IGet<GetUserStatisticsRequest>
    {
        private readonly IDomainSettings _domainSettings;
        private readonly ITimeEntryRepository _timeEntryRepository;

        public GeneralSettingsService(IDomainSettings domainSettings, ITimeEntryRepository timeEntryRepository)
        {
            _domainSettings = domainSettings;
            _timeEntryRepository = timeEntryRepository;
        }

        public object Get(GetGeneralSettingsRequest request)
        {
            return new GetGeneralSettingsResponse
            {
                TimeEntryMinStartDate = _domainSettings.TimeEntryMinStartDate
            };
        }

        public object Get(GetUserStatisticsRequest request)
        {
            return new GetUserStatisticsResponse
            {
                UserStatistics = GetUserStatistics(request.UserId, request.NumberOfDaysBack),
            };
        }

        private UserStatistics GetUserStatistics(int userId, int numOfDaysBack)
        {
            var info = new UserStatistics();
            var firstDayOfWeek = DateTime.Now;
            var firstDayOfMonth = DateTime.Now;

            while (firstDayOfWeek.DayOfWeek != DayOfWeek.Monday)
            {
                firstDayOfWeek = firstDayOfWeek.AddDays(-1);
            }

            while (firstDayOfMonth.Day != 1)
            {
                firstDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            }

            info.RegisteredHoursThisWeek = _timeEntryRepository.GetRegisteredHours(firstDayOfWeek.Date, DateTime.Now, userId);
            info.RegisteredHoursToday = _timeEntryRepository.GetRegisteredHours(DateTime.Now.Date, DateTime.Now, userId);
            info.RegisteredHoursThisMonth = _timeEntryRepository.GetRegisteredHours(firstDayOfMonth.Date, DateTime.Now, userId);
            info.EarningsToday = _timeEntryRepository.GetEarningsByUser(DateTime.Now.Date, DateTime.Now, userId);
            info.EarningsThisWeek = _timeEntryRepository.GetEarningsByUser(firstDayOfWeek.Date, DateTime.Now, userId);
            info.EarningsThisMonth = _timeEntryRepository.GetEarningsByUser(firstDayOfMonth.Date, DateTime.Now, userId);
            info.BillableHoursToday = _timeEntryRepository.GetBillableHours(DateTime.Now.Date, DateTime.Now, userId);
            info.BillableHoursThisWeek = _timeEntryRepository.GetBillableHours(firstDayOfWeek.Date, DateTime.Now, userId);
            info.BillableHoursThisMonth = _timeEntryRepository.GetBillableHours(firstDayOfMonth.Date, DateTime.Now, userId);

            return info;
        }


    }
}