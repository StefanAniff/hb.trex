using System;
using Trex.Core.Interfaces;

namespace Trex.Core.Implemented
{
    public abstract class ResultBase : IResult
    {
        #region IResult Members

        public abstract void Execute();

        public Exception Error { get; set; }

        public Action Completed { get; set; }

        #endregion
    }
}