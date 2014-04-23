using Microsoft.Practices.Unity;

namespace Trex.Server.Core.Unity
{
    public static class UnityContainerManager
    {
        private static readonly IUnityContainer Container = new UnityContainer();

        public static IUnityContainer GetInstance
        {
            get
            {
                return Container;
            }
        }
    }
}