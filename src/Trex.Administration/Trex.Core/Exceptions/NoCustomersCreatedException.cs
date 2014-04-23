using System;

namespace Trex.Core.Exceptions
{
    public class NoCustomersCreatedException : ApplicationBaseException
    {
        public NoCustomersCreatedException() {}
        public NoCustomersCreatedException(string message) : base(message) {}
        public NoCustomersCreatedException(string message, Exception inner) : base(message, inner) {}
    }
}