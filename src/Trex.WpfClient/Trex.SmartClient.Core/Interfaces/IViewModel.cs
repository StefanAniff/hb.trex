using System;

namespace Trex.SmartClient.Core.Interfaces
{
    public interface IViewModel : IDisposable
    {
    }

    public interface IViewModelInitializable : IViewModel
    {
        void Initialize();
    }
}
