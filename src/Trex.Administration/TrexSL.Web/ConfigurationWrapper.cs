using System.Configuration;

namespace TrexSL.Web
{
    public static class ConfigurationWrapper
    {
        public static string Environment
        {
            get { return ConfigurationManager.AppSettings["Environment"]; }
        }
    }
}