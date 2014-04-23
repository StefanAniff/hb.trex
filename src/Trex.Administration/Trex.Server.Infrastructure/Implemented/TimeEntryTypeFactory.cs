using Trex.Server.Core.Model;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented
{
    public class TimeEntryTypeFactory : ITimeEntryTypeFactory
    {
        #region ITimeEntryTypeFactory Members

        public TimeEntryType Create(string name, bool isDefault, bool isBillableByDefault, Customer customer)
        {
            return new TimeEntryType
                       {
                           Name = name,
                           IsBillableByDefault = isBillableByDefault,
                           IsDefault = isDefault,
                           Customer = customer
                       };
        }

        #endregion
    }
}