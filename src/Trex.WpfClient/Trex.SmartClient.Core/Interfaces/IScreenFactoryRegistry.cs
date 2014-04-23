using System;

namespace Trex.SmartClient.Core.Interfaces
{
    public interface IScreenFactoryRegistry
    {

        void RegisterFactory(Guid guid, IScreenFactory screenFactory);
        IScreenFactory GetFactory(Guid guid);
        int Count{ get; }
    }
}