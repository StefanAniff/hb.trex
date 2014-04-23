using Trex.SmartClient.Core.Interfaces;

namespace Trex.SmartClient.Infrastructure.Implemented
{
    public class RegionNames : IRegionNames
    {
        private const string DIALOG_REGION = "DialogRegion";
        private const string MAIN_REGION = "MainRegion";
        private const string BUSY_REGION = "BusyRegion";
        private const string SubMenuRegion = "SubmenuPanelRegion";
        private const string BOTTOM_TAB_REGION = "BottomTabRegion";

        public string DialogRegion
        {
            get { return DIALOG_REGION; }
        }

        public string MainRegion
        {
            get { return MAIN_REGION; }
        }

        public string BusyRegion
        {
            get { return BUSY_REGION; }
        }

        public string SubmenuRegion
        {
            get { return SubMenuRegion; }
        }

        public string BottomTabRegion
        {
            get { return BOTTOM_TAB_REGION; }
        }
    }
}