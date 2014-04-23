using System;

namespace Trex.Core.Interfaces
{
    public interface IResult
    {
        Exception Error { get; set; }
        Action Completed { get; set; }
        void Execute();
    }
}