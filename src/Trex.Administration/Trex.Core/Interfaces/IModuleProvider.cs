using System;
using System.Collections.Generic;

namespace Trex.Core.Interfaces
{
    public interface IModuleProvider
    {
        List<Type> GetModules();
    }
}