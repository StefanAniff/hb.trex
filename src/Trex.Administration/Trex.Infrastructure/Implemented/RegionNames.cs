using Trex.Core.Interfaces;

namespace Trex.Infrastructure.Implemented
{
    public class RegionNames : IRegionNames
    {
        private const string MENU_REGION = "MenuRegion";
        private const string MAIN_REGION = "MainRegion";
        private const string BUSY_REGION = "BusyRegion";

        #region IRegionNames Members

        public string MenuRegion
        {
            get { return MENU_REGION; }
        }

        public string MainRegion
        {
            get { return MAIN_REGION; }
        }

        public string BusyRegion
        {
            get { return BUSY_REGION; }
        }

        #endregion
    }
}