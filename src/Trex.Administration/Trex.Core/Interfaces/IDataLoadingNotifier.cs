using System;

namespace Trex.Core.Interfaces
{
    public interface IDataLoadingNotifier
    {
        void NotifySystemLoadingData();
        void NotifySystemIdle();
        void HandleLoadFailed(Exception exception);
    }
}