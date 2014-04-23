using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.ServiceHost;
using Trex.Common.ServiceStack;
using Trex.Server.Infrastructure.ServiceStack;

namespace TrexSL.Web.ServiceStackServices.GeneralSettings
{
    public class GeneralSettingsService : NhServiceBase<GetGeneralSettingsRequest>,
        IGet<GetGeneralSettingsRequest>
    {
        protected override object Send(GetGeneralSettingsRequest request)
        {
            throw new NotImplementedException();
        }

        public object Get(GetGeneralSettingsRequest request)
        {
            throw new NotImplementedException();
        }
    }
}