using System;
using Trex.Core.Interfaces;

namespace Trex.Core.Services
{
    public interface ITransitionService
    {
        void EnterViewAnimation(IView view);
        void LeaveViewAnimation(IView view, Action onLeaveComplete);
    }
}