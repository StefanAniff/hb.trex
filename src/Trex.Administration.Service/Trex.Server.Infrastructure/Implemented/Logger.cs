using System;
using System.Reflection;
using log4net;

namespace Trex.Server.Infrastructure.Implemented
{
    public static class Logger
    {
        public static void Log(string message)
        {
            var logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            logger.Info(message);

        }

        public static void LogError(string message, Exception exception)
        {
            var logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            logger.Error(message,exception);
            
        }
    }
}