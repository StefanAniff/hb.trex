namespace Trex.Infrastructure.Implemented
{
    public class AppState
    {
        private static readonly AppState _instance = new AppState();

        private AppState() {}

        public string AppVirtualPath { get; set; }

        public static AppState Instance
        {
            get { return _instance; }
        }
    }
}