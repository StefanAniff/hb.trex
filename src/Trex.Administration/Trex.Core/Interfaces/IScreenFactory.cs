using System;
using Microsoft.Practices.Prism.Regions;


namespace Trex.Core.Interfaces
{
    public interface IScreenFactory
    {
        IScreen CreateScreen(IRegion region, Guid guid);
    }
}