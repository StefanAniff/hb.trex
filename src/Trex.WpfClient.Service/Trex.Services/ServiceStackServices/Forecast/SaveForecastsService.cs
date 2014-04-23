using System;
using System.Linq;
using Trex.Common.ServiceStack;
using Trex.Server.Core.Model.Forecast;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.ServiceStack;
using Trex.Server.Infrastructure.UnitOfWork;

namespace TrexSL.Web.ServiceStackServices.Forecast
{    
    public class SaveForecastsService : NhServiceBasePost<SaveForecastsRequest>
    {
        private readonly IForecastTypeRepository _forecastTypeRepository;
        private readonly IForecastMonthFactory _forecastMonthFactory;
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IForecastMonthRepository _forecastMonthRepository;
        private readonly IActiveSessionManager _activeSessionManager;

        public SaveForecastsService(IForecastTypeRepository forecastTypeRepository
            , IForecastMonthFactory forecastMonthFactory
            , IUserRepository userRepository
            , IProjectRepository projectRepository
            , IForecastMonthRepository forecastMonthRepository
            , IActiveSessionManager activeSessionManager)
        {
            _forecastTypeRepository = forecastTypeRepository;
            _forecastMonthFactory = forecastMonthFactory;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _forecastMonthRepository = forecastMonthRepository;
            _activeSessionManager = activeSessionManager;
        }

        protected override object Send(SaveForecastsRequest request)
        {
            return PostValue(request);
        }

        private SaveForecastsResponse PostValue(SaveForecastsRequest request)
        {
            /**
             * Save strategy is to delete all in month
             * and create new for month.
             */

            var monthDto = request.ForecastMonthDto;
            var user = _userRepository.GetByUserID(monthDto.UserId);
            var creator = _userRepository.GetByUserID(monthDto.CreatedById);
            var forecastMonth = _forecastMonthRepository.GetById(monthDto.Id) ??
                                _forecastMonthFactory.CreateForecastMonth(monthDto.Month, monthDto.Year, user, creator);

            EnsureNotLocked(forecastMonth);

            var forecastTypes = _forecastTypeRepository.GetAll();

            // Cleanup and initialize
            forecastMonth.SetCreationDetails(creator);
            forecastMonth.ClearForecasts();
            _forecastMonthRepository.SessionFlush();
            
            // Map ForecastDtos
            foreach (var forecastDto in monthDto.ForecastDtos)
            {
                var forecastType = forecastTypes.Single(y => y.Id.Equals(forecastDto.ForecastType.Id));
                var newForecast = forecastMonth.AddForecast(forecastDto.Date, forecastType, forecastDto.DedicatedForecastTypeHours);                    

                newForecast.ClearProjectRegistrations();
                foreach (var projectHoursDto in forecastDto.ForecastProjectHoursDtos)
                {
                    var customer = _projectRepository.GetById(projectHoursDto.Project.Id);
                    newForecast.AddProjectRegistration(customer, projectHoursDto.Hours);
                }
            }

            _forecastMonthRepository.SaveOrUpdate(forecastMonth);
            return new SaveForecastsResponse { ForecastMonthId = forecastMonth.Id };
        }

        private void EnsureNotLocked(ForecastMonth forecastMonth)
        {
            if (forecastMonth.IsLocked)
                throw new Exception(string.Format("Cannot update/create workplan for month: {0} year: {1}, since it is locked",
                                                  forecastMonth.Month, forecastMonth.Year));
        }
    }
}