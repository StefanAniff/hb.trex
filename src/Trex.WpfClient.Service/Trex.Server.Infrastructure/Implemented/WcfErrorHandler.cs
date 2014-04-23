using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using log4net;

namespace Trex.Server.Infrastructure.Implemented
{
    public class WcfErrorHandler : IErrorHandler
    {
        private static readonly ILog Log = LogManager.GetLogger("TRex." + typeof(RoleManagementService).Name);
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
        }

        public bool HandleError(Exception error)
        {
            Log.Error(error);
            return false;
        }
    }
}
