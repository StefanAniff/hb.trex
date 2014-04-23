using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Trex.Common.DataTransferObjects;
using Trex.Common.ServiceStack;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.ServiceStack;

namespace TrexSL.Web.ServiceStackServices.Forecast
{
    public class ForecastSearchOptionsService : NhServiceBasePost<ForecastSearchOptionsRequest>
    {
        private readonly IUserRepository _userRepository;

        public ForecastSearchOptionsService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        protected override object Send(ForecastSearchOptionsRequest request)
        {
            var result = new ForecastSearchOptionsResponse
                {
                    Users = new List<ForecastUserDto>(_userRepository.GetActiveUsers().Select(Mapper.Map<User, ForecastUserDto>))
                };

            return result;
        }
    }
}