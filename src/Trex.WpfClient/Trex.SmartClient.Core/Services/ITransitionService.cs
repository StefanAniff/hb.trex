using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.Core.Services
{
    public interface ITransitionService
    {
        void EnterViewAnimation(IView view);
        void LeaveViewAnimation(IView view, Action onLeaveComplete);
    }
}
