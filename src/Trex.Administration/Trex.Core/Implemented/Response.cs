using System;
using Trex.Core.Interfaces;

namespace Trex.Core.Implemented
{
    public class Response<T> : IResponse<T>
    {
        public Response(T result, Exception error)
        {
            Result = result;
            Error = error;
        }

        #region IResponse<T> Members

        public T Result { get; private set; }

        public Exception Error { get; private set; }

        #endregion
    }
}