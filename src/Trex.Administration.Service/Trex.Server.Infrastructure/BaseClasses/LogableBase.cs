using System;
using System.Reflection;
using Trex.Server.Core.Interfaces;
using log4net;

namespace Trex.Server.Infrastructure.BaseClasses
{
    public class LogableBase : ILogable
    {
        public void LogError(Exception exception)
        {
            var logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            logger.Error(exception);
        }

        public void LogMessage(string message)
        {
            var logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            logger.Info(message);
        }
    }
}
