using System;

namespace Trex.Core.Interfaces
{
    public interface IResponse<T>
    {
        T Result { get; }
        Exception Error { get; }
    }
}