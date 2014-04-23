using System;

namespace Trex.Core.Exceptions
{
    public class NotFoundByIDException : ApplicationBaseException
    {
        public NotFoundByIDException() {}
        public NotFoundByIDException(string message) : base(message) {}
        public NotFoundByIDException(string message, Exception inner) : base(message, inner) {}

        public int Id { get; set; }
    }
}