using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace RegistrationWebsite.Helpers
{
    public static class LogHelper
    {

        public static void LogError(Exception exception)
        {

            var entry = new LogEntry()
                {
                    Message = exception.ToString(),
                    Severity = TraceEventType.Error

                };

            entry.Categories.Add("Exceptions");

            Logger.Write(entry);

        }

    }
}