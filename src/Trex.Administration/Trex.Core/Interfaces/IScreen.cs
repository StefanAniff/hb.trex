using System;

namespace Trex.Core.Interfaces
{
    public interface IScreen
    {
        Guid Guid { get; }
        IView MasterView { get; }
        bool IsActive { get; set; }
    }
}