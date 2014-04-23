using System.Collections.Generic;
using AutoMapper;
using Trex.Common.DataTransferObjects;
using Trex.Common.ServiceStack;
using Trex.Server.Core.Model.Forecast;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.ServiceStack;
using System.Linq;

namespace TrexSL.Web.ServiceStackServices.Forecast
{
    public class AllForecastTypesService : NhServiceBasePost<ForecastTypesRequest>
    {
        private readonly IForecastTypeRepository _forecastTypeRepository;

        public AllForecastTypesService(IForecastTypeRepository forecastTypeRepository)
        {
            _forecastTypeRepository = forecastTypeRepository;
        }

        private ForecastTypesResponse GetValue(ForecastTypesRequest _)
        {            
            var forecastTypes = _forecastTypeRepository.GetAll();
            return new ForecastTypesResponse { ForecastTypeDtos = new List<ForecastTypeDto>(forecastTypes.Select(Mapper.Map<ForecastType, ForecastTypeDto>))};   
        }

        protected override object Send(ForecastTypesRequest request)
        {
            return GetValue(request);
        }
    }
}