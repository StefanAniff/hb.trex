using System;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented
{
    public class TimeEntryTypeFactory : ITimeEntryTypeFactory
    {
        public TimeEntryType Create(string name, bool isDefault, bool isBillableByDefault, Company company)
        {
            return new TimeEntryType
                       {
                           Name = name,
                           IsBillableByDefault = isBillableByDefault,
                           IsDefault = isDefault,
                           Company = company
                       };
        }
    }
}
