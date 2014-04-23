using System;

namespace Trex.Core.Exceptions
{
    public class VersionException : ApplicationBaseException
    {
        public VersionException() {}
        public VersionException(string message) : base(message) {}
        public VersionException(string message, Exception inner) : base(message, inner) {}
    }
}