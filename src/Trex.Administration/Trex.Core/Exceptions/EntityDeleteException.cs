using System;

namespace Trex.Core.Exceptions
{
    public class EntityDeleteException : ApplicationBaseException
    {
        public EntityDeleteException() {}
        public EntityDeleteException(string message) : base(message) {}
        public EntityDeleteException(string message, Exception inner) : base(message, inner) {}
    }
}