using System;

namespace Trex.Core.Services
{
    public interface IExceptionHandlerService
    {
        void OnError(Exception exception);
    }
}
