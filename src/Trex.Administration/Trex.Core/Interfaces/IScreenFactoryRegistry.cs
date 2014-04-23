using System;

namespace Trex.Core.Interfaces
{
    public interface IScreenFactoryRegistry
    {
        int Count { get; }
        void RegisterFactory(Guid guid, IScreenFactory screenFactory);
        IScreenFactory GetFactory(Guid guid);
    }
}