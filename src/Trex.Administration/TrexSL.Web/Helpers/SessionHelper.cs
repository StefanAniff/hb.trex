using StructureMap;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;

namespace TrexSL.Web.Helpers
{
    public static class SessionHelper
    {
        public static IUserSession UserSession
        {
            get { return ObjectFactory.GetInstance<IUserSession>(); }
        }

        public static ApplicationStateSession AppSession
        {
            get
            {
                var stateFactory = ObjectFactory.GetInstance<IAppStateSessionFactory>();
                return stateFactory.AppState;
            }
        }
    }
}