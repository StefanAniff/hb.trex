using Trex.Core.Interfaces;

namespace Trex.Infrastructure.Implemented
{
    public class MenuIndexService : IMenuIndexService
    {
        //Todo: Get from config file
        private readonly string[] _topMenuItems = {"Task Management", "Reports", "Invoices", "Administration"};

        #region IMenuIndexService Members

        public int GetTopMenuIndex(IMenuInfo menuInfo)
        {
            for (var i = 0; i < _topMenuItems.Length; i++)
            {
                if (menuInfo.DisplayName == _topMenuItems[i])
                {
                    return i;
                }
            }
            return _topMenuItems.Length;
        }

        #endregion
    }
}