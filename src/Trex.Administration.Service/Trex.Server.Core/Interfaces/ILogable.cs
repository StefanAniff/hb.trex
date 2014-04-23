using System;

namespace Trex.Server.Core.Interfaces
{
    public interface ILogable
    {
        void LogError(Exception exception);
        void LogMessage(string message);
    }
}
