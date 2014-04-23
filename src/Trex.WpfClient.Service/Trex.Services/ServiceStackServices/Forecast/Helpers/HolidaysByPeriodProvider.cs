using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Trex.Common.DataTransferObjects;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;

namespace TrexSL.Web.ServiceStackServices.Forecast.Helpers
{
    /// <summary>
    /// Helper for getting holidays in a given month and year
    /// </summary>
    public class HolidaysByPeriodProvider
    {
        private readonly IHolidayRepository _holidayRepository;

        public HolidaysByPeriodProvider(IHolidayRepository holidayRepository)
        {
            _holidayRepository = holidayRepository;
        }

        public virtual List<HolidayDto> GetHolidays(int month, int year)
        {
            return _holidayRepository.GetByMonth(month, year)
                                     .Select(Mapper.Map<Holiday, HolidayDto>)
                                     .ToList();
        } 
    }
}