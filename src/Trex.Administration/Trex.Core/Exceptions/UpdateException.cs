using System;

namespace Trex.Core.Exceptions
{
    public class EntityUpdateException : ApplicationBaseException
    {
        public EntityUpdateException() {}
        public EntityUpdateException(string message) : base(message) {}
        public EntityUpdateException(string message, Exception inner) : base(message, inner) {}
    }
}