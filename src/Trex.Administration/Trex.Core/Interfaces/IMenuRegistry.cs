using System.Collections.Generic;

namespace Trex.Core.Interfaces
{
    public interface IMenuRegistry : IEnumerable<IMenuInfo>
    {
        void RegisterMenuInfo(IMenuInfo menuInfo, string parent, int menuIndex);

        IMenuInfo GetStartPage();
    }
}