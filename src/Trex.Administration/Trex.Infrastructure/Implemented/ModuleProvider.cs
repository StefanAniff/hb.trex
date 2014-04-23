using System;
using System.Collections.Generic;
using Trex.Core.Interfaces;

namespace Trex.Infrastructure.Implemented
{
    public class ModuleProvider : IModuleProvider
    {
        #region IModuleProvider Members

        public List<Type> GetModules()
        {
            var modules = new List<Type>();

            modules.Add(Type.GetType("Trex.TaskAdministration.TaskAdministrationModule, Trex.TaskAdministration, Version=1.0.0.0, Culture=neutral", true));
            return modules;
        }

        #endregion
    }
}