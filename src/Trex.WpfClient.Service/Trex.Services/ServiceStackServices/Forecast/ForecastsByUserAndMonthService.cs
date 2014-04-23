using System;
using System.Collections.Generic;
using AutoMapper;
using Trex.Common.DataTransferObjects;
using Trex.Common.ServiceStack;
using Trex.Server.Core.Model;
using Trex.Server.Core.Model.Forecast;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.ServiceStack;
using System.Linq;

namespace TrexSL.Web.ServiceStackServices.Forecast
{
    public class ForecastsByUserAndMonthService : NhServiceBasePost<ForecastsByUserAndMonthRequest>
    {
        private readonly IForecastMonthRepository _forecastMonthRepository;
        private readonly IHolidayRepository _holidayRepository;
        private readonly IDomainSettings _domainSettings;

        public ForecastsByUserAndMonthService(IForecastMonthRepository forecastMonthRepository
            , IHolidayRepository holidayRepository
            , IDomainSettings domainSettings)
        {
            _forecastMonthRepository = forecastMonthRepository;
            _holidayRepository = holidayRepository;
            _domainSettings = domainSettings;
        }
        
        private ForecastsByUserAndMonthResponse GetValue(ForecastsByUserAndMonthRequest request)
        {
            var result = new ForecastsByUserAndMonthResponse();

            var forecastMonth = _forecastMonthRepository.GetByUserAndMonth(request.UserId, request.ForecastMonth, request.ForecastYear);
            result.ForecastMonth = ToForecastDtoObject(forecastMonth, request);
            
            var holidays = _holidayRepository.GetByMonth(request.HolidayMonth, request.HolidayYear);
            result.Holidays = holidays.Select(ToHolidayDtoObject).ToList();

            result.ProjectForecastTypeId = _domainSettings.ProjectForecastTypeId;

            return result;
        }

        private HolidayDto ToHolidayDtoObject(Holiday holiday)
        {
            return Mapper.Map<Holiday, HolidayDto>(holiday);
        }

        private ForecastMonthDto ToForecastDtoObject(ForecastMonth forecastMonth, ForecastsByUserAndMonthRequest request)
        {
            ForecastMonthDto result = null;
            if (forecastMonth == null)
            {
                // Can be null, if not entered earlier. Resolution add to db and set UnLocked to true
                result = new ForecastMonthDto
                    {
                        Id = 0,
                        Month = request.ForecastMonth,
                        Year = request.ForecastYear,
                        ForecastDtos = new List<ForecastDto>()
                    };
            }
            else
            {
                result = Mapper.Map<ForecastMonth, ForecastMonthDto>(forecastMonth);
            }

            SetDtoIsLocked(result, forecastMonth);

            return result;
        }

        /// <summary>
        /// Determines if month is locked.
        /// Business rule:
        /// If currentdate exceeds currentlockdate the previous month
        /// is by default locked unless stated explicitly that it is unlocked by UnLocked property
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="forecastMonth"></param>
        public virtual void SetDtoIsLocked(ForecastMonthDto dto, ForecastMonth forecastMonth)
        {
            var now = Now;

            // Is explicitly set to allow update
            if (forecastMonth != null)
            {
                dto.IsLocked = forecastMonth.IsLocked;
                return;
            }

            // Forecast is null. 
            var lockDate = ForecastMonth.CreateLockedFrom(dto.Month, dto.Year, _domainSettings.PastMonthsDayLock);
            dto.IsLocked = now > lockDate;
        }

        protected override object Send(ForecastsByUserAndMonthRequest request)
        {
            return GetValue(request);
        }

        // Poor mans injection for unittesting.
        private DateTime? _now;
        public virtual DateTime Now
        {
            get { return _now.HasValue ? _now.Value : DateTime.Now; }
            set { _now = value; }
        }
    }
}