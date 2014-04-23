using System.Collections.Generic;
using AutoMapper;
using Trex.Common.DataTransferObjects;
using Trex.SmartClient.Forecast.ForecastOverview;
using Trex.SmartClient.Forecast.Shared;

namespace Trex.SmartClient.Forecast
{
    public static class ForecastAutoMapperExtensions
    {
        static ForecastAutoMapperExtensions()
        {
            // Bi-directional
            Mapper.CreateMap<ForecastType, ForecastTypeDto>().ReverseMap();

            // To client
            Mapper
                .CreateMap<ForecastProjectHoursDto, ForecastOverviewProjectHours>()
                .ForMember(x => x.ProjectName, s => s.MapFrom(x => x.Project.Name))
                .ForMember(x => x.ProjectId, s => s.MapFrom(x => x.Project.Id))
                .ForMember(x => x.CompanyId, s => s.MapFrom(x => x.Project.CompanyDto.Id));

            Mapper.CreateMap<ForecastDto, ForecastOverviewForecast>()
                .ForMember(x => x.ForecastId, s => s.MapFrom(x => x.Id))
                .ForMember(x => x.ForecastType, s => s.MapFrom(x => x.ForecastType))
                .ForMember(x => x.Projects, s => s.MapFrom(x => x.ForecastProjectHoursDtos))
                .ForMember(x => x.Date, s => s.MapFrom(x => new ForecastDate(x.Date)))
                .ForMember(x => x.DisplayHandler, s => s.Ignore())
                .ForMember(x => x.Disposed, s => s.Ignore());

            Mapper.AssertConfigurationIsValid();
        }

        #region ForecastType

        public static List<ForecastType> ToClient(this IEnumerable<ForecastTypeDto> src)
        {
            return Mapper.Map<IEnumerable<ForecastTypeDto>, List<ForecastType>>(src);
        }

        public static ForecastTypeDto ToServerDto(this ForecastType src)
        {
            return Mapper.Map<ForecastType, ForecastTypeDto>(src);
        }

        #endregion

        #region Project hours

        public static ForecastOverviewProjectHours ToClient(this ForecastProjectHoursDto src)
        {
            return Mapper.Map<ForecastProjectHoursDto, ForecastOverviewProjectHours>(src);
        }

        #endregion

        #region Overview forecast

        public static ForecastOverviewForecast ToClient(this ForecastDto src)
        {
            var newForecast = Mapper.Map<ForecastDto, ForecastOverviewForecast>(src);
            return newForecast;
        }

        #endregion
    }
}